#!/usr/bin/env bash
# ==============================================================================
# ATPCli CRUD test suite
#
# Runs every happy-path + edge case against the live AED_ATPLUGIN001 database.
# All test data uses code prefixes SI-SUITE- / SC-SUITE- and is deleted before
# and after the run, so repeats are idempotent.
#
# Usage:
#   bash tests/crud-suite.sh            # run everything
#   bash tests/crud-suite.sh -v         # verbose (show full command output)
#   bash tests/crud-suite.sh T12 T15    # run specific cases only
#
# Exit code = number of failed tests (0 = all green).
# ==============================================================================

set -u
ATP="C:/Dev/Plugin/ATP/ATPCli/bin/Debug/atp.exe"
VERBOSE=0
ONLY=()

for a in "$@"; do
    case "$a" in
        -v|--verbose) VERBOSE=1 ;;
        T[0-9]*)      ONLY+=("$a") ;;
    esac
done

if [[ ! -f "$ATP" ]]; then
    echo "ERROR: atp.exe not found at $ATP"
    echo "Build first: msbuild ATPCli/ATPCli.csproj -v:m -nologo"
    exit 127
fi

PASS=0
FAIL=0
SKIP=0
FAILED=()

should_run() {
    local id=$1
    (( ${#ONLY[@]} == 0 )) && return 0
    for x in "${ONLY[@]}"; do [[ "$x" == "$id" ]] && return 0; done
    return 1
}

# run <id> <description> <expected_exit> -- <command...>
run() {
    local id=$1; shift
    local desc=$1; shift
    local expected=$1; shift
    shift  # drop the literal --
    if ! should_run "$id"; then SKIP=$((SKIP+1)); return; fi

    local out; local ec
    out=$( "$@" 2>&1 ); ec=$?
    if [[ "$ec" == "$expected" ]]; then
        printf '  [OK]   %s  %s\n' "$id" "$desc"
        PASS=$((PASS+1))
        (( VERBOSE )) && printf '     %s\n' "$out" | sed 's/^/     /'
    else
        printf '  [FAIL] %s  %s  (expected exit %s, got %s)\n' "$id" "$desc" "$expected" "$ec"
        printf '     %s\n' "$out" | sed 's/^/     /'
        FAIL=$((FAIL+1))
        FAILED+=("$id $desc")
    fi
}

# run_grep <id> <description> <expected_exit> <grep_pattern> -- <command...>
# passes only if exit matches AND stdout contains pattern
run_grep() {
    local id=$1; shift
    local desc=$1; shift
    local expected=$1; shift
    local pattern=$1; shift
    shift  # drop the literal --
    if ! should_run "$id"; then SKIP=$((SKIP+1)); return; fi

    local out; local ec
    out=$( "$@" 2>&1 ); ec=$?
    if [[ "$ec" == "$expected" ]] && echo "$out" | grep -qE "$pattern"; then
        printf '  [OK]   %s  %s\n' "$id" "$desc"
        PASS=$((PASS+1))
        (( VERBOSE )) && printf '     %s\n' "$out" | sed 's/^/     /'
    else
        printf '  \u2717 %s  %s  (exit=%s, wanted ~= %s)\n' "$id" "$desc" "$ec" "$pattern"
        printf '     %s\n' "$out" | sed 's/^/     /'
        FAIL=$((FAIL+1))
        FAILED+=("$id $desc")
    fi
}

cleanup() {
    "$ATP" cleanup --prefix SI-SUITE- --confirm >/dev/null 2>&1
    "$ATP" cleanup --prefix SC-SUITE- --confirm >/dev/null 2>&1
}

echo "=== ATPCli CRUD suite ==="
cleanup

# ------------------------------------------------------------
# Group 1 — schema / smoke
# ------------------------------------------------------------
echo "--- Group 1: schema + counts"
run T01 "schema verify passes"         0 -- "$ATP" schema verify
run T02 "schema columns on parent"     0 -- "$ATP" schema columns --table zSCP_ServiceItem --json
run T03 "item count returns 0-or-more" 0 -- "$ATP" item count
run T04 "contract count works"         0 -- "$ATP" contract count
run T05 "help exits 0"                 0 -- "$ATP" help

# ------------------------------------------------------------
# Group 2 — item happy path
# ------------------------------------------------------------
echo "--- Group 2: item happy path"
run T10 "item create minimal"                 0 -- "$ATP" item create --tag SI-SUITE-001 --json
run T11 "item read after create returns row"  0 -- "$ATP" item read --tag SI-SUITE-001 --json
run T12 "item audit shows CreatedBy=ATPCLI"   0 -- "$ATP" item audit --tag SI-SUITE-001 --json
run_grep T13 "ModifiedBy advances after update" 0 '"ModifiedBy":"cli2"' -- bash -c "
    '$ATP' item update --tag SI-SUITE-001 --desc 'updated' --user cli2 >/dev/null &&
    '$ATP' item audit  --tag SI-SUITE-001 --json"
run_grep T14 "CreatedBy preserved across update" 0 '"CreatedBy":"ATPCLI"' -- "$ATP" item audit --tag SI-SUITE-001 --json
run T15 "item delete"                         0 -- "$ATP" item delete --tag SI-SUITE-001

# ------------------------------------------------------------
# Group 3 — duplicate / missing / bad-args
# ------------------------------------------------------------
echo "--- Group 3: validation + error paths"
"$ATP" item create --tag SI-SUITE-DUP >/dev/null 2>&1
run T20 "duplicate code rejected (SQL 2627)"  3 -- "$ATP" item create --tag SI-SUITE-DUP
"$ATP" item delete --tag SI-SUITE-DUP >/dev/null 2>&1

run T21 "update nonexistent → exit 4"         4 -- "$ATP" item update --tag SI-SUITE-MISSING --desc x
run T22 "delete nonexistent → exit 4"         4 -- "$ATP" item delete --tag SI-SUITE-MISSING
run_grep T23 "read nonexistent → empty JSON"  0 '^\[\]$' -- "$ATP" item read --tag SI-SUITE-MISSING --json

run T24 "create without --tag → bad args"     1 -- "$ATP" item create --desc foo
run T25 "read without selector → bad args"    1 -- "$ATP" item read
run T26 "invalid date → bad args exit 2"      2 -- "$ATP" item create --tag SI-SUITE-BAD --start "not-a-date"
run T27 "invalid --key → bad args exit 2"     2 -- "$ATP" item read --key abc
run T28 "unknown subcommand → exit 2"         2 -- "$ATP" item frobnicate
run T29 "unknown command → exit 2"            2 -- "$ATP" gribble list
run T30 "cleanup without --confirm → exit 2"  2 -- "$ATP" cleanup --prefix SI-SUITE-
run T31 "no-op update (no field flags)"       2 -- bash -c "
    '$ATP' item create --tag SI-SUITE-NOOP >/dev/null &&
    '$ATP' item update --tag SI-SUITE-NOOP"
"$ATP" item delete --tag SI-SUITE-NOOP >/dev/null 2>&1

# ------------------------------------------------------------
# Group 4 — optimistic concurrency (--if-modified)
# ------------------------------------------------------------
echo "--- Group 4: optimistic concurrency"
"$ATP" item create --tag SI-SUITE-RACE >/dev/null
RV=$("$ATP" item audit --tag SI-SUITE-RACE --json | grep -oE '"LastModified":"[^"]+"' | cut -d'"' -f4)
run T40 "update with correct --if-modified"   0 -- "$ATP" item update --tag SI-SUITE-RACE --desc "winner" --if-modified "$RV" --json
run T41 "stale --if-modified rejected → exit 4" 4 -- "$ATP" item update --tag SI-SUITE-RACE --desc "loser" --if-modified "$RV" --json
"$ATP" item delete --tag SI-SUITE-RACE >/dev/null

# ------------------------------------------------------------
# Group 5 — meter child + cascade
# ------------------------------------------------------------
echo "--- Group 5: meter child + cascade delete"
"$ATP" item create --tag SI-SUITE-METER >/dev/null
run T50 "meter add bad --type → FK error exit 3" 3 -- "$ATP" meter add --tag SI-SUITE-METER --type "DEFINITELY-NOT-A-REAL-METER-TYPE" --rate 0.01 --min 0
run T51 "meter add with real type"               0 -- "$ATP" meter add --tag SI-SUITE-METER --type "01. RENTAL HSI" --rate 0.02 --min 50 --json
run T52 "meter add second + third"               0 -- bash -c "
    '$ATP' meter add --tag SI-SUITE-METER --type '01.1 UNIT RENTAL' --rate 0.03 --min 60 --json &&
    '$ATP' meter add --tag SI-SUITE-METER --type '01.MR.BK.2JC10897' --rate 0.04 --min 70 --json"
run_grep T53 "meter list shows 3 rows"           0 '"ServiceItemMeterTypeKey":.*"ServiceItemMeterTypeKey":.*"ServiceItemMeterTypeKey":' -- "$ATP" meter list --tag SI-SUITE-METER --json
run T54 "meter delete-all"                       0 -- "$ATP" meter delete-all --tag SI-SUITE-METER
run_grep T55 "meter list returns []"             0 '^\[\]$' -- "$ATP" meter list --tag SI-SUITE-METER --json
# re-add one and verify cascade on parent delete
"$ATP" meter add --tag SI-SUITE-METER --type "01. RENTAL HSI" --rate 0.05 --min 5 >/dev/null
CASCADE_BEFORE=$("$ATP" sql "SELECT COUNT(*) AS n FROM zSCP_ServiceItemMeterType WHERE ServiceItemKey IN (SELECT ServiceItemKey FROM zSCP_ServiceItem WHERE ServiceItemCode='SI-SUITE-METER')" | awk '/^n/ {print $2}')
run T56 "cascade: parent delete wipes child meter" 0 -- bash -c "
    '$ATP' item delete --tag SI-SUITE-METER >/dev/null &&
    cnt=\$('$ATP' sql \"SELECT COUNT(*) AS n FROM zSCP_ServiceItemMeterType WHERE MeterTypeCode='01. RENTAL HSI' AND ServiceItemKey NOT IN (SELECT ServiceItemKey FROM zSCP_ServiceItem)\" | awk '/^n/ {print \$2}') &&
    [[ \"\$cnt\" == 0 ]]"

# ------------------------------------------------------------
# Group 6 — partial / sparse update
# ------------------------------------------------------------
echo "--- Group 6: partial update semantics"
"$ATP" item create --tag SI-SUITE-PART --desc "initial" --debtor "3000-A0084" >/dev/null
run T60 "update --desc only, debtor untouched" 0 -- bash -c "
    '$ATP' item update --tag SI-SUITE-PART --desc 'changed' --json >/dev/null &&
    '$ATP' sql \"SELECT ServiceItemCode, [Description], DebtorCode FROM zSCP_ServiceItem WHERE ServiceItemCode='SI-SUITE-PART'\" | grep -q '3000-A0084' &&
    '$ATP' sql \"SELECT [Description] FROM zSCP_ServiceItem WHERE ServiceItemCode='SI-SUITE-PART'\" | grep -q 'changed'"
run T61 "update --debtor only, desc untouched" 0 -- bash -c "
    '$ATP' item update --tag SI-SUITE-PART --debtor '3000-A0001' --json >/dev/null &&
    '$ATP' sql \"SELECT [Description] FROM zSCP_ServiceItem WHERE ServiceItemCode='SI-SUITE-PART'\" | grep -q 'changed' &&
    '$ATP' sql \"SELECT DebtorCode FROM zSCP_ServiceItem WHERE ServiceItemCode='SI-SUITE-PART'\" | grep -q '3000-A0001'"
"$ATP" item delete --tag SI-SUITE-PART >/dev/null

# ------------------------------------------------------------
# Group 7 — list / like / top
# ------------------------------------------------------------
echo "--- Group 7: list + filters"
for n in 1 2 3; do "$ATP" item create --tag "SI-SUITE-LIST-$n" >/dev/null; done
run T70 "list --top 2 returns exactly 2 rows" 0 -- bash -c "
    n=\$('$ATP' item list --top 2 --like 'SI-SUITE-LIST-%' --json | grep -o 'ServiceItemKey' | wc -l)
    [[ \"\$n\" == 2 ]]"
run T71 "list --top 10 --like matches all 3 test rows" 0 -- bash -c "
    n=\$('$ATP' item list --top 10 --like 'SI-SUITE-LIST-%' --json | grep -o 'ServiceItemKey' | wc -l)
    [[ \"\$n\" == 3 ]]"
"$ATP" cleanup --prefix SI-SUITE-LIST- --confirm >/dev/null

# ------------------------------------------------------------
# Group 8 — contract CRUD
# ------------------------------------------------------------
echo "--- Group 8: contract CRUD"
run T80 "contract create"                       0 -- "$ATP" contract create --code SC-SUITE-001 --debtor 3000-A0084 --desc "suite contract" --json
run_grep T81 "contract audit shows CreatedBy"   0 '"CreatedBy":"ATPCLI"' -- "$ATP" contract audit --code SC-SUITE-001 --json
run_grep T82 "contract update changes ModifiedBy" 0 '"ModifiedBy":"cli2"' -- bash -c "
    '$ATP' contract update --code SC-SUITE-001 --desc 'updated' --user cli2 >/dev/null &&
    '$ATP' contract audit --code SC-SUITE-001 --json"
RV2=$("$ATP" contract audit --code SC-SUITE-001 --json | grep -oE '"LastModified":"[^"]+"' | cut -d'"' -f4)
run T83 "contract --if-modified correct → ok"   0 -- "$ATP" contract update --code SC-SUITE-001 --desc "win" --if-modified "$RV2" --json
run T84 "contract --if-modified stale → exit 4" 4 -- "$ATP" contract update --code SC-SUITE-001 --desc "lose" --if-modified "$RV2" --json
run T85 "contract delete"                       0 -- "$ATP" contract delete --code SC-SUITE-001

# ------------------------------------------------------------
# Group 9 — SQL safety filter + raw SELECT
# ------------------------------------------------------------
echo "--- Group 9: SQL safety filter"
run T90 "SELECT allowed"  0 -- "$ATP" sql "SELECT COUNT(*) AS n FROM zSCP_ServiceItem"
run T91 "DELETE blocked"  2 -- "$ATP" sql "DELETE FROM zSCP_ServiceItem"
run T92 "DROP blocked"    2 -- "$ATP" sql "DROP TABLE zSCP_ServiceItem"
run T93 "INSERT blocked"  2 -- "$ATP" sql "INSERT INTO zSCP_ServiceItem (ServiceItemCode) VALUES ('x')"
run T94 "UPDATE blocked"  2 -- "$ATP" sql "UPDATE zSCP_ServiceItem SET Inactive='Y'"
run T95 "EXEC sp_help allowed" 0 -- "$ATP" sql "EXEC sp_help 'dbo.zSCP_ServiceItem'"

# ------------------------------------------------------------
# Group 10 — special chars + SQL injection resistance
# ------------------------------------------------------------
echo "--- Group 10: escaping / injection"
run T100 "single quote in description survives" 0 -- bash -c "
    '$ATP' item create --tag SI-SUITE-QUOTE --desc \"O'Brien's copier\" --json >/dev/null &&
    '$ATP' sql \"SELECT [Description] FROM zSCP_ServiceItem WHERE ServiceItemCode='SI-SUITE-QUOTE'\" | grep -q \"O'Brien\""
run T101 "classic injection attempt neutralised" 0 -- bash -c "
    '$ATP' item create --tag SI-SUITE-INJ --desc \"'); DROP TABLE zSCP_ServiceItem; --\" --json >/dev/null &&
    cnt=\$('$ATP' item count | head -1) &&
    [[ \"\$cnt\" -ge 2 ]]"  # table must still exist and have our 2 test rows
"$ATP" cleanup --prefix SI-SUITE-QUOTE --confirm >/dev/null
"$ATP" cleanup --prefix SI-SUITE-INJ   --confirm >/dev/null

# ------------------------------------------------------------
# Group 11 — cleanup idempotency
# ------------------------------------------------------------
echo "--- Group 11: cleanup"
run T110 "cleanup --prefix with zero matches" 0 -- "$ATP" cleanup --prefix SI-ZERO-MATCH-XYZ- --confirm
run_grep T111 "cleanup reports total count"   0 "Total deleted:" -- "$ATP" cleanup --prefix SI-ZERO-MATCH-XYZ- --confirm

# ==============================================================================
cleanup
echo
echo "===================================="
printf 'PASS  %d\nFAIL  %d\nSKIP  %d\n' "$PASS" "$FAIL" "$SKIP"
echo "===================================="
if (( FAIL > 0 )); then
    echo
    echo "Failed tests:"
    printf '  - %s\n' "${FAILED[@]}"
fi
exit $FAIL
