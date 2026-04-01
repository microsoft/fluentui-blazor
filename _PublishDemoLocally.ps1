#!/usr/bin/env pwsh

# ###########################################
# Requires PowerShell Version 7.0+
# ###########################################

# Script to generate documentation and publish the FluentUI Demo locally

# Ask user for configuration
Write-Host "👉 Configuration setup..." -ForegroundColor Cyan
Write-Host ""

# Ask for .NET version
$dotnetVersionChoice = Read-Host "❓ Which .NET version do you want to use? (9 for net9.0, 10 for net10.0) [default: 10]"
if ($dotnetVersionChoice -eq "9") {
    $NetVersion = "net9.0"
} elseif ($dotnetVersionChoice -eq "" -or $dotnetVersionChoice -eq "10") {
    $NetVersion = "net10.0"
} else {
    Write-Host "⛔ Invalid choice." -ForegroundColor Red
    exit 1
}

# Show build number
$propsContent = Get-Content "./Directory.Build.props" -Raw

$versionPrefix = $propsContent -match "<VersionPrefix>([0-9]+\.[0-9]+\.[0-9]+)</VersionPrefix>"
$pipelineVersion = $Matches[1]

$versionSuffix = $propsContent -match "<VersionSuffix>([0-9A-Za-z\.-]+)</VersionSuffix>"
if ($versionSuffix) {
    $pipelineSuffix = $Matches[1]
}
if ($pipelineSuffix) {
    $version = "$pipelineVersion-$pipelineSuffix"
} else {
    $version = $pipelineVersion
}

Write-Host ""
Write-Host "Configuration:" -ForegroundColor Green
Write-Host "     .NET Version: $NetVersion" -ForegroundColor White
Write-Host "  Package version: $version" -ForegroundColor White
Write-Host ""

# Ask for doing quick of full publish
$publishChoice = Read-Host "❓ Do you want to do a full publish (skip API documentation generation, MCP Server build)? (y/n) [default: y]"
if ($publishChoice -eq "n") {
    $fullBuild = $false
} elseif ($publishChoice -eq "" -or $publishChoice -eq "y") {
    $fullBuild = $true
} else {
    Write-Host "⛔ Invalid choice." -ForegroundColor Red
    exit 1
}

# Clean previous build artifacts
Write-Host "👉 Cleaning previous build artifacts (bin and obj)..." -ForegroundColor Yellow

if (Test-Path "./examples/Demo/FluentUI.Demo/bin") {
    Remove-Item -Path "./examples/Demo/FluentUI.Demo/bin" -Recurse -Force
}

if (Test-Path "./examples/Demo/FluentUI.Demo/obj") {
    Remove-Item -Path "./examples/Demo/FluentUI.Demo/obj" -Recurse -Force
}

if (Test-Path "./src/Core/bin/") {
    Remove-Item -Path "./src/Core/bin" -Recurse -Force
}

if (Test-Path "./src/Core/obj/") {
    Remove-Item -Path "./src/Core/obj" -Recurse -Force
}

$RootDir = $PSScriptRoot

# Update the Directory.Build.props file with the correct .NET version
Write-Host "👉 Updating Directory.Build.props with .NET version: $NetVersion..." -ForegroundColor Yellow

$propsMatch = $propsContent -match "(<NetVersion>net[0-9]+\.[0-9]+</NetVersion>)"
if ($Matches[1] -ne "<NetVersion>$NetVersion</NetVersion>") {
    $propsVersion = $Matches[1]
    $propsContent -replace '<NetVersion>net[0-9]+\.[0-9]+</NetVersion>', "<NetVersion>$NetVersion</NetVersion>" | Set-Content "./Directory.Build.props"
    Write-Host "- Replaced NetVersion: $propsVersion -> $NetVersion..."
}


$propsMatch = $propsContent -match "(<TargetNetVersions Condition=`"'\$\(Configuration\)' == 'Release'`">.*</TargetNetVersions>)"
if ($Matches[1] -ne "<TargetNetVersions Condition=`"'`$(Configuration)' == 'Release'`">$NetVersion</TargetNetVersions>") {
    $propsVersion = $Matches[1]
    $propsContent -replace "<TargetNetVersions Condition=`"'\$\(Configuration\)' == 'Release'`">.*</TargetNetVersions>", "<TargetNetVersions Condition=`"'`$(Configuration)' == 'Release'`">$NetVersion</TargetNetVersions>" | Set-Content "./Directory.Build.props"
    Write-Host "- Replaced TargetNetVersions for Release: $propsVersion -> $NetVersion..."
}

