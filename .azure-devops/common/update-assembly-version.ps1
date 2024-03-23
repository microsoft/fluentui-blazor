# **/*.csproj
# !**/src/Templates/content/**/*.csproj

# Define the patterns
$includePattern = "**/*.csproj"
$excludePatterns = @(
  "**/src/Templates/content/**/*.csproj",
  "**/tests/TemplateValidation/**/*.csproj")

# Get all files matching the include pattern
$allFiles = Get-ChildItem -Path $includePattern -Recurse

# Get all files matching the exclude pattern
$excludedFiles = @() # Empty array
foreach ($pattern in $excludePatterns) {
  # Create a RegEx: replacing "**" by ".*", "*" by "[^/]*"
  $parts = $pattern -split '[\\/]'
  foreach ($i in 0..($parts.Length - 1)) {
    if ($parts[$i] -eq '**') {
      $parts[$i] = '.*'
    }
    if ($parts[$i] -eq '*') {
      $parts[$i] = '[^/]*'
    }
  }
  $excludeRegEx = $parts -join "\/"

  # Check all files to detect the matching one
  foreach ($file in $allFiles) {
    $match1 = ($file.FullName -replace "\\", "/") -match $excludeRegEx
    if ($match1) {
      $excludedFiles += $file.FullName
    }
  }

  # Exclude found files
  $filteredFiles = $allFiles | Where-Object { $_.FullName -notin $excludedFiles }
}

# Print the filtered files
$filteredFiles | ForEach-Object {
  Write-Host $_.FullName -ForegroundColor Yellow
}
