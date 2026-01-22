#!/usr/bin/env pwsh

# Script to generate documentation and publish the FluentUI Demo locally

Write-Host "👉 Starting local demo publish process..." -ForegroundColor Green

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

# Build the core project
Write-Host "👉 Building Core project..." -ForegroundColor Yellow
dotnet build "./src/Core/Microsoft.FluentUI.AspNetCore.Components.csproj" -c Release -o "./src/Core/bin/Publish"  -f net9.0

# Generate API documentation file
Write-Host "👉 Generating API documentation..." -ForegroundColor Yellow
dotnet run --project ".\examples\Tools\FluentUI.Demo.DocApiGen\FluentUI.Demo.DocApiGen.csproj" --xml "./src/Core/bin/Publish/Microsoft.FluentUI.AspNetCore.Components.xml" --dll "./src/Core/bin/Publish/Microsoft.FluentUI.AspNetCore.Components.dll" --output "./examples/Demo/FluentUI.Demo.Client/wwwroot/api-comments.json" --format json

# Build the MCP Server project
Write-Host "👉 Building MCP Server project..." -ForegroundColor Yellow
dotnet build "./src/Tools/McpServer/Microsoft.FluentUI.AspNetCore.McpServer.csproj" -c Release -o "./src/Tools/McpServer/bin/Publish" -f net9.0

# Generate MCP documentation file
Write-Host "👉 Generating MCP documentation..." -ForegroundColor Yellow
dotnet run --project ".\examples\Tools\FluentUI.Demo.DocApiGen\FluentUI.Demo.DocApiGen.csproj" --xml "./src/Tools/McpServer/bin/Publish/Microsoft.FluentUI.AspNetCore.McpServer.xml" --dll "./src/Tools/McpServer/bin/Publish/Microsoft.FluentUI.AspNetCore.McpServer.dll" --output "./examples/Demo/FluentUI.Demo.Client/wwwroot/mcp-documentation.json" --format json --mode mcp

# Publish the demo
Write-Host "👉 Publishing demo..." -ForegroundColor Yellow
dotnet publish "./examples/Demo/FluentUI.Demo/FluentUI.Demo.csproj" -c Release -o "./examples/Demo/FluentUI.Demo/bin/Publish" -f net9.0

# Verify that the bundle CSS file has the expected size
Write-Host "👉 Verifying bundle CSS file size..." -ForegroundColor Yellow
$bundleFilePath = "./examples/Demo/FluentUI.Demo/bin/Publish/wwwroot/_content/Microsoft.FluentUI.AspNetCore.Components/Microsoft.FluentUI.AspNetCore.Components.bundle.scp.css.br"

if (Test-Path $bundleFilePath) {
    $fileSize = (Get-Item $bundleFilePath).Length
    $fileSizeKB = [math]::Round($fileSize / 1024, 2)

    if ($fileSize -gt 1024) {
        Write-Host "☑️ Bundle CSS file verified: $fileSizeKB KB" -ForegroundColor Green
    } else {
        Write-Host "⛔ Bundle CSS file is too small: $fileSizeKB KB (expected > 1KB)" -ForegroundColor Red
        Write-Host "⛔ This may indicate a build issue with the CSS bundle generation." -ForegroundColor Red
        Write-Host "⛔ Install .NET 9.0.205 SDK and a `global.json` file with `{ ""sdk"": { ""version"": ""9.0.205"" } }`." -ForegroundColor Red
        exit 1
    }
} else {
    Write-Host "⛔ Bundle CSS file not found: $bundleFilePath" -ForegroundColor Red
    Write-Host "⛔ This may indicate a build issue with the CSS bundle generation." -ForegroundColor Red
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

Write-Host "✅ Demo publish process completed successfully!" -ForegroundColor Green

# Ask user if they want to run the website
Write-Host ""
$runWebsite = Read-Host "Do you want to run the website now? (Y/n)"
if ($runWebsite -eq "" -or $runWebsite -eq "Y" -or $runWebsite -eq "y") {
    Write-Host "👉 Starting the website..." -ForegroundColor Green
    Start-Process -FilePath "./examples/Demo/FluentUI.Demo/bin/Publish/FluentUI.Demo.exe" -WorkingDirectory "./examples/Demo/FluentUI.Demo/bin/Publish"
}

Write-Host "👉 You can deploy to Azure using a command like:" -ForegroundColor Green
Write-Host "▶️ az webapp deploy --resource-group FluentUI --name fluentui-blazor-v5 --src-path ./examples/Demo/FluentUI.Demo/bin/FluentUI-Blazor.zip --type zip" -ForegroundColor Green
