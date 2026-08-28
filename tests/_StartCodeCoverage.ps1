
# ---------------------------------------------------------------------------
# Code Coverage for Components (Core) and Charts test projects combined.
# ---------------------------------------------------------------------------
#
# Prerequisites (install once, globally):
#   dotnet tool install --global dotnet-reportgenerator-globaltool
#
#   Use this command to list installed tools:
#   dotnet tool list --global
#
# How it works:
#   Both test projects are run with the built-in "XPlat Code Coverage"
#   DataCollector (provided by coverlet.collector). Each run writes its
#   coverage.cobertura.xml under tests\TestResults\{Core|Charts}\.
#   ReportGenerator then merges all cobertura files in one step and outputs
#   the merged report to tests\TestResults\Report.
#
#   A stamp file is written to TestResults\_stamps\ after each successful
#   test run. On the next invocation, PowerShell checks whether any .cs,
#   .razor, or .csproj file under the relevant source and test directories
#   is newer than the stamp. If nothing changed the project is skipped,
#   and the previous coverage.cobertura.xml is reused for the merged report.
#
#   Note: TestResults\ should be in .gitignore so stamps are local-only.
#
# Usage:
#   _StartCodeCoverage.ps1           - run changed projects and open report
#   _StartCodeCoverage.ps1 /noopen   - run changed projects, skip browser
#   _StartCodeCoverage.ps1 /force    - ignore stamps, always re-run all tests

param(
    [switch]$Force,
    [switch]$NoOpen,
    [Parameter(ValueFromRemainingArguments = $true)]
    [string[]]$RemainingArgs
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

foreach ($arg in $RemainingArgs) {
    switch ($arg.ToLowerInvariant()) {
        '/force' { $Force = $true }
        '/noopen' { $NoOpen = $true }
    }
}

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$resultsDir = Join-Path $scriptDir 'TestResults'
$stampsDir = Join-Path $resultsDir '_stamps'

if (-not (Test-Path $stampsDir)) {
    New-Item -ItemType Directory -Path $stampsDir | Out-Null
}

function Test-ProjectNeedsRun {
    param(
        [Parameter(Mandatory = $true)]
        [string]$StampPath,
        [Parameter(Mandatory = $true)]
        [string[]]$Directories
    )

    if (-not (Test-Path $StampPath)) {
        return $true
    }

    $stampTime = (Get-Item $StampPath).LastWriteTime

    foreach ($dir in $Directories) {
        if (-not (Test-Path $dir)) {
            continue
        }

        $changedFile = Get-ChildItem -Path $dir -Recurse -File -Include '*.cs', '*.razor', '*.csproj' -ErrorAction SilentlyContinue |
            Where-Object { $_.LastWriteTime -gt $stampTime } |
            Select-Object -First 1

        if ($null -ne $changedFile) {
            return $true
        }
    }

    return $false
}

Clear-Host
Write-Host '=== Determining which projects need to run ==='

$coreStamp = Join-Path $stampsDir 'core.stamp'
$chartsStamp = Join-Path $stampsDir 'charts.stamp'

$coreDirs = @(
    (Join-Path $scriptDir '..\src\Core'),
    (Join-Path $scriptDir 'Core'),
    (Join-Path $scriptDir 'Shared')
)

$chartsDirs = @(
    (Join-Path $scriptDir '..\src\Charts'),
    (Join-Path $scriptDir 'Charts'),
    (Join-Path $scriptDir 'Shared')
)

$coreRun = $true
$chartsRun = $true

if (-not $Force) {
    $coreRun = Test-ProjectNeedsRun -StampPath $coreStamp -Directories $coreDirs
    $chartsRun = Test-ProjectNeedsRun -StampPath $chartsStamp -Directories $chartsDirs
}

Write-Host
if (-not $coreRun) {
    Write-Host ' Core   - SKIPPED (no changes detected)'
}
if (-not $chartsRun) {
    Write-Host ' Charts - SKIPPED (no changes detected)'
}

if (-not $coreRun -and -not $chartsRun) {
    Write-Host
    Write-Host 'Nothing to run. Use /force to override.'
}
else {
    if ($coreRun) {
        $coreResults = Join-Path $resultsDir 'Core'
        if (Test-Path $coreResults) {
            Remove-Item -Path $coreResults -Recurse -Force
        }

        Write-Host
        Write-Host '=== Running Core component tests with coverage ==='

        & dotnet test (Join-Path $scriptDir 'Core\Components.Tests.csproj') `
            --results-directory $coreResults `
            --configuration Release `
            --coverage `
            --coverage-output-format cobertura `
            --coverage-output Components.Tests.cobertura.xml `
            --coverage-settings (Join-Path $scriptDir 'Core\coverage.runsettings')

        if ($LASTEXITCODE -eq 0) {
            New-Item -ItemType File -Path $coreStamp -Force | Out-Null
        }
    }

    if ($chartsRun) {
        $chartsResults = Join-Path $resultsDir 'Charts'
        if (Test-Path $chartsResults) {
            Remove-Item -Path $chartsResults -Recurse -Force
        }

        Write-Host 
        Write-Host '=== Running Charts component tests with coverage ==='

        & dotnet test (Join-Path $scriptDir 'Charts\Components.Charts.Tests.csproj') `
            --results-directory $chartsResults `
            --configuration Release `
            --coverage `
            --coverage-output-format cobertura `
            --coverage-output Components.Charts.Tests.cobertura.xml `
            --coverage-settings (Join-Path $scriptDir 'Charts\coverage.runsettings')

        if ($LASTEXITCODE -eq 0) {
            New-Item -ItemType File -Path $chartsStamp -Force | Out-Null
        }
    }
}

Write-Host
Write-Host '=== Merging coverage reports ==='

& reportgenerator `
    "-reports:$resultsDir\**\*.cobertura.xml" `
    "-targetdir:$resultsDir\Report" `
    '-reporttypes:HtmlInline_AzurePipelines' `
    '-assemblyfilters:-Microsoft.FluentUI.AspNetCore.Components.Tests.Tools' `
    '-classfilters:-Microsoft.FluentUI.AspNetCore.Components.DesignTokens.*' `
    '-filefilters:-*RegexGenerator.g.cs' `
    'riskHotspotsAnalysisThresholds:metricThresholdForCrapScore=30' `
    'riskHotspotsAnalysisThresholds:metricThresholdForCyclomaticComplexity=30' `
    'minimumCoverageThresholds:lineCoverage=98'

Write-Host
$reportPath = Join-Path $resultsDir 'Report\index.htm'
if (-not $NoOpen -and (Test-Path $reportPath)) {
    Start-Process $reportPath
}