if ($fullBuild) {
    # Build the core project
    Write-Host "👉 Building Core project..." -ForegroundColor Yellow
    dotnet build "./src/Core/Microsoft.FluentUI.AspNetCore.Components.csproj" -c Release -o "./src/Core/bin/Publish"  -f $NetVersion

    # Generate API documentation file
    Write-Host "👉 Generating API documentation..." -ForegroundColor Yellow
    dotnet run --project ".\examples\Tools\FluentUI.Demo.DocApiGen\FluentUI.Demo.DocApiGen.csproj" --xml "$RootDir/src/Core/bin/Publish/Microsoft.FluentUI.AspNetCore.Components.xml" --dll "$RootDir/src/Core/bin/Publish/Microsoft.FluentUI.AspNetCore.Components.dll" --output "$RootDir/examples/Demo/FluentUI.Demo.Client/wwwroot/api-comments.json" --format json

    # Build the MCP Server project
    Write-Host "👉 Building MCP Server project..." -ForegroundColor Yellow
    dotnet build "./src/Tools/McpServer/Microsoft.FluentUI.AspNetCore.McpServer.csproj" -c Release -o "./src/Tools/McpServer/bin/Publish" -f $NetVersion

    # Generate MCP documentation file
    Write-Host "👉 Generating MCP documentation..." -ForegroundColor Yellow
    # dotnet run --project ".\examples\Tools\FluentUI.Demo.DocApiGen\FluentUI.Demo.DocApiGen.csproj" --xml "$RootDir/src/Tools/McpServer/bin/Publish/Microsoft.FluentUI.AspNetCore.McpServer.xml" --dll "$RootDir/src/Tools/McpServer/bin/Publish/Microsoft.FluentUI.AspNetCore.McpServer.dll" --output "$RootDir/examples/Demo/FluentUI.Demo.Client/wwwroot/mcp-documentation.json" --format json --mode mcp
    Write-Host "   Skipped."
}

# Publish the demo
Write-Host "👉 Publishing demo..." -ForegroundColor Yellow
if ($fullBuild) {
    dotnet publish "./examples/Demo/FluentUI.Demo/FluentUI.Demo.csproj" -c Release -o "./examples/Demo/FluentUI.Demo/bin/Publish" -f $NetVersion --no-build
} else {
    dotnet publish "./examples/Demo/FluentUI.Demo/FluentUI.Demo.csproj" -c Release -o "./examples/Demo/FluentUI.Demo/bin/Publish" -f $NetVersion
}

# Verify that the bundle JS file has the expected size
Write-Host ""
Write-Host "👉 Verifying bundle JS file size..." -ForegroundColor Yellow
$bundleFilePath = "./examples/Demo/FluentUI.Demo/bin/Publish/wwwroot/_content/Microsoft.FluentUI.AspNetCore.Components/Microsoft.FluentUI.AspNetCore.Components.lib.module.js.br"

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
# Create deployment archive
Write-Host "👉 Creating deployment archive..." -ForegroundColor Yellow
if (Test-Path "./examples/Demo/FluentUI.Demo/bin/Publish") {
    Compress-Archive -Path ./examples/Demo/FluentUI.Demo/bin/Publish/* -DestinationPath ./examples/Demo/FluentUI.Demo/bin/FluentUI-Blazor.zip -Force
    Write-Host "☑️ Archive created: ./examples/Demo/FluentUI.Demo/bin/FluentUI-Blazor.zip" -ForegroundColor Green
} else {
    Write-Host "⛔Publish directory not found!" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "✅ Demo publish process completed successfully!" -ForegroundColor Green
Write-Host ""

Write-Host ""
Write-Host "----------------------------------------------------"
Write-Host "👉 You can deploy to Azure using a command like:" -ForegroundColor Green
Write-Host "▶️ az webapp deploy --resource-group FluentUI --name fluentui-blazor-v5 --src-path ./examples/Demo/FluentUI.Demo/bin/FluentUI-Blazor.zip --type zip" -ForegroundColor Green
Write-Host "----------------------------------------------------"

# Ask user if they want to run the website
Write-Host ""
$runWebsite = Read-Host "Do you want to run the local website now? (y/n) [default: y]"
if ($runWebsite -eq "" -or $runWebsite -eq "Y" -or $runWebsite -eq "y") {
    Write-Host "👉 Starting the website..." -ForegroundColor Green
    Start-Process -FilePath "./examples/Demo/FluentUI.Demo/bin/Publish/FluentUI.Demo.exe" -WorkingDirectory "./examples/Demo/FluentUI.Demo/bin/Publish"
}
