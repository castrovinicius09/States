@ECHO OFF

REM Install tools if not present
dotnet tool install --global coverlet.console
dotnet tool install --global dotnet-reportgenerator-globaltool

REM Clean and build solution
dotnet restore States_Persistence.sln
dotnet build States_Persistence.sln --configuration Release --no-restore

REM Garantir que a pasta TestResults exista na raiz
if not exist "TestResults" mkdir TestResults

REM Run tests with coverage
dotnet test ./States_Persistence.sln --settings coverlet.runsettings ^
/p:CollectCoverage=true ^
/p:CoverletOutputFormat=cobertura ^
/p:CoverletOutput=./TestResults/coverage.cobertura.xml ^
/p:Exclude="[*]*.Program%2c[*]*.Startup%2c[*]*.Migrations.*"

REM Generate coverage report
powershell -Command ^
"$files = Get-ChildItem -Recurse -Filter 'coverage.cobertura.xml' -Path './tests' | ForEach-Object { $_.FullName }; ^
 if ($files.Count -eq 0) { Write-Host 'Nenhum arquivo de cobertura encontrado!'; exit 1 }; ^
 reportgenerator -reports:$files -targetdir:'./TestResults/CoverageReport' -reporttypes:Html"

REM Removing temporary files
rmdir /s /q bin 2>nul
rmdir /s /q obj 2>nul

echo.
echo Coverage report generated at TestResults/CoverageReport/index.html
pause