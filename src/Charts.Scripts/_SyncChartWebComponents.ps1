#Requires -Version 7.0

[CmdletBinding(SupportsShouldProcess)]
param(
    [string] $SourceRoot = 'D:\Source\FluentUI\fluentui\packages\charts\chart-web-components\src',
    [string] $DestinationRoot = (Join-Path $PSScriptRoot 'src')
)

$ErrorActionPreference = 'Stop'

if (-not (Test-Path -LiteralPath $SourceRoot -PathType Container)) {
    throw "Chart Web Components source folder does not exist: $SourceRoot"
}

if (-not (Test-Path -LiteralPath $DestinationRoot -PathType Container)) {
    throw "Charts.Scripts destination folder does not exist: $DestinationRoot"
}

$resolvedSourceRoot = (Resolve-Path -LiteralPath $SourceRoot).Path
$resolvedDestinationRoot = (Resolve-Path -LiteralPath $DestinationRoot).Path
$excludedTypeScriptPattern = '\.(bench|spec|stories)\.ts$'
$excludedChartDirectories = @(
    'stacked-bar-chart'
    'tree-chart'
)
$copiedFileCount = 0
$skippedFileCount = 0

foreach ($sourceFile in Get-ChildItem -LiteralPath $resolvedSourceRoot -File -Recurse) {
    $relativePath = [System.IO.Path]::GetRelativePath($resolvedSourceRoot, $sourceFile.FullName)
    $relativeDirectory = Split-Path -Parent $relativePath
    $chartDirectory = ($relativeDirectory -split '[\\/]')[0]

    if (-not $relativePath.Contains([System.IO.Path]::DirectorySeparatorChar) -or
        $sourceFile.Name -match $excludedTypeScriptPattern -or
        $excludedChartDirectories -contains $chartDirectory) {
        $skippedFileCount++
        continue
    }

    $destinationPath = Join-Path $resolvedDestinationRoot $relativePath
    $destinationDirectory = Split-Path -Parent $destinationPath

    if ($PSCmdlet.ShouldProcess($destinationPath, "Copy $($sourceFile.FullName)")) {
        if (-not (Test-Path -LiteralPath $destinationDirectory -PathType Container)) {
            New-Item -ItemType Directory -Path $destinationDirectory -Force | Out-Null
        }

        Copy-Item -LiteralPath $sourceFile.FullName -Destination $destinationPath -Force
        $copiedFileCount++
    }
}

Write-Host "Copied $copiedFileCount chart files to $resolvedDestinationRoot."
Write-Host "Skipped $skippedFileCount root, benchmark, story, test, stacked-bar-chart, and tree-chart files."
