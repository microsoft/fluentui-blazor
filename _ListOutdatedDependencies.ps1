#Requires -Version 7.0
<#
.SYNOPSIS
    Lists all outdated NuGet and npm packages for the solution, across every supported target framework.

.DESCRIPTION
    Runs `dotnet outdated` for the solution (projects already multi-target
    net8.0/net9.0/net10.0 via <TargetFrameworks>) and groups the results by
    $(TargetFramework).
    Also lists outdated npm packages (via npm-check-updates) for the Core.Assets project,
    without changing any package.json or lock file.

.NOTES
    Requires the dotnet-outdated-tool and npm-check-updates:
        dotnet tool install --global dotnet-outdated-tool --add-source https://api.nuget.org/v3/index.json
        npm install --global npm-check-updates --registry https://registry.npmjs.org/
#>

$ErrorActionPreference = 'Stop'

$repoRoot = $PSScriptRoot
$slnPath = Join-Path $repoRoot 'Microsoft.FluentUI.sln'
$npmProjects = @('src/Core.Assets')

# Key = "TargetFramework|Name|OldVersion|NewVersion", used to avoid duplicate rows across projects.
$updates = [ordered]@{}

Write-Host "`n=== Checking outdated NuGet packages ===" -ForegroundColor Cyan

$reportPath = Join-Path $repoRoot 'outdated.json'
if (Test-Path $reportPath) {
    Remove-Item $reportPath -Force
}

try {
    dotnet outdated $slnPath --version-lock Major --pre-release Never -o $reportPath -of json

    if (-not (Test-Path $reportPath)) {
        throw "No report generated (dotnet-outdated-tool might not be installed)."
    }

    $report = Get-Content -Path $reportPath -Raw | ConvertFrom-Json
    foreach ($project in $report.Projects) {
        foreach ($targetFramework in $project.TargetFrameworks) {
            foreach ($dependency in $targetFramework.Dependencies) {
                $key = "$($targetFramework.Name)|$($dependency.Name)|$($dependency.ResolvedVersion)|$($dependency.LatestVersion)"
                if (-not $updates.Contains($key)) {
                    $updates[$key] = [pscustomobject]@{
                        TargetFramework = $targetFramework.Name
                        Package         = $dependency.Name
                        OldVersion      = $dependency.ResolvedVersion
                        NewVersion      = $dependency.LatestVersion
                    }
                }
            }
        }
    }
}
finally {
    if (Test-Path $reportPath) {
        Remove-Item $reportPath -Force
    }
}

$targetFrameworks = $updates.Values | Select-Object -ExpandProperty TargetFramework -Unique | Sort-Object
$summary = $updates.Values | Sort-Object TargetFramework, Package, OldVersion

Write-Host "`n=== Summary of outdated NuGet packages (grouped by target framework) ===" -ForegroundColor Green
foreach ($targetFramework in $targetFrameworks) {
    $group = $summary | Where-Object { $_.TargetFramework -eq $targetFramework }
    if ($group) {
        Write-Host "`n-- $targetFramework --" -ForegroundColor Yellow
        $group | Select-Object Package, OldVersion, NewVersion | Format-Table -AutoSize
    }
}

# Key = "Project|Name|OldVersion|NewVersion", used to avoid duplicate rows.
$npmUpdates = [ordered]@{}

foreach ($npmProject in $npmProjects) {
    Write-Host "`n=== Checking outdated npm packages for $npmProject ===" -ForegroundColor Cyan

    $packageFilePath = Join-Path $repoRoot "$npmProject/package.json"
    try {
        $jsonOutput = ncu --packageFile $packageFilePath --jsonUpgraded 2>&1 | Out-String
        $upgraded = $jsonOutput | ConvertFrom-Json
    }
    catch {
        Write-Warning "ncu failed for ${npmProject}: $_"
        continue
    }

    $currentPackageJson = Get-Content -Path $packageFilePath -Raw | ConvertFrom-Json
    foreach ($property in $upgraded.PSObject.Properties) {
        $packageName = $property.Name
        $newVersion = $property.Value -replace '^[\^~]', ''
        $oldVersion = $null
        foreach ($section in @('dependencies', 'devDependencies')) {
            if ($currentPackageJson.$section -and $currentPackageJson.$section.PSObject.Properties[$packageName]) {
                $oldVersion = $currentPackageJson.$section.$packageName -replace '^[\^~]', ''
                break
            }
        }

        $key = "$npmProject|$packageName|$oldVersion|$newVersion"
        if (-not $npmUpdates.Contains($key)) {
            $npmUpdates[$key] = [pscustomobject]@{
                Project    = $npmProject
                Package    = $packageName
                OldVersion = $oldVersion
                NewVersion = $newVersion
            }
        }
    }
}

if ($npmUpdates.Count -gt 0) {
    Write-Host "`n=== Summary of outdated npm packages (grouped by project) ===" -ForegroundColor Green
    foreach ($npmProject in $npmProjects) {
        $group = $npmUpdates.Values | Where-Object { $_.Project -eq $npmProject } | Sort-Object Package
        if ($group) {
            Write-Host "`n-- $npmProject --" -ForegroundColor Yellow
            $group | Select-Object Package, OldVersion, NewVersion | Format-Table -AutoSize
        }
    }
}

Write-Host "`n=== Next steps: apply the updates manually ===" -ForegroundColor Green
Write-Host @'

NuGet packages:
  1. Update the versions listed above in Directory.Packages.props.
  2. Review shared version properties (RuntimeVersion*, AspNetCoreVersion*, EfCoreVersion*) so packages
     that reference them stay aligned for each target framework.
  3. Restore and build the solution:
       dotnet restore ./Microsoft.FluentUI.sln
       dotnet build ./Microsoft.FluentUI.sln --configuration Release

npm packages:
  1. Update the versions listed above in src/Core.Assets/package.json.
  2. Reinstall to regenerate the lock files:
       npm install --prefix ./src/Core.Assets
'@