# PowerShell script to find and build the nearest .csproj file
param(
    [string]$CurrentFile = $env:VSCODE_ACTIVE_FILE,
    [string]$WorkspaceFolder = $env:WORKSPACE_FOLDER
)

# Global constants
$DEFAULT_SOLUTION_NAME = "Microsoft.FluentUI-v5.sln"

# Function to find the nearest .csproj file
function Find-NearestProject {
    param([string]$StartPath)
    
    $currentDir = Split-Path -Parent $StartPath
    
    while ($currentDir -and $currentDir -ne [System.IO.Path]::GetPathRoot($currentDir)) {
        # Look for .csproj files in current directory
        $csprojFiles = Get-ChildItem -Path $currentDir -Filter "*.csproj" -ErrorAction SilentlyContinue
        
        if ($csprojFiles) {
            # Return the first .csproj found
            return $csprojFiles[0].FullName
        }
        
        # Move up one directory
        $currentDir = Split-Path -Parent $currentDir
    }
    
    return $null
}

# Main execution
try {
    if (-not $CurrentFile) {
        Write-Host "No active file detected. Building default solution..." -ForegroundColor Yellow
        $projectPath = Join-Path $WorkspaceFolder $DEFAULT_SOLUTION_NAME
    } else {
        Write-Host "Active file: $CurrentFile" -ForegroundColor Green
        
        $nearestProject = Find-NearestProject -StartPath $CurrentFile
        
        if ($nearestProject) {
            $projectPath = $nearestProject
            Write-Host "Found nearest project: $projectPath" -ForegroundColor Green
        } else {
            Write-Host "No .csproj file found in parent directories. Building default solution..." -ForegroundColor Yellow
            $projectPath = Join-Path $WorkspaceFolder $DEFAULT_SOLUTION_NAME
        }
    }
    
    # Build the project
    Write-Host "Building: $projectPath" -ForegroundColor Cyan
    & dotnet build "$projectPath" --configuration Debug
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Build completed successfully!" -ForegroundColor Green
    } else {
        Write-Host "Build failed with exit code $LASTEXITCODE" -ForegroundColor Red
        exit $LASTEXITCODE
    }
}
catch {
    Write-Host "Error: $_" -ForegroundColor Red
    exit 1
}
