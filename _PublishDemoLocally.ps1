#!/usr/bin/env pwsh

# Ask user for configuration
Write-Host "👉 Configuration setup..." -ForegroundColor Cyan
Write-Host ""

# Ask for .NET version
$dotnetVersionChoice = Read-Host "❓ Which .NET version do you want to use? (9 for net9.0, 10 for net10.0) [default: 9]"
if ($dotnetVersionChoice -eq "" -or $dotnetVersionChoice -eq "9") {
    $dotnetVersion = "net9.0"
} elseif ($dotnetVersionChoice -eq "10") {
    $dotnetVersion = "net10.0"
} else {
    Write-Host "⛔ Invalid choice." -ForegroundColor Red
    exit 1
}

# Ask for build number
# Get the version number from the eng/pipelines/version.yml file if it exists
$versionFilePath = "./eng/pipelines/version.yml"
if (Test-Path $versionFilePath) {
    $versionFileContent = Get-Content $versionFilePath -Raw
    $versionMatch = $versionFileContent -match "FileVersion:\s*'([0-9]+\.[0-9]+\.[0-9]+)'"
    if ($versionMatch) {
        $pipelineVersion = $Matches[1]
        Write-Host "ℹ️ Found version in version.yml: $pipelineVersion" -ForegroundColor Cyan
        $buildNumber = $pipelineVersion
    } else {
        Write-Host "⚠️ Could not find a version in version.yml." -ForegroundColor Yellow
    }
} else {
    Write-Host "⚠️ version.yml file not found at $versionFilePath." -ForegroundColor Yellow
    $buildNumber = Read-Host "❓ What is the BuildNumber version to use? (e.g., 4.13.2)"
    if ([string]::IsNullOrWhiteSpace($buildNumber)) {
        Write-Host "⛔ Build number cannot be empty." -ForegroundColor Red
        exit 1
    }
}

Write-Host ""
Write-Host "Configuration:" -ForegroundColor Green
Write-Host "  .NET Version: $dotnetVersion" -ForegroundColor White
Write-Host "  Build Number: $buildNumber" -ForegroundColor White
Write-Host ""

# Clean previous build artifacts
Write-Host "👉 Cleaning previous build artifacts (bin and obj)..." -ForegroundColor Yellow

if (Test-Path "./examples/Demo/Client/bin") {
    Remove-Item -Path "./examples/Demo/Client/bin" -Recurse -Force
}

if (Test-Path "./examples/Demo/Client/obj") {
    Remove-Item -Path "./examples/Demo/Client/obj" -Recurse -Force
}

if (Test-Path "./src/Core/bin/") {
    Remove-Item -Path "./src/Core/bin" -Recurse -Force
}

if (Test-Path "./src/Core/obj/") {
    Remove-Item -Path "./src/Core/obj" -Recurse -Force
}

if (Test-Path "./src/Extensions/DesignToken.Generator/bin/") {
    Remove-Item -Path "./src/Extensions/DesignToken.Generator/bin" -Recurse -Force
}

if (Test-Path "./src/Extensions/DesignToken.Generator/obj/") {
    Remove-Item -Path "./src/Extensions/DesignToken.Generator/obj" -Recurse -Force
}

# If a 'global.json' file exists, back it up
$globalJsonPath = "./global.json"
$globalJsonBackupPath = "./global.json.localpublishbackup"
if (Test-Path $globalJsonPath) {
    Write-Host "👉 Backing up existing global.json file..." -ForegroundColor Yellow
    Copy-Item -Path $globalJsonPath -Destination $globalJsonBackupPath -Force
    Remove-Item -Path $globalJsonPath -Force
    # store that we need to restore it later
    $restoreGlobalJson = $true
}

# If a 'global.json.local' file exists, rename it to 'global.json'
$globalJsonLocalPath = "./global.json.local"
if (Test-Path $globalJsonLocalPath) {
    Write-Host "👉 Using specific global.json for this publish..." -ForegroundColor Yellow
    Rename-Item -Path $globalJsonLocalPath -NewName "global.json"
    # store that we need to restore it later
    $restoreGlobalJsonLocal = $true
}

