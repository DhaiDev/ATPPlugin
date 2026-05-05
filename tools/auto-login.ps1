# Restart AutoCount and auto-fill the login dialog.
#
# Flow:
#   1. Kill any running Accounting.exe (so the new plugin actually loads)
#   2. Launch Accounting.exe
#   3. Wait for the login window to appear
#   4. Type UserID -> Tab -> Password -> Enter
#
# Credentials come from %APPDATA%\ATP\autocount-login.xml
# (created by save-credentials.ps1, DPAPI-encrypted to current user).
#
# Usage:  powershell -ExecutionPolicy Bypass -File auto-login.ps1
#         powershell -ExecutionPolicy Bypass -File auto-login.ps1 -AccountingExe "C:\Path\Accounting.exe"

param(
    [string]$AccountingExe = "C:\Program Files (x86)\AutoCount\Accounting 2.1\Accounting.exe",
    [int]$LoginWindowTimeoutSec = 60
)

$ErrorActionPreference = 'Stop'

# ---------- Load credentials ----------
$credPath = Join-Path $env:APPDATA 'ATP\autocount-login.xml'
if (-not (Test-Path $credPath)) {
    Write-Host "[ERROR] No saved credentials at $credPath" -ForegroundColor Red
    Write-Host "        Run save-credentials.ps1 first." -ForegroundColor Yellow
    exit 1
}
$cred = Import-Clixml $credPath
$userId    = $cred.UserId
$secure    = ConvertTo-SecureString $cred.Password    # DPAPI decrypt
$bstr      = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($secure)
$plainPass = [Runtime.InteropServices.Marshal]::PtrToStringAuto($bstr)
[Runtime.InteropServices.Marshal]::ZeroFreeBSTR($bstr) | Out-Null

# ---------- Kill existing Accounting.exe ----------
Write-Host '[1/4] Killing any running Accounting.exe...'
Get-Process Accounting -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue
Start-Sleep -Milliseconds 800

# ---------- Launch ----------
if (-not (Test-Path $AccountingExe)) {
    Write-Host "[ERROR] Accounting.exe not found at: $AccountingExe" -ForegroundColor Red
    exit 1
}
Write-Host "[2/4] Launching $AccountingExe ..."
Start-Process -FilePath $AccountingExe | Out-Null

# ---------- Wait for login window ----------
Add-Type -AssemblyName System.Windows.Forms
Write-Host "[3/4] Waiting up to $LoginWindowTimeoutSec s for login window..."
$deadline = (Get-Date).AddSeconds($LoginWindowTimeoutSec)
$loginProc = $null
while ((Get-Date) -lt $deadline) {
    $loginProc = Get-Process Accounting -ErrorAction SilentlyContinue |
        Where-Object { $_.MainWindowTitle -match 'Login|登入|登录' } |
        Select-Object -First 1
    if ($loginProc) { break }
    Start-Sleep -Milliseconds 500
}
if (-not $loginProc) {
    Write-Host '[WARN] Did not detect a login-titled window. Will try to send keys to whatever Accounting window is foreground.' -ForegroundColor Yellow
    $loginProc = Get-Process Accounting -ErrorAction SilentlyContinue | Select-Object -First 1
    if (-not $loginProc) { Write-Host '[ERROR] No Accounting.exe process found.' -ForegroundColor Red; exit 1 }
}

# ---------- Send credentials ----------
Write-Host '[4/4] Sending credentials...'

# Bring window to front
Add-Type @'
using System;
using System.Runtime.InteropServices;
public class Win {
    [DllImport("user32.dll")] public static extern bool SetForegroundWindow(IntPtr hWnd);
    [DllImport("user32.dll")] public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
}
'@
[Win]::ShowWindowAsync($loginProc.MainWindowHandle, 9) | Out-Null   # SW_RESTORE
[Win]::SetForegroundWindow($loginProc.MainWindowHandle) | Out-Null
Start-Sleep -Milliseconds 500

# UserID may already be focused or pre-filled — clear and type
[System.Windows.Forms.SendKeys]::SendWait('^a')
Start-Sleep -Milliseconds 50
[System.Windows.Forms.SendKeys]::SendWait($userId)
Start-Sleep -Milliseconds 100
[System.Windows.Forms.SendKeys]::SendWait('{TAB}')
Start-Sleep -Milliseconds 100
# Escape special SendKeys characters in password
$escaped = $plainPass -replace '([+^%~(){}\[\]])','{$1}'
[System.Windows.Forms.SendKeys]::SendWait($escaped)
Start-Sleep -Milliseconds 100
[System.Windows.Forms.SendKeys]::SendWait('{ENTER}')

# Wipe local plaintext from memory
$plainPass = $null
$escaped = $null
[GC]::Collect()

Write-Host 'Auto-login keys sent.'
