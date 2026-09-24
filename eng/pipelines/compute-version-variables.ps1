<#
.DESCRIPTION
    Compute AssemblyVersion and PackageVersion

.PARAMETER branchName
    The branch of the trigger repo for which the build has been queued.
    Please, use $(Build.SourceBranchName)

.PARAMETER buildNumber
    The name of the completed build, also known as the run number.
    This value is formatted like "[Major].[Minor].[Revision].[Year:00].[DayOfYear].[BuildRevision]" (e.g. "4.6.1.24123.3")
    Please, use $(Build.BuildNumber)

.PARAMETER packageSuffix
    Suffix to add to the computed version. Example "Preview", "RC.1", ...
    This suffix overrides the one calculated for dev or main branches.
    Default is "".

.PARAMETER testProjects
    Projects to tests, to define the variable "ShouldTest".
    Not empty, ShouldTest will be "true".

.PARAMETER ForceAssemblyVersion
    Assembly version to use instead of the computed version.
    Default is "".

.PARAMETER ForcePackageVersion
    Package version to use instead of the computed version.
    Default is "".

.EXAMPLE
    $> .\compute-version-variables -branchName "dev" -buildNumber "4.6.1.24123.3" -packageSuffix "Preview"

.EXAMPLE
    # Compute AssemblyVersion and PackageVersion
    # -> Update version.yml
    - task: PowerShell@2
      displayName: 'Compute AssemblyVersion and PackageVersion'
      inputs:
          targetType: 'filePath'
          filePath: $(System.DefaultWorkingDirectory)/eng/pipelines/compute-version-variables.ps1
          arguments: > # Use this to avoid newline characters in multiline string
          -branchName "$(Build.SourceBranchName)"
          -buildNumber "$(Build.BuildNumber)"
          -packageSuffix "$(PackageSuffix)"
          -ForceAssemblyVersion "${{ parameters.ForcedAssemblyVersion }}"
          -ForcePackageVersion "${{ parameters.ForcedNugetPackageVersion }}"

#>

param (
    [Parameter(Mandatory = $true)][ValidateNotNullOrEmpty()][string]$branchName,
    [Parameter(Mandatory = $true)][ValidateNotNullOrEmpty()][string]$buildNumber,
    [string]$packageSuffix,
    [string]$testProjects,
    [string]$ForceAssemblyVersion,
    [string]$ForcePackageVersion
)

Write-Host "Compute AssemblyVersion and PackageVersion."
Write-Host ""

# Default values
$branch = "PR"
$package = ""

# To Test?
$toTest = "true"

# BranchName = dev, main, archive or PR
if ($branchName -eq "main") {
    $branch = "main"
}
elseif ($branchName -eq "dev") {
    $branch = "dev"
}
# elseif ("$(Build.SourceBranch)" -like "refs/heads/archives/*")
elseif ($branchName -like "*/archives/*" -or $branchName -like "archive-*") {
    $branch = "archive"
}
else {
    $branch = "PR"
}

# [1, 2, 4, 23296, 1]
$builds = $buildNumber.Split('.')

# 1.2.4
$fileVersion = "$($builds[0]).$($builds[1]).$($builds[2])"

# 1.2.4.23296
$assembly = "$($builds[0]).$($builds[1]).$($builds[2]).$($builds[3])"

# Main or Archive without PackageSuffix: 1.2.4
# Main or Archive with    PackageSuffix: 1.2.4-rc.1
if ($branch -eq "main" -or $branch -eq "archive") {
    # Main without PackageSuffix
    if ($packageSuffix -eq "") {
        $package = "$($builds[0]).$($builds[1]).$($builds[2])"
    }

    # Main with PackageSuffix
    else {
        $package = "$($builds[0]).$($builds[1]).$($builds[2])-$packageSuffix"
    }

    $toTest = "true"
}


# Dev without PackageSuffix: 1.2.4-preview-23296-1
# Dev with    PackageSuffix: 1.2.4-beta.1
elseif ($branch -eq "dev") {
    if ($packageSuffix -eq "") {
        $package = "$($builds[0]).$($builds[1]).$($builds[2])-preview.$($builds[3]).$($builds[4])"
    }
    else {
        $package = "$($builds[0]).$($builds[1]).$($builds[2])-$packageSuffix"
    }
    $toTest = "true"
}

# Other branches: 1.2.4-preview-23296-1
else {
    $package = "$($builds[0]).$($builds[1]).$($builds[2])-preview.$($builds[3]).$($builds[4])"
    $toTest = "true"
}

# Force versions if specified
if (-not [string]::IsNullOrWhiteSpace($ForceAssemblyVersion)) {
    $assembly = $ForceAssemblyVersion
}

if (-not [string]::IsNullOrWhiteSpace($ForcePackageVersion)) {
    $package = $ForcePackageVersion
}

if ($testProjects -eq "") {
    $toTest = "false"
}

# Set the output variable for use in other tasks.
Write-Host "##vso[task.setvariable variable=AssemblyVersion]$assembly"
Write-Host "##vso[task.setvariable variable=PackageVersion]$package"
Write-Host "##vso[task.setvariable variable=ShouldTest]$toTest"

# Display computed versions
Write-Host ""
Write-Host "----------------------------------------------- "
Write-Host "Parameters:"
Write-Host "----------------------------------------------- "
Write-Host " -  Branch                 = $branch "
Write-Host " -  BuildNumber            = $buildNumber "
Write-Host " -  PackageSuffix          = $packageSuffix "
Write-Host " -  ForceAssemblyVersion   = $ForceAssemblyVersion "
Write-Host " -  ForcePackageVersion    = $ForcePackageVersion "
Write-Host "----------------------------------------------- "
Write-Host "Computed:"
Write-Host "----------------------------------------------- "
Write-Host " -> AssemblyVersion        = $assembly "
Write-Host " -> PackageVersion         = $package "
Write-Host " -> ShouldTest             = $toTest "
Write-Host "----------------------------------------------- "
