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
            '--collect:XPlat Code Coverage' `
            '--results-directory' $coreResults `
            '--' 'DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Include=[Microsoft.FluentUI.AspNetCore.Components]*'

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
            '--collect:XPlat Code Coverage' `
            '--results-directory' $chartsResults `
            '--' 'DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Include=[Microsoft.FluentUI.AspNetCore.Components.Charts]*'

        if ($LASTEXITCODE -eq 0) {
            New-Item -ItemType File -Path $chartsStamp -Force | Out-Null
        }
    }
}

Write-Host
Write-Host '=== Merging coverage reports ==='

& reportgenerator `
    "-reports:$resultsDir\**\coverage.cobertura.xml" `
    "-targetdir:$resultsDir\Report" `
    '-reporttypes:HtmlInline_AzurePipelines' `
    '-assemblyfilters:+Microsoft.FluentUI.AspNetCore.Components;+Microsoft.FluentUI.AspNetCore.Components.Charts' `
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
