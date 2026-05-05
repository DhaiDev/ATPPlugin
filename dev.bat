@echo off
REM ============================================================
REM  ATP dev.bat — INNER LOOP (hot reload via ShadowMain)
REM ------------------------------------------------------------
REM  Builds ServiceContractPhotocopier.dll + ATPShadowMain.exe,
REM  then launches the exe. The exe boots AutoCount in-process,
REM  auto-logs in via App.config, and opens a plugin form.
REM  No .app, no Plug-in Manager, no clicks. Use 99% of the time.
REM ============================================================
setlocal
cd /d "%~dp0"

set "MSBUILD=C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
set "SHADOW=ATPShadowMain\ATPShadowMain.csproj"
set "EXE=ATPShadowMain\bin\Debug\ATPShadowMain.exe"

if not exist "%MSBUILD%" echo [ERROR] MSBuild not found && exit /b 1
if not exist "%SHADOW%"  echo [ERROR] %SHADOW% not found && exit /b 1

echo === Building ShadowMain (chains plugin + base) ===
"%MSBUILD%" "%SHADOW%" /p:Configuration=Debug /p:Platform=AnyCPU /v:minimal /nologo
if errorlevel 1 echo [ERROR] Build failed && exit /b 1

echo.
echo === Launching %EXE% ===
start "" "%EXE%"
endlocal
