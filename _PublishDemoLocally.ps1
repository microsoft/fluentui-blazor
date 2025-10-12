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
Write-Host "👉 Building core project..." -ForegroundColor Yellow
dotnet build "./src/Core/Microsoft.FluentUI.AspNetCore.Components.csproj" -c Release -o "./src/Core/bin/Publish"  -f net9.0

# Generate documentation file
Write-Host "👉 Generating API documentation..." -ForegroundColor Yellow
dotnet run --project ".\examples\Tools\FluentUI.Demo.DocApiGen\FluentUI.Demo.DocApiGen.csproj" --xml "./src/Core/bin/Publish/Microsoft.FluentUI.AspNetCore.Components.xml" --dll "./src/Core/bin/Publish/Microsoft.FluentUI.AspNetCore.Components.dll" --output "./examples/Demo/FluentUI.Demo.Client/wwwroot/api-comments.json" --format json

# Publish the demo
Write-Host "👉 Publishing demo..." -ForegroundColor Yellow
dotnet publish "./examples/Demo/FluentUI.Demo/FluentUI.Demo.csproj" -c Release -o "./examples/Demo/FluentUI.Demo/bin/Publish" -f net9.0

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
