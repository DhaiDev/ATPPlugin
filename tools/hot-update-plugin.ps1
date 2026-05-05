# ============================================================
#  ATP Plugin: hot-update DLLs straight into AutoCount's DB
#  - bypass the .app install dialog entirely.
#
#  How AutoCount loads plugins (verified against C:\Dev\Autocount):
#    AutoCount\AutoCount.PlugIn\PlugInManager.cs ~line 173 (LoadPlugInByGuid)
#      1. Reads PlugIn.Signature from the SQL DB.
#      2. Reads the local checksum file at
#         %APPDATA%\AutoCount\Accounting\<DB>@<server>\Plugin Cache\
#           <guid-N-lowercase>\checksum
#      3. If they don't match (or file missing) -> re-extracts every
#         row in PlugInFiles for this Guid into the cache folder, then
#         loads the AssemblyFile from disk.
#
#  So the dev hot-swap is simple:
#    a) build -> fresh bytes in bin\Debug\
#    b) UPDATE PlugInFiles.FileImage with the new bytes
#    c) UPDATE PlugIn.Signature to a brand-new value
#    d) wipe the on-disk Plugin Cache\<guid> folder (belt-and-suspenders)
#    e) restart AutoCount -> it sees signature mismatch / no checksum ->
#       re-extracts from DB -> loads new DLL. No dialog.
# ============================================================

[CmdletBinding()]
param(
    [string]$PluginGuid = '6A996121-169E-4D35-AEED-58CFBB1386B7',
    [string]$ServerName = 'localhost,1433',
    [string]$Database   = 'AED_ATPLUGIN001',
    [string]$DbUser     = 'sa',
    [string]$DbPassword = 'rs6663',
    [string]$BinDir     = 'C:\Dev\Plugin\ATP\ServiceContractPhotocopier\bin\Debug',
    [string[]]$Files    = @(
        'ServiceContractPhotocopier.dll',
        'ServiceContractPhotocopier.pdb',
        'VecTech.ACPluginBase.dll',
        'VecTech.ACPluginBase.pdb'
    ),
    [switch]$Restart
)

$ErrorActionPreference = 'Stop'

function Write-Step($msg) { Write-Host "  $msg" -ForegroundColor Cyan }
function Write-Ok  ($msg) { Write-Host "  [OK] $msg" -ForegroundColor Green }
function Write-Warn($msg) { Write-Host "  [WARN] $msg" -ForegroundColor Yellow }

# AC stores each PlugInFiles.FileImage as a single-entry ZIP archive.
# The entry MUST be literally named "data" — see PlugInManager.UncompressData
# / PlugInManager.CompressData in C:\Dev\Autocount\AutoCount\AutoCount.PlugIn\.
# Writing raw bytes (no zip framing) trips "End of Central Directory record could not be found"
# on the next plugin load and AC refuses to start the plugin.
Add-Type -AssemblyName System.IO.Compression
function Compress-ToZipBlob([byte[]]$bytes) {
    $ms = New-Object System.IO.MemoryStream
    $zip = New-Object System.IO.Compression.ZipArchive($ms, [System.IO.Compression.ZipArchiveMode]::Create, $true)
    try {
        $entry = $zip.CreateEntry('data')
        $stream = $entry.Open()
        try { $stream.Write($bytes, 0, $bytes.Length) } finally { $stream.Dispose() }
    } finally { $zip.Dispose() }
    $out = $ms.ToArray()
    $ms.Dispose()
    return ,$out
}

Write-Host ""
Write-Host "=== ATP Plugin Hot-Update ===" -ForegroundColor Magenta
Write-Host "  Guid   : $PluginGuid"
Write-Host "  DB     : $Database on $ServerName"
Write-Host "  Source : $BinDir"
Write-Host ""

# 1) Validate inputs
if (-not (Test-Path $BinDir)) { throw "BinDir not found: $BinDir" }
$missing = @()
foreach ($f in $Files) {
    if (-not (Test-Path (Join-Path $BinDir $f))) { $missing += $f }
}
if ($missing.Count -gt 0) {
    throw ('Missing build artifacts in {0}: {1}' -f $BinDir, ($missing -join ', '))
}

# 2) Stop AutoCount if it is holding the cached DLL open
$ac = Get-Process -Name Accounting -ErrorAction SilentlyContinue
if ($ac) {
    Write-Step "AutoCount is running (pid $($ac.Id)) - stopping so cache can be replaced."
    $ac | Stop-Process -Force
    Start-Sleep -Milliseconds 500
}

# 3) Connect to SQL Server
Add-Type -AssemblyName System.Data
$cnstr = "Data Source=$ServerName;Initial Catalog=$Database;User Id=$DbUser;Password=$DbPassword;Encrypt=False;TrustServerCertificate=True"
$cn = New-Object System.Data.SqlClient.SqlConnection
$cn.ConnectionString = $cnstr
$cn.Open()

