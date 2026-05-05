@echo off
setlocal
cd /d "%~dp0"

set "CSPROJ=ServiceContractPhotocopier\ServiceContractPhotocopier.csproj"
set "APPP=ServiceContractPhotocopier\ServiceContractPhotocopier.appp"
set "OUTPUT_APP=ServiceContractPhotocopier\ServiceContractPhotocopier.app"
set "MSBUILD=C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
set "APPBUILDERCMD=C:\Program Files (x86)\AutoCount\Development\AppBuilder 2.1\AppBuilderCmd.exe"

echo === [1/2] Building %CSPROJ% ===
"%MSBUILD%" "%CSPROJ%" /t:Rebuild /p:Configuration=Debug /p:Platform=AnyCPU /v:minimal /nologo
if errorlevel 1 ( echo [ERROR] MSBuild failed. & exit /b 1 )

echo.
echo === [2/2] Packaging %APPP% -^> %OUTPUT_APP% ===
"%APPBUILDERCMD%" "%~dp0%APPP%" "%~dp0%OUTPUT_APP%"
if errorlevel 1 ( echo [ERROR] AppBuilderCmd failed. & exit /b 1 )

echo.
echo Done.
endlocal
