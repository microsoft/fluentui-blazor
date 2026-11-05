# Dependency validation procedure

This procedure is mandatory for the update-dependencies skill. Run all commands
from the disposable validation copy, using actual repository scripts and runners.
Apply the framework/prerequisite policy defined in the skill.

## A. Restore and build the libraries

Validate both projects:

- [Core](../../../../src/Core/Microsoft.FluentUI.AspNetCore.Components.csproj)
- [Charts](../../../../src/Charts/Microsoft.FluentUI.AspNetCore.Components.Charts.csproj)

Both use `TargetNetVersions` from `Directory.Build.props`. A default Debug build
or solution restore does not replace the required matrix.

Set `$tfms` to `net8.0`, `net9.0`, and `net10.0`, adding `net11.0` only when the
installed SDK supports it. Run six explicit restore/build pairs, or eight with
.NET 11. Run sequentially because project references can share intermediate
outputs. Use the same configuration and framework properties for each pair.

For each project path and each `$tfm`, from the validation copy's root:

```powershell
$tfms = @('net8.0', 'net9.0', 'net10.0')
# Add 'net11.0' only when a compatible SDK is already installed.
dotnet restore $project -p:Configuration=Release "-p:TargetNetVersions=$tfm"
# Run the build only if this restore succeeded.
dotnet build $project --configuration Release --framework $tfm --no-restore "-p:TargetNetVersions=$tfm"
```

Use these exact project paths:

```text
src\Core\Microsoft.FluentUI.AspNetCore.Components.csproj
src\Charts\Microsoft.FluentUI.AspNetCore.Components.Charts.csproj
```

Check native command exit codes explicitly, including in loops: PowerShell does
not automatically stop on a nonzero native exit code. Record a failed restore
and its blocked build separately, then continue independent matrix entries.
Mark unavailable .NET 11 entries as intentionally skipped.

## B. Install and build both NPM projects

Run Core.Scripts before Charts.Scripts. In each directory, use `npm install
--package-lock=true` to generate/update the lockfile alongside the proposed
manifest, followed by the actual build script only if installation succeeds.
Do not substitute `npm ci` or use `--package-lock=false`.

```powershell
Push-Location "src\Core.Scripts"
npm install --package-lock=true
# Run only after a successful install.
npm run build -- --build-mode=Release
Pop-Location

Push-Location "src\Charts.Scripts"
npm install --package-lock=true
# Run only after a successful install.
npm run build -- --build-mode=Release
Pop-Location
```

Check every exit code and restore the working directory even on failure. Review
resolved top-level versions and peer dependency errors with `npm ls --depth=0`.
An installation failure is not a successful build.

For setup, authentication errors (`E401` / `401 Unable to authenticate`), or
feed caching problems, consult the
[Core.Scripts README](../../../../src/Core.Scripts/README.md).
A feed cache miss can appear as an authentication error. Stay within the skill's
change boundary: do not modify NPM configuration or discard lockfiles; obtain
approval before installing global tools or clearing the global NPM cache.

After successful installs and builds, verify each lockfile agrees with its final
`package.json`. Copy only validated `package-lock.json` files to the original
working tree, subject to the skill's concurrent-change and revalidation rules.

## C. Execute bUnit test projects only

Rediscover bUnit test projects through package references and configuration,
not filenames alone. Currently these include:

- [Components.Tests.csproj](../../../../tests/Core/Components.Tests.csproj)
- [Components.Charts.Tests.csproj](../../../../tests/Charts/Components.Charts.Tests.csproj)
- [FluentUI.Demo.DocViewer.Tests.csproj](../../../../examples/Tools/FluentUI.Demo.DocViewer.Tests/FluentUI.Demo.DocViewer.Tests.csproj)

[Components.Tests.Tools.csproj](../../../../tests/Tools/Components.Tests.Tools.csproj)
is a shared helper library, not an executable test suite.

Do not restore, build, or run Playwright/browser integration test projects for
test validation. Do not download browsers or install Playwright tooling.
Other non-bUnit suites, including McpServer and DocApiGen tests, are also
excluded. Report exclusions explicitly rather than as blocked or passed checks.

Restore/build each bUnit project and execute its complete suite in Release using
its declared frameworks. Do not force projects onto all library TFMs, retarget
them, or use `--no-build` with stale assemblies. For projects declaring .NET 11
targets, select only targets with the required prerequisites; intentionally skip
unavailable .NET 11 targets and execute other declared targets normally.

Pass the applicable library TFM list from section A through `TargetNetVersions`
for test restore/build/run. This prevents referenced libraries from requesting
unavailable .NET 11 SDK support without changing test project TFMs or files.
Quote the semicolon-separated value as shown; do not use `%3B`, which can break
framework negotiation for project references.

The current `global.json` selects Microsoft.Testing.Platform. With SDK 10+, use
named project arguments and pass MTP options directly, without a `--` separator.
For compatible MTP projects:

```powershell
$targetNetVersionsArgument = '-p:TargetNetVersions="' + ($tfms -join ';') + '"'
dotnet test --project $testProject --configuration Release $targetNetVersionsArgument
```

Inspect each project's runner before executing it. Do not edit `global.json`
or migrate a test framework to make a command work.

Record passed, failed, and skipped test counts. A zero-test run is not successful
validation. Explicitly report runner incompatibilities, missing prerequisites,
and blocked bUnit tests; do not silently omit projects or accept snapshots.
