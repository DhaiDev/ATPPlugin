# ATP CRUD Test Suite

Reusable end-to-end tests for the ATP plugin DB, driven through `atp.exe`
(the `ATPCli` console). Every case uses code prefixes `SI-SUITE-` / `SC-SUITE-`
and auto-cleans before + after, so it's safe to run on the live
`AED_ATPLUGIN001` database at any time.

## Run

Three equivalent entry points — pick whichever matches your shell:

```bat
REM Windows cmd / PowerShell / double-click
test-crud.bat                         REM builds atp.exe if needed, then runs all 52 cases
test-crud.bat -v                      REM verbose
test-crud.bat T40 T41 T56             REM specific cases by ID
```

```bash
# Git Bash (WSL, Cygwin)
bash tests/crud-suite.sh              # direct
bash tests/crud-suite.sh -v
bash tests/crud-suite.sh T40 T41 T56
```

Exit code = number of failed tests. `0` = all green.

The `.sh` script needs a POSIX shell (bash + grep/awk/sed/wc). On Windows that
means Git Bash is installed (it is, if you have `git` on PATH); the
`test-crud.bat` and `tests/crud-suite.bat` wrappers find it automatically.

## Catalog

### Group 1 — schema + smoke (T01-T05)
| ID  | What it asserts |
|-----|-----|
| T01 | `schema verify` → exit 0; v1.2.0 (`PMNextServiceDate`) + v1.3.0 (`CreatedBy`, `ModifiedBy`) columns are present on both master tables. |
| T02 | `schema columns --table zSCP_ServiceItem --json` runs and returns column metadata. |
| T03 | `item count` returns a number (starts at 0 after cleanup). |
| T04 | `contract count` runs. |
| T05 | `atp help` exits 0. |

### Group 2 — item happy path (T10-T15)
| ID  | What it asserts |
|-----|-----|
| T10 | `item create --tag SI-SUITE-001` succeeds, returns JSON with `"ok": true`. |
| T11 | `item read` finds the just-created row. |
| T12 | `item audit` shows `CreatedBy=ATPCLI`, `ModifiedBy=ATPCLI`. |
| T13 | After `item update --user cli2`, the audit now shows `ModifiedBy=cli2`. |
| T14 | After the update, `CreatedBy` is **still** `ATPCLI` (insert-time stamp preserved). |
| T15 | `item delete` succeeds. |

### Group 3 — validation + error paths (T20-T31)
| ID  | What it asserts |
|-----|-----|
| T20 | Creating a duplicate `ServiceItemCode` → SQL error 2627, exit **3**. |
| T21 | `item update` on a missing code → exit **4**. |
| T22 | `item delete` on a missing code → exit **4**. |
| T23 | `item read --json` on a missing code → `[]`, exit 0. |
| T24 | `item create` without `--tag` → exit **1** (missing required flag). |
| T25 | `item read` with no selector → exit 1. |
| T26 | Invalid date (`--start "not-a-date"`) → exit **2** (bad args). |
| T27 | `--key abc` (non-numeric) → exit **2**. |
| T28 | Unknown subcommand (`item frobnicate`) → exit **2**. |
| T29 | Unknown noun (`gribble list`) → exit **2** + help text. |
| T30 | `cleanup --prefix X` without `--confirm` → exit **2** (refuses). |
| T31 | `item update` with no field flags → exit **2** with "No fields to update". |

### Group 4 — optimistic concurrency (T40-T41)
Captures `LastModified` from an `item audit --json`, then tries two UPDATEs using that value in `--if-modified`:
| ID  | What it asserts |
|-----|-----|
| T40 | First UPDATE (correct RV) succeeds, `rows=1`. |
| T41 | Second UPDATE (now-stale RV) fails with "Concurrency conflict - LastModified changed.", exit **4**. |

### Group 5 — meter child + cascade (T50-T56)
| ID  | What it asserts |
|-----|-----|
| T50 | `meter add` with a fake `--type` → FK violation, exit **3**. |
| T51 | `meter add` with a real seed `MeterTypeCode` (`01. RENTAL HSI`) → ok. |
| T52 | Add 2 more meters → all ok. |
| T53 | `meter list --json` returns 3 rows (joined with `zSCP_MeterType.Description`). |
| T54 | `meter delete-all` clears them. |
| T55 | `meter list` now returns `[]`. |
| T56 | Re-add a meter, then DELETE the parent item → child meter is auto-CASCADE-removed (verified via raw `SELECT COUNT(*)`). |

