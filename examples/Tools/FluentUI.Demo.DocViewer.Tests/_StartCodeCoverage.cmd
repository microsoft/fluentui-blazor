echo off

REM 0. Include the NuGet Package "coverlet.msbuild" in the UnitTests project.
REM 1. Install tools:
REM     $:\> dotnet tool install --global coverlet.console --version 3.2.0
REM     $:\> dotnet tool install --global dotnet-reportgenerator-globaltool --version 5.3.7
REM    
REM     Use this command to list existing installed tools:
REM     $:\> dotnet tool list --global
REM
REM 2. Start a code coverage in the UnitTests project:
REM     $:\> dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
REM 
REM 3. Display the Coverage Report:
REM     $:\> reportgenerator "-reports:coverage.cobertura.xml" "-targetdir:C:\Temp\FluentUI\Coverage" -reporttypes:HtmlInline_AzurePipelines
REM     $:\> explorer C:\Temp\Coverage\index.html

echo on
cls

dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
reportgenerator "-reports:coverage.cobertura.xml" "-targetdir:C:\Temp\FluentUI\Coverage-DocViewer" -reporttypes:HtmlInline_AzurePipelines riskHotspotsAnalysisThresholds:metricThresholdForCrapScore=30 riskHotspotsAnalysisThresholds:metricThresholdForCyclomaticComplexity=30
start "" "C:\Temp\FluentUI\Coverage-DocViewer\index.htm"
