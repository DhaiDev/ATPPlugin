@echo off
REM Windows launcher for the ATP CRUD test suite.
REM Runs tests\crud-suite.sh under Git Bash. Passes through any args.
REM
REM Usage (cmd or PowerShell):
REM   tests\crud-suite.bat
REM   tests\crud-suite.bat -v
REM   tests\crud-suite.bat T40 T41
REM Exit code = number of failed tests (0 = all green).

setlocal
set "SCRIPT_DIR=%~dp0"
set "SUITE=%SCRIPT_DIR%crud-suite.sh"

REM Prefer Git Bash in the usual install locations.
set "BASH="
if exist "C:\Program Files\Git\bin\bash.exe"       set "BASH=C:\Program Files\Git\bin\bash.exe"
if exist "C:\Program Files (x86)\Git\bin\bash.exe" set "BASH=C:\Program Files (x86)\Git\bin\bash.exe"

REM Fall back to whatever's on PATH (covers MSYS2, Cygwin, WSL wrappers).
if "%BASH%"=="" (
    where bash >nul 2>nul
    if errorlevel 1 (
        echo ERROR: bash not found. Install Git for Windows ^(https://git-scm.com^) or run the suite from Git Bash directly:
        echo     bash tests/crud-suite.sh
        exit /b 127
    )
    set "BASH=bash"
)

"%BASH%" "%SUITE%" %*
exit /b %ERRORLEVEL%
