@echo off
REM ============================================================
REM  ATP Plugin: hot-update — build + push DLLs into AC's DB,
REM  bypassing the .app install dialog. ~3 seconds end-to-end.
REM
REM  Use this for INNER-loop dev iteration when you want to test
REM  inside real AutoCount (not ShadowMain). For ShadowMain alone
REM  you don't need this — ProjectReference handles it.
REM
REM  Add  "restart"  as an arg to also relaunch Accounting.exe.
REM ============================================================
setlocal
cd /d "%~dp0"

set "MSBUILD=C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
set "CSPROJ=ServiceContractPhotocopier\ServiceContractPhotocopier.csproj"

if not exist "%MSBUILD%" ( echo [ERROR] MSBuild not found & exit /b 1 )
if not exist "%CSPROJ%"  ( echo [ERROR] csproj not found  & exit /b 1 )

echo.
echo === [1/2] Build ===
"%MSBUILD%" "%CSPROJ%" /p:Configuration=Debug /p:Platform=AnyCPU /v:minimal /nologo
if errorlevel 1 ( echo [ERROR] Build failed. & exit /b 1 )

echo.
echo === [2/2] Push DLLs into PlugInFiles ===
if /I "%~1"=="restart" (
    powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0tools\hot-update-plugin.ps1" -Restart
) else (
    powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0tools\hot-update-plugin.ps1"
)
if errorlevel 1 ( echo [ERROR] Hot-update failed. & exit /b 1 )

endlocal
