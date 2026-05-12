# Install these required tools globally if not already present.
#
# 1. dotnet tool install --global dotnet-outdated-tool --add-source https://api.nuget.org/v3/index.json
# 2. npm install -g npm-check-updates --registry https://registry.npmjs.org/

dotnet outdated ./Microsoft.FluentUI-v5.slnx --upgrade --version-lock Major --pre-release Never

# vsts-npm-auth -config ./src/Core.Scripts/.npmrc -force
ncu -u --packageFile src/Core.Scripts/package.json
npm install --packageFile src/Core.Scripts/

Write-Host 'Check the updated files and run these commands manually:'
Write-Host '   Get-ChildItem -Path . -Include bin,obj -Recurse -Directory -Force | Remove-Item -Recurse -Force'
Write-Host '   dotnet clean .\Microsoft.FluentUI-v5.slnx'
Write-Host '   dotnet restore ./Microsoft.FluentUI-v5.slnx'
Write-Host '   dotnet build ./Microsoft.FluentUI-v5.slnx'
Write-Host '   dotnet test ./tests/Core/Components.Tests.csproj'
Write-Host '   dotnet build ./src/Core/Microsoft.FluentUI.AspNetCore.Components.csproj -p:TargetFramework=net10.0'