# Dependency validation procedure

This procedure is mandatory for the update-dependencies skill. Run all commands
from the disposable validation copy, using actual repository scripts and runners.
Only .NET 8, 9, and 10 are supported. Missing required prerequisites are blockers.

## A. Restore and build the Core library

Validate [Core](../../../../src/Core/Microsoft.FluentUI.AspNetCore.Components.csproj)
for each of its declared frameworks: `net8.0`, `net9.0`, and `net10.0`.
There is no Charts library in this branch.

Run three explicit restore/build pairs. A default Debug build or solution
restore does not replace this matrix. Run sequentially because builds can share
intermediate outputs. Use the same configuration and framework for each pair.
The project declares `TargetFrameworks` directly; do not use the v5-only
`TargetNetVersions` property.

For each `$tfm`, from the validation copy's root:

```powershell
$project = 'src\Core\Microsoft.FluentUI.AspNetCore.Components.csproj'
$tfms = @('net8.0', 'net9.0', 'net10.0')
# Run these commands separately for each value in $tfms.
dotnet restore $project -p:Configuration=Release "-p:TargetFramework=$tfm"
# Run the build only if this restore succeeded.
dotnet build $project --configuration Release --framework $tfm --no-restore
```

Check native command exit codes explicitly, including in loops: PowerShell does
not automatically stop on a nonzero native exit code. Record a failed restore
and its blocked build separately, then continue independent matrix entries.
Do not add unsupported frameworks or skip a required entry as a success.

## B. Install and build Core.Assets

The only NPM project is
[Core.Assets](../../../../src/Core.Assets/package.json).
Use Node 22.x or newer, subject to the proposed dependencies' requirements.
Use `npm install --package-lock=true` to generate/update the lockfile alongside
the proposed manifest, followed by the actual build script only if installation
succeeds. Do not substitute `npm ci` or use `--package-lock=false`.

```powershell
Push-Location 'src\Core.Assets'
try {
    npm install --package-lock=true
    if ($LASTEXITCODE -ne 0) { throw 'Core.Assets install failed.' }
    npm run build
    if ($LASTEXITCODE -ne 0) { throw 'Core.Assets build failed.' }
    npm ls --depth=0
    if ($LASTEXITCODE -ne 0) { throw 'Core.Assets dependency validation failed.' }
} finally {
    Pop-Location
}
```

Check every exit code and restore the working directory even on failure. Review
resolved top-level versions and peer dependency errors with `npm ls --depth=0`.
Verify that the final manifest ranges and corresponding lockfile resolutions
keep `@fluentui/web-components` on **2.x** and `@microsoft/fast-element` on
**1.x**. A successful install or build does not waive these v4 major-version
constraints; reject candidates that cross either boundary.
An installation failure is not a successful build. The esbuild configuration
already bundles/minifies the assets; there is no `--build-mode=Release` option.
Do not copy the generated, ignored `dist` directory back to the working tree.

For setup, authentication errors (`E401` / `401 Unable to authenticate`), or
feed caching problems, consult the
[Core.Assets README](../../../../src/Core.Assets/README.md).
A feed cache miss can appear as an authentication error. Stay within the skill's
change boundary: do not modify NPM configuration or discard lockfiles; obtain
approval before installing global tools or clearing the global NPM cache.
Keep credentials out of reports and copied artifacts.

After a successful install and build, verify the lockfile agrees with the final
`package.json`. Copy only the validated `package-lock.json` to the original
working tree, subject to the skill's concurrent-change and revalidation rules.

## C. Execute the Core bUnit tests

Rediscover bUnit test projects through package references and configuration,
not filenames alone. This branch has one bUnit project:

- [Microsoft.FluentUI.AspNetCore.Components.Tests.csproj](../../../../tests/Core/Microsoft.FluentUI.AspNetCore.Components.Tests.csproj)

It targets `net10.0` and uses bUnit, xUnit, `Microsoft.NET.Test.Sdk`, and the
VSTest runner. There is no `global.json` selecting Microsoft.Testing.Platform.
Do not use the v5 `dotnet test --project` syntax or MTP-specific options.

Execute the complete suite in Release using its declared framework. This
command restores and builds the project and its references before running:

```powershell
$testProject = 'tests\Core\Microsoft.FluentUI.AspNetCore.Components.Tests.csproj'
dotnet test $testProject --configuration Release --framework net10.0 --logger trx --results-directory TestResults --verbosity normal
```

Do not force the tests onto all library TFMs, retarget projects, use
`TargetNetVersions`, or use `--no-build` with stale assemblies. Do not create
`global.json` or migrate a test framework to make a command work.

Template validation projects under `tests\TemplateValidation` are not bUnit
suites and are excluded from this test matrix. Do not restore, build, or run
Playwright/browser integration projects for test validation; do not download
browsers or install Playwright tooling. Report non-bUnit/browser exclusions
explicitly rather than as blocked or passed checks. Do not require absent v5
Charts, DocViewer, McpServer, DocApiGen, or shared test-helper projects.

Check the exit code and record passed, failed, and skipped test counts.
A zero-test run is not successful validation. Explicitly report runner
incompatibilities, missing prerequisites, and blocked tests; do not silently
omit the project or accept snapshots.
