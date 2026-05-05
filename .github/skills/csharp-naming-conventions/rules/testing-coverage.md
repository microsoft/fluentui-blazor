---
title: Code Coverage
impact: MEDIUM
impactDescription: Low code coverage allows regressions to slip through
tags: testing, coverage, coverlet, reportgenerator
---

## Code Coverage

Code coverage measures the amount of code run by unit tests — lines, branches, or methods. Use **Coverlet** and **ReportGenerator**.

### 1. Requirements

Include NuGet packages in your unit tests project:

```xml
<PackageReference Include="coverlet.msbuild" Version="8.0.1" />
<PackageReference Include="coverlet.collector" Version="8.0.1" />
```

### 2. Install Tools

```bash
dotnet tool install --global coverlet.console --version 8.0.1
dotnet tool install --global dotnet-reportgenerator-globaltool --version 5.5.5
```

Verify with:
```bash
dotnet tool list --global
```

### 3. Run Code Coverage

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
```

### 4. Generate HTML Report

```bash
reportgenerator "-reports:**/*.cobertura.xml" "-targetdir:C:\Temp\Coverage" -reporttypes:HtmlInline_AzurePipelines
```

Reference: [Unit Testing Code Coverage](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-code-coverage)
