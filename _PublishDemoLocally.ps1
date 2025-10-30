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
$buildNumber = Read-Host "❓ What is the BuildNumber version to use? (e.g., 4.13.0)"
if ([string]::IsNullOrWhiteSpace($buildNumber)) {
    Write-Host "⛔ Build number cannot be empty." -ForegroundColor Red
    exit 1
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

# Ask user if they want to run the website
# Require 'dotnet tool install --global dotnet-serve'
Write-Host ""
$runWebsite = Read-Host "Do you want to run the local website now? (Y/n) ... using `dotnet serve` "
if ($runWebsite -eq "" -or $runWebsite -eq "Y" -or $runWebsite -eq "y") {
    Write-Host "👉 Starting the website..." -ForegroundColor Green
    dotnet serve --directory "./examples/Demo/Client/bin/Publish/wwwroot" --brotli  --gzip --open-browser
}