#search through all .csproj files and replace <TargetFrameworks>net8.0;net9.0;net10.0</TargetFrameworks> with <TargetFrameworks>net8.0;net9.0</TargetFrameworks>
Write-Host "👉 Adjusting TargetFrameworks in project files..." -ForegroundColor Yellow
$csprojFiles = Get-ChildItem -Path "." -Recurse -Filter "*.csproj"
foreach ($file in $csprojFiles) {
    #if the project file is in the Templates folder, skip it
    if ($file.PSPath -like "*Templates*") {
        continue
    }
    $originalContent = Get-Content $file.PSPath -Raw
    $newContent = $originalContent -replace '<TargetFrameworks>(.*?);net10.0</TargetFrameworks>', '<TargetFrameworks>$1</TargetFrameworks>'
    if ($originalContent -ne $newContent) {
        Set-Content $file.PSPath ($newContent.TrimEnd("`r", "`n"))
    }
    $newContent = $originalContent -replace '<TargetFramework>net10.0</TargetFramework>', '<TargetFramework>net9.0</TargetFramework>'
    if ($originalContent -ne $newContent) {
        Set-Content $file.PSPath ($newContent.TrimEnd("`r", "`n"))
    }
}

# Search through Directory.Packages.props and replace the following package version from 4.14.0 to 4.13.0 for the following packages:
# - Microsoft.CodeAnalysis.Analyzers
# - Microsoft.CodeAnalysis.CSharp
Write-Host "👉 Setting CodeAnalasys packages versions to 4.13.0..." -ForegroundColor Yellow
$directoryPackagesFile = "./Directory.Packages.props"
if (Test-Path $directoryPackagesFile) {
    $originalContent = Get-Content $directoryPackagesFile -Raw
    $newContent = $originalContent -replace '<PackageVersion Include="Microsoft.CodeAnalysis.Analyzers" Version="4.14.0" />', '<PackageVersion Include="Microsoft.CodeAnalysis.Analyzers" Version="4.13.0" />'
    $newContent = $newContent -replace '<PackageVersion Include="Microsoft.CodeAnalysis.CSharp" Version="4.14.0" />', '<PackageVersion Include="Microsoft.CodeAnalysis.CSharp" Version="4.13.0" />'
    if ($originalContent -ne $newContent) {
        Set-Content $directoryPackagesFile ($newContent.TrimEnd("`r", "`n"))
    }
} else {
    Write-Host "⚠️ Directory.Packages.props file not found at $directoryPackagesFile." -ForegroundColor Red
}


# Publish the demo
Write-Host "👉 Publishing demo..." -ForegroundColor Yellow
dotnet publish "./examples/Demo/Client/FluentUI.Demo.Client.csproj" -c Release -o "./examples/Demo/Client/bin/Publish" -f $dotnetVersion -r linux-x64 --self-contained=true -p:BuildNumber=$buildNumber

# Verify that the bundle JS file has the expected size
Write-Host "👉 Verifying bundle JS file size..." -ForegroundColor Yellow
$bundleFilePath = "./examples/Demo/Client/bin/Publish/wwwroot/_content/Microsoft.FluentUI.AspNetCore.Components/Microsoft.FluentUI.AspNetCore.Components.lib.module.js.br"

