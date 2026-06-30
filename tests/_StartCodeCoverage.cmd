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
REM   coverage.cobertura.xml under tests\TestResults\{Core|Charts}\.
REM   ReportGenerator then merges all cobertura files in one step and outputs
REM   the merged report to tests\TestResults\Report.
REM
REM   A stamp file is written to TestResults\_stamps\ after each successful
REM   test run. On the next invocation, PowerShell checks whether any .cs,
REM   .razor, or .csproj file under the relevant source and test directories
REM   is newer than the stamp. If nothing changed the project is skipped,
REM   and the previous coverage.cobertura.xml is reused for the merged report.
REM
REM   Note: TestResults\ should be in .gitignore so stamps are local-only.
REM
REM Usage:
REM   _StartCodeCoverage.cmd           - run changed projects and open report
REM   _StartCodeCoverage.cmd /noopen   - run changed projects, skip browser
REM   _StartCodeCoverage.cmd /force    - ignore stamps, always re-run all tests

setlocal EnableDelayedExpansion

set RESULTS_DIR=%~dp0TestResults
set STAMPS_DIR=%RESULTS_DIR%\_stamps

REM Parse flags
set FORCE=
set NOOPEN=
for %%A in (%*) do (
    if /i "%%A"=="/force" set FORCE=1
    if /i "%%A"=="/noopen" set NOOPEN=1
)

if not exist "%STAMPS_DIR%" mkdir "%STAMPS_DIR%"

cls
echo === Determining which projects need to run ===

set CORE_RUN=1
set CHARTS_RUN=1

if not defined FORCE (
    set "_STAMP=%STAMPS_DIR%\core.stamp"
    set "_DIRS=%~dp0..\src\Core;%~dp0Core;%~dp0Shared"
    powershell -NoProfile -Command "$s=$env:_STAMP;$dirs=$env:_DIRS.Split(';')|Where-Object{$_};if(-not(Test-Path $s)){exit 1};$t=(Get-Item $s).LastWriteTime;$c=$dirs|ForEach-Object{Get-ChildItem $_ -Recurse -Include '*.cs','*.razor','*.csproj' -ErrorAction SilentlyContinue}|Where-Object{$_.LastWriteTime -gt $t}|Select-Object -First 1;if($c){exit 1}else{exit 0}"
    if !ERRORLEVEL!==0 set CORE_RUN=0

    set "_STAMP=%STAMPS_DIR%\charts.stamp"
    set "_DIRS=%~dp0..\src\Charts;%~dp0Charts;%~dp0Shared"
    powershell -NoProfile -Command "$s=$env:_STAMP;$dirs=$env:_DIRS.Split(';')|Where-Object{$_};if(-not(Test-Path $s)){exit 1};$t=(Get-Item $s).LastWriteTime;$c=$dirs|ForEach-Object{Get-ChildItem $_ -Recurse -Include '*.cs','*.razor','*.csproj' -ErrorAction SilentlyContinue}|Where-Object{$_.LastWriteTime -gt $t}|Select-Object -First 1;if($c){exit 1}else{exit 0}"
    if !ERRORLEVEL!==0 set CHARTS_RUN=0
)

echo.
if !CORE_RUN!==0   echo  Core   - SKIPPED (no changes detected)
if !CHARTS_RUN!==0 echo  Charts - SKIPPED (no changes detected)

if !CORE_RUN!==0 if !CHARTS_RUN!==0 (
    echo.
    echo Nothing to run. Use /force to override.
    goto :OpenReport
)

REM ---- Clean only the directories that will be re-run ----
if !CORE_RUN!==1   if exist "%RESULTS_DIR%\Core"   rmdir /s /q "%RESULTS_DIR%\Core"
if !CHARTS_RUN!==1 if exist "%RESULTS_DIR%\Charts" rmdir /s /q "%RESULTS_DIR%\Charts"

REM ---- Run Core ----
if !CORE_RUN!==1 (
    echo.
    echo === Running Core component tests with coverage ===
    dotnet test "%~dp0Core\Components.Tests.csproj" ^
        --collect:"XPlat Code Coverage" ^
        --results-directory "%RESULTS_DIR%\Core" ^
        -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Include="[Microsoft.FluentUI.AspNetCore.Components]*"
    if !ERRORLEVEL!==0 type nul > "%STAMPS_DIR%\core.stamp"
)

REM ---- Run Charts ----
if !CHARTS_RUN!==1 (
    echo.
    echo === Running Charts component tests with coverage ===
    dotnet test "%~dp0Charts\Components.Charts.Tests.csproj" ^
        --collect:"XPlat Code Coverage" ^
        --results-directory "%RESULTS_DIR%\Charts" ^
        -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Include="[Microsoft.FluentUI.AspNetCore.Components.Charts]*"
    if !ERRORLEVEL!==0 type nul > "%STAMPS_DIR%\charts.stamp"
)

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

:OpenReport
echo.
if not defined NOOPEN if exist "%RESULTS_DIR%\Report\index.htm" start "" "%RESULTS_DIR%\Report\index.htm"

endlocal

