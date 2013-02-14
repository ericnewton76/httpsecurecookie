@ECHO OFF

setlocal

set Build_Config=Release

msbuild ..\src\HttpCookieEncryption.csproj /p:Configuration=%Build_Config% /p:outdir=%~dp0\bin\%Build_Config%\