if (Test-Path $bundleFilePath) {
    $fileSize = (Get-Item $bundleFilePath).Length
    $fileSizeKB = [math]::Round($fileSize / 1024, 2)

    if ($fileSize -gt 1024) {
        Write-Host "☑️ Bundle JS file verified: $fileSizeKB KB" -ForegroundColor Green
    } else {
        Write-Host "⛔ Bundle JS file is too small: $fileSizeKB KB (expected > 1KB)" -ForegroundColor Red
        Write-Host "⛔ This may indicate a build issue with the JS bundle generation." -ForegroundColor Red
        Write-Host "⛔ Install .NET 9.0.205 SDK, remove the references to 'net10' and add a `global.json` file with `{ ""sdk"": { ""version"": ""9.0.205"" } }`." -ForegroundColor Red
        exit 1
    }
} else {
    Write-Host "⛔ Bundle JS file not found: $bundleFilePath" -ForegroundColor Red
    Write-Host "⛔ This may indicate a build issue with the JS bundle generation." -ForegroundColor Red
    exit 1
}

Write-Host "✅ Demo publish process completed successfully!" -ForegroundColor Green

Write-Host "👉 You can deploy to Azure using a command like:" -ForegroundColor Green
Write-Host "▶️ swa deploy --output-location ./examples/Demo/Client/bin/Publish/wwwroot --env production --deployment-token <TOKEN>" -ForegroundColor Green

# Undo the TargetFrameworks changes

Write-Host "👉 Restoring TargetFrameworks in project files..." -ForegroundColor Yellow
foreach ($file in $csprojFiles) {
    #if the project file is in the Templates folder, skip it
    if ($file.PSPath -like "*Templates*") {
        continue
    }
    $originalContent = Get-Content $file.PSPath -Raw
    $newContent = $originalContent -replace '<TargetFrameworks>net8.0;net9.0</TargetFrameworks>', '<TargetFrameworks>net8.0;net9.0;net10.0</TargetFrameworks>'
    if ($originalContent -ne $newContent) {
        Set-Content $file.PSPath ($newContent.TrimEnd("`r", "`n"))
    }
    $newContent = $originalContent -replace '<TargetFramework>net9.0</TargetFramework>', '<TargetFramework>net10.0</TargetFramework>'
    if ($originalContent -ne $newContent) {
        Set-Content $file.PSPath ($newContent.TrimEnd("`r", "`n"))
    }
}

# Undo the CodeAnalysis package version changes
Write-Host "👉 Restoring CodeAnalysis packages versions to 4.14.0..." -ForegroundColor Yellow
if (Test-Path $directoryPackagesFile) {
    $originalContent = Get-Content $directoryPackagesFile -Raw
    $newContent = $originalContent -replace '<PackageVersion Include="Microsoft.CodeAnalysis.Analyzers" Version="4.13.0" />', '<PackageVersion Include="Microsoft.CodeAnalysis.Analyzers" Version="4.14.0" />'
    $newContent = $newContent -replace '<PackageVersion Include="Microsoft.CodeAnalysis.CSharp" Version="4.13.0" />', '<PackageVersion Include="Microsoft.CodeAnalysis.CSharp" Version="4.14.0" />'
    if ($originalContent -ne $newContent) {
        Set-Content $directoryPackagesFile ($newContent.TrimEnd("`r", "`n"))
    }
}

# Restore the global.json.local file if it was used
if ($restoreGlobalJsonLocal) {
    Write-Host "👉 Restoring global.json.local file..." -ForegroundColor Yellow
    Rename-Item -Path $globalJsonPath -NewName "global.json.local"
}

# Restore the original global.json file if it was backed up
if ($restoreGlobalJson) {
    Write-Host "👉 Restoring original global.json file..." -ForegroundColor Yellow
    Move-Item -Path $globalJsonBackupPath -Destination $globalJsonPath -Force
}


# Ask user if they want to run the website
# Require 'dotnet tool install --global dotnet-serve'
Write-Host ""
$runWebsite = Read-Host "Do you want to run the local website now? (Y/n) ... using `dotnet serve` "
if ($runWebsite -eq "" -or $runWebsite -eq "Y" -or $runWebsite -eq "y") {
    Write-Host "👉 Starting the website..." -ForegroundColor Green
    dotnet serve --directory "./examples/Demo/Client/bin/Publish/wwwroot" --brotli  --gzip --open-browser
}
