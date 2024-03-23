# **/*.csproj
# !**/src/Templates/content/**/*.csproj

# Define the patterns
$includePattern = "**/*.csproj"
$excludePatterns = @(
  "**/src/Templates/content/**/*.csproj",
  "**/tests/TemplateValidation/**/*.csproj")
$assemblyVersion = "1.2.3"
$packageVersion = "7.8.9"

Write-Host "Add or update assemblies versions '$assemblyVersion' and package versions '$packageVersion'."

# ------------------------------------------
# Get all files matching the include pattern
# And matching the exclude patterns
# ------------------------------------------
function Get-FilesFromDirectory {
  param (
    [string]$includePattern,
    [string[]]$excludePatterns
  )

  # Get all files matching the include pattern
  $allFiles = Get-ChildItem -Path $includePattern -Recurse


  # Get all files matching the exclude pattern => $filteredFiles
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

  return $filteredFiles;
}

# -------------------------------------------------------------
# Add or update the PropertyName tag, to set the PropertyValue.
# in the XML file name
# -------------------------------------------------------------
function Update-PropertyGroup {
  param (
    [string]$fileName,
    [string]$propertyName,
    [string]$propertyValue
  )

  # Load the XML file and find the first "default" PropertyGroup
  $xmlDoc = New-Object System.Xml.XmlDocument
  $xmlDoc.Load($fileName)

  $propertyGroup = $xmlDoc.SelectSingleNode("//Project//PropertyGroup")

  # Find the VersionNumber element
  $propertyTag = $xmlDoc.SelectSingleNode("//Project//PropertyGroup//$propertyName")

  if ($null -eq $propertyTag) {
    $propertyTag = $xmlDoc.CreateElement($propertyName)
    $propertyGroup.AppendChild($propertyTag) > $null  # Suppress the output
  }

  # Update the text value of the VersionNumber element
  $propertyTag.InnerText = $propertyValue

  # Save the updated XML back to the file
  # $xmlDoc.Save($fileName)
}

# ------------------------------------------
# Main calls
# ------------------------------------------
$filteredFiles = Get-FilesFromDirectory -includePattern $includePattern -excludePatterns $excludePatterns

# Update the PropertyGroup items...
#  - VersionNumber: '$(assemblyVersion)'
#  - FileVersionNumber: '$(assemblyVersion)'
#  - InformationalVersion: '$(assemblyVersion)'
#  - PackageVersion: '$(packageVersion)'
foreach ($file in $filteredFiles) {
  $name = $file.Name
  $fileName = $file.FullName
  Write-Host " .. Updating $name" -ForegroundColor Yellow
  Update-PropertyGroup -fileName $fileName -propertyName "VersionNumber" -propertyValue $assemblyVersion
  Update-PropertyGroup -fileName $fileName -propertyName "FileVersionNumber" -propertyValue $assemblyVersion
  Update-PropertyGroup -fileName $fileName -propertyName "InformationalVersion" -propertyValue $assemblyVersion
  Update-PropertyGroup -fileName $fileName -propertyName "PackageVersion" -propertyValue $packageVersion
}
