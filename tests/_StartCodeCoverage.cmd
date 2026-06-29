@echo off

REM ---------------------------------------------------------------------------
REM Code Coverage for Components (Core) and Charts test projects combined.
REM ---------------------------------------------------------------------------
REM
REM Prerequisites (install once, globally):
REM   dotnet tool install --global dotnet-reportgenerator-globaltool
REM
REM   Use this command to list installed tools:
REM   dotnet tool list --global
REM
REM How it works:
REM   Both test projects are run with the built-in "XPlat Code Coverage"
REM   DataCollector (provided by coverlet.collector). Each run writes its
REM   coverage.cobertura.xml into a unique sub-folder under tests\TestResults\.
REM   ReportGenerator then merges all cobertura files in one step and outputs
REM   the merged report to the tests\TestResults\Reports folder.
REM
REM Usage:
REM   _StartCodeCoverage.cmd           - run coverage and open the report
REM   _StartCodeCoverage.cmd /noopen   - run coverage, skip opening browser

setlocal

REM Shared TestResults root (written next to this script)
set RESULTS_DIR=%~dp0TestResults

REM Clean previous results so the glob stays accurate
if exist "%RESULTS_DIR%" rmdir /s /q "%RESULTS_DIR%"

cls
echo.
echo === Running Core component tests with coverage ===
dotnet test "%~dp0Core\Components.Tests.csproj" ^
    --collect:"XPlat Code Coverage" ^
    --results-directory "%RESULTS_DIR%"

echo.
echo === Running Charts component tests with coverage ===
dotnet test "%~dp0Charts\Components.Charts.Tests.csproj" ^
    --collect:"XPlat Code Coverage" ^
    --results-directory "%RESULTS_DIR%"

echo.
echo === Merging coverage reports ===
reportgenerator ^
    "-reports:%RESULTS_DIR%\**\coverage.cobertura.xml" ^
    "-targetdir:%RESULTS_DIR%\Report" ^
    -reporttypes:HtmlInline_AzurePipelines ^
    "-assemblyfilters:+Microsoft.FluentUI.AspNetCore.Components;+Microsoft.FluentUI.AspNetCore.Components.Charts" ^
    "-classfilters:-Microsoft.FluentUI.AspNetCore.Components.DesignTokens.*" ^
    "-filefilters:-*RegexGenerator.g.cs" ^
    riskHotspotsAnalysisThresholds:metricThresholdForCrapScore=30 ^
    riskHotspotsAnalysisThresholds:metricThresholdForCyclomaticComplexity=30 ^
    minimumCoverageThresholds:lineCoverage=98

echo.
if "%~1" neq "/noopen" start "" "%RESULTS_DIR%\Report\index.htm"

endlocal