try {
    # Confirm plugin row exists
    $check = $cn.CreateCommand()
    $check.CommandText = 'SELECT Name, Version FROM PlugIn WHERE Guid=@g'
    [void]$check.Parameters.AddWithValue('@g', [Guid]$PluginGuid)
    $rdr = $check.ExecuteReader()
    if (-not $rdr.Read()) {
        $rdr.Close()
        throw "Plugin Guid $PluginGuid not found in PlugIn table - install via .app at least once first."
    }
    $name = $rdr['Name']
    $ver  = $rdr['Version']
    $rdr.Close()
    Write-Step "Updating $name v$ver ..."

    # 4) UPDATE / INSERT PlugInFiles for each file
    $now    = (Get-Date).ToUniversalTime()
    $updSql = @'
UPDATE PlugInFiles
   SET FileImage         = @img,
       LastWriteTimeUtc  = @t,
       LastAccessTimeUtc = @t
 WHERE Guid = @g AND FileName = @n
'@
    $insSql = @'
INSERT INTO PlugInFiles
       (Guid, FileName, CreationTimeUtc, LastAccessTimeUtc,
        LastWriteTimeUtc, ExecuteAfterExtracted, FileImage)
VALUES (@g,   @n,       @t,              @t,
        @t,                'F',                  @img)
'@
    foreach ($f in $Files) {
        $rawBytes = [System.IO.File]::ReadAllBytes((Join-Path $BinDir $f))
        $blob     = Compress-ToZipBlob $rawBytes
        $upd = $cn.CreateCommand()
        $upd.CommandText = $updSql
        [void]$upd.Parameters.AddWithValue('@img', $blob)
        [void]$upd.Parameters.AddWithValue('@t',   $now)
        [void]$upd.Parameters.AddWithValue('@g',   [Guid]$PluginGuid)
        [void]$upd.Parameters.AddWithValue('@n',   $f)
        $rows = $upd.ExecuteNonQuery()
        if ($rows -eq 0) {
            Write-Warn "$f - no row in PlugInFiles. Inserting fresh row."
            $ins = $cn.CreateCommand()
            $ins.CommandText = $insSql
            [void]$ins.Parameters.AddWithValue('@g',   [Guid]$PluginGuid)
            [void]$ins.Parameters.AddWithValue('@n',   $f)
            [void]$ins.Parameters.AddWithValue('@t',   $now)
            [void]$ins.Parameters.AddWithValue('@img', $blob)
            [void]$ins.ExecuteNonQuery()
        }
        $rawKb  = [math]::Round($rawBytes.Length / 1024, 1)
        $blobKb = [math]::Round($blob.Length     / 1024, 1)
        $msg = "$f  raw=$rawKb KB  zip=$blobKb KB"
        Write-Ok $msg
    }

    # 5) Bump PlugIn.Signature so the cached checksum no longer matches
    $sig = [Guid]::NewGuid().ToString('N').ToUpper()
    $bump = $cn.CreateCommand()
    $bump.CommandText = 'UPDATE PlugIn SET Signature=@s WHERE Guid=@g'
    [void]$bump.Parameters.AddWithValue('@s', $sig)
    [void]$bump.Parameters.AddWithValue('@g', [Guid]$PluginGuid)
    [void]$bump.ExecuteNonQuery()
    Write-Ok "PlugIn.Signature bumped to $sig"
}
finally {
    $cn.Close()
}

# 6) Wipe on-disk cache for this Guid in every account-book folder
$guidN     = ([Guid]$PluginGuid).ToString('N').ToLowerInvariant()
$cacheRoot = Join-Path $env:APPDATA 'AutoCount\Accounting'
$wiped     = 0
if (Test-Path $cacheRoot) {
    Get-ChildItem $cacheRoot -Directory -ErrorAction SilentlyContinue | ForEach-Object {
        $sub = Join-Path $_.FullName "Plugin Cache\$guidN"
        if (Test-Path $sub) {
            try {
                Remove-Item $sub -Recurse -Force
                Write-Ok "Cache wiped: $sub"
                $wiped++
            } catch {
                Write-Warn ("Could not wipe {0} - {1}" -f $sub, $_.Exception.Message)
            }
        }
    }
}
if ($wiped -eq 0) {
    Write-Step 'No on-disk cache folders found (will be created fresh on next launch).'
}

Write-Host ''
Write-Host 'Done. Next AutoCount launch will re-extract and load the new DLL.' -ForegroundColor Green

if ($Restart) {
    $exe = 'C:\Program Files\AutoCount\Accounting 2.2\Accounting.exe'
    if (Test-Path $exe) {
        Write-Host ''
        Write-Step 'Launching AutoCount...'
        Start-Process -FilePath $exe
    } else {
        Write-Warn "Accounting.exe not at $exe - launch manually."
    }
}