### Group 6 — partial update (T60-T61)
The CLI builds a sparse `SET` clause from whatever flags are present.
| ID  | What it asserts |
|-----|-----|
| T60 | Updating only `--desc` leaves `DebtorCode` untouched. |
| T61 | Updating only `--debtor` leaves `[Description]` untouched. |

### Group 7 — list / filters (T70-T71)
| ID  | What it asserts |
|-----|-----|
| T70 | `item list --top 2 --like SI-SUITE-LIST-%` returns exactly 2 rows. |
| T71 | `item list --top 10 --like SI-SUITE-LIST-%` lists all 3 test rows (order-insensitive). |

### Group 8 — contract CRUD (T80-T85)
Mirrors the item tests against `zSCP_ServiceContract`:
| ID  | What it asserts |
|-----|-----|
| T80 | `contract create`. |
| T81 | Audit shows `CreatedBy`. |
| T82 | Update bumps `ModifiedBy`. |
| T83 | `--if-modified` correct RV succeeds. |
| T84 | `--if-modified` stale RV → exit **4**. |
| T85 | Delete. |

### Group 9 — SQL safety filter (T90-T95)
The raw `atp sql "..."` accepts SELECT / WITH / `EXEC sp_help` only.
| ID  | Blocks…/Allows… |
|-----|-----|
| T90 | SELECT ✓ |
| T91 | DELETE ✗ exit 2 |
| T92 | DROP ✗ exit 2 |
| T93 | INSERT ✗ exit 2 |
| T94 | UPDATE ✗ exit 2 |
| T95 | `EXEC sp_help` ✓ |

### Group 10 — escaping + SQL injection (T100-T101)
| ID  | What it asserts |
|-----|-----|
| T100 | A `'` in a text value survives the round trip (parameterised insert works). |
| T101 | A classic `'); DROP TABLE zSCP_ServiceItem; --` payload ends up as literal text; the table is still there. |

### Group 11 — cleanup idempotency (T110-T111)
| ID  | What it asserts |
|-----|-----|
| T110 | `cleanup` with a prefix that matches 0 rows → exit 0. |
| T111 | Cleanup output always includes `Total deleted:` line. |

---

## What's *not* yet in the suite (follow-up tickets)

- **Transaction rollback on child failure** — the form's `OnSave` wraps parent + all child inserts in one `SqlTransaction`, so a mid-save failure rolls the parent back too. The CLI issues one SQL per call, so this can only be exercised through the WinForms path. TODO: add a harness that drives a form's `OnSave` directly.
- **Multi-process concurrency** — the `--if-modified` path is tested single-process. True racing would need two subshells updating simultaneously.
- **nvarchar max-length overruns** — haven't hit the 50-char business-code limit or the 256-char address limit explicitly.
- **Date boundary conditions** — min/max SQL date, DST transitions, year 9999.
- **Decimal precision** — `ChargesRate` is `decimal(18,6)`; we never push past 6 dp.
- **Case sensitivity in codes** — SQL Server is CI by default; we assume but don't assert.
- **Reports / views** — `zvSCP_*` views are not tested.
- **UI smoke** — actual form load/save/delete paths still need an interactive walk-through; the CLI covers the DB but not the WinForms code-behind.

## Known CLI limitations (documented in NOTES.md)

- Unicode strings passed through Bash on Windows may lose chars to cp850. Workaround: call `atp.exe` from a UTF-8-aware host (Windows Terminal with `@chcp 65001`, or PowerShell `$OutputEncoding = [Text.Encoding]::UTF8`).
- Non-JSON tabular output uses Unicode box-drawing chars that render as `???` in cp850. JSON mode unaffected.
- Audit timestamps: `Created`/`Modified` come from `DateTime.UtcNow` in the CLI but `GETDATE()` (local) in the form — same instant, different displayed tz. Doesn't break concurrency checks.
