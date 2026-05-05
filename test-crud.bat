@echo off
REM One-click ATP CRUD test runner.
REM Builds atp.exe if missing, then runs the 52-case suite via Git Bash.

setlocal
set "ROOT=%~dp0"
set "ATP=%ROOT%ATPCli\bin\Debug\atp.exe"

if not exist "%ATP%" (
    echo Building ATPCli...
    "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" "%ROOT%ATPCli\ATPCli.csproj" -v:m -nologo || exit /b 1
)

call "%ROOT%tests\crud-suite.bat" %*
exit /b %ERRORLEVEL%
