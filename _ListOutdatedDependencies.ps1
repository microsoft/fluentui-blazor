#Requires -Version 7.0
<#
.SYNOPSIS
    Lists all outdated NuGet and npm packages for the solution, across every supported .NET target version.

.DESCRIPTION
    Temporarily switches the <NetVersion> property in Directory.Build.props to each of
    net8.0, net9.0 and net10.0, runs `dotnet outdated` for the solution, and
    aggregates the results into a single de-duplicated summary list.
    Also lists outdated npm packages (via npm-check-updates) for the Core.Scripts and
    Charts.Scripts projects, without changing any package.json or lock file.

    Directory.Build.props is restored to its original content when the script finishes
    (even if an error occurs).

.NOTES
    Requires the dotnet-outdated-tool and npm-check-updates:
        dotnet tool install --global dotnet-outdated-tool --add-source https://api.nuget.org/v3/index.json
        npm install --global npm-check-updates --registry https://registry.npmjs.org/
#>

$ErrorActionPreference = 'Stop'

$repoRoot = $PSScriptRoot
$propsPath = Join-Path $repoRoot 'Directory.Build.props'
$slnPath = Join-Path $repoRoot 'Microsoft.FluentUI-v5.slnx'
$netVersions = @('net8.0', 'net9.0', 'net10.0')
$netVersionPattern = '<NetVersion>net\d+\.\d+</NetVersion>'
$exampleVersionPattern = '<ExampleNetVersion>net\d+\.\d+</ExampleNetVersion>'
# Demo/Samples/Tests projects require net9.0+ (see ExampleNetVersion comment in Directory.Build.props).
$minExampleVersion = 9
$npmProjects = @('src/Core.Scripts', 'src/Charts.Scripts')

$originalContent = Get-Content -Path $propsPath -Raw
if ($originalContent -notmatch $netVersionPattern) {
    throw "Could not find a <NetVersion> element in $propsPath"
}
if ($originalContent -notmatch $exampleVersionPattern) {
    throw "Could not find an <ExampleNetVersion> element in $propsPath"
}

# Key = "NetVersion|Name|OldVersion|NewVersion", used to avoid duplicate rows across projects.
$updates = [ordered]@{}

try {
    foreach ($netVersion in $netVersions) {
        Write-Host "`n=== Checking outdated NuGet packages for $netVersion ===" -ForegroundColor Cyan

        # Example projects can't target lower than net9.0, and can't reference a library built for a higher TFM,
        # so ExampleNetVersion must track NetVersion once NetVersion reaches net9.0+.
        $netMajor = [int]($netVersion -replace '^net(\d+)\.\d+$', '$1')
        $exampleVersion = if ($netMajor -ge $minExampleVersion) { $netVersion } else { "net$minExampleVersion.0" }

        $updatedContent = $originalContent -replace $netVersionPattern, "<NetVersion>$netVersion</NetVersion>"
        $updatedContent = $updatedContent -replace $exampleVersionPattern, "<ExampleNetVersion>$exampleVersion</ExampleNetVersion>"
        Set-Content -Path $propsPath -Value $updatedContent -NoNewline

        $reportPath = Join-Path $repoRoot "outdated-$netVersion.json"
        if (Test-Path $reportPath) {
            Remove-Item $reportPath -Force
        }

        try {
            dotnet outdated $slnPath --version-lock Major --pre-release Never -o $reportPath -of json
        }
        catch {
            Write-Warning "dotnet outdated failed for ${netVersion}: $_"
            continue
        }

        if (-not (Test-Path $reportPath)) {
            Write-Warning "No report generated for $netVersion (SDK might not be installed). Skipping."
            continue
        }

        $report = Get-Content -Path $reportPath -Raw | ConvertFrom-Json
        foreach ($project in $report.Projects) {
            foreach ($targetFramework in $project.TargetFrameworks) {
                foreach ($dependency in $targetFramework.Dependencies) {
                    $key = "$netVersion|$($dependency.Name)|$($dependency.ResolvedVersion)|$($dependency.LatestVersion)"
                    if (-not $updates.Contains($key)) {
                        $updates[$key] = [pscustomobject]@{
                            NetVersion = $netVersion
                            Package    = $dependency.Name
                            OldVersion = $dependency.ResolvedVersion
                            NewVersion = $dependency.LatestVersion
                        }
                    }
                }
            }
        }

        Remove-Item $reportPath -Force
    }
}
finally {
    Set-Content -Path $propsPath -Value $originalContent -NoNewline
}

$summary = $updates.Values | Sort-Object @{Expression = { $netVersions.IndexOf($_.NetVersion) } }, Package, OldVersion

Write-Host "`n=== Summary of outdated NuGet packages (grouped by .NET version) ===" -ForegroundColor Green
foreach ($netVersion in $netVersions) {
    $group = $summary | Where-Object { $_.NetVersion -eq $netVersion }
    if ($group) {
        Write-Host "`n-- $netVersion --" -ForegroundColor Yellow
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
       dotnet restore ./Microsoft.FluentUI-v5.slnx
       dotnet build ./Microsoft.FluentUI-v5.slnx --configuration Release

npm packages:
  1. Update the versions listed above in src/Core.Scripts/package.json and src/Charts.Scripts/package.json.
  2. Reinstall to regenerate the lock files:
       npm install --prefix ./src/Core.Scripts
       npm install --prefix ./src/Charts.Scripts
'@

