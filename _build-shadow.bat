@echo off
setlocal
"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" "C:\Dev\Plugin\ATP\ATPShadowMain\ATPShadowMain.csproj" /p:Configuration=Debug /p:Platform=AnyCPU /v:minimal /nologo
exit /b %errorlevel%
