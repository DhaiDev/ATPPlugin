# Run this ONCE to save your AutoCount login credentials.
# Stored encrypted with Windows DPAPI — only your Windows user can decrypt.
# Location: %APPDATA%\ATP\autocount-login.xml
#
# Usage:  powershell -ExecutionPolicy Bypass -File save-credentials.ps1

$dir = Join-Path $env:APPDATA 'ATP'
if (-not (Test-Path $dir)) { New-Item -ItemType Directory -Path $dir | Out-Null }
$path = Join-Path $dir 'autocount-login.xml'

Write-Host ''
Write-Host 'Save AutoCount login credentials (DPAPI-encrypted, current user only)'
Write-Host '----------------------------------------------------------------------'
$user = Read-Host 'AutoCount UserID'
$pass = Read-Host 'AutoCount Password' -AsSecureString

# DPAPI: SecureString -> encrypted string tied to current Windows user
$encPass = ConvertFrom-SecureString $pass

[PSCustomObject]@{
    UserId   = $user
    Password = $encPass
} | Export-Clixml -Path $path

Write-Host ''
Write-Host "Saved to: $path"
Write-Host 'Done. You can now run auto-login.ps1 / build-and-install.bat.'
