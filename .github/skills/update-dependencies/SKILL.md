---
name: update-dependencies
description: "Check and update NuGet and NPM dependencies, then validate .NET 8, 9, 10, and 11 builds and all test projects. Use when asked to update or upgrade repository dependencies."
user-invocable: true
disable-model-invocation: true
---

# Update and validate dependencies

Update this repository's NuGet and NPM dependencies to the newest compatible
versions, then complete every validation below. Inspect the repository's current
configuration rather than relying on assumptions about SDKs, scripts, or runners.

## Strict change boundary

- Only dependency-version changes in repository-owned files named
  `Directory.Packages.props` or `package.json` are allowed.
- Do not modify project files, `global.json`, NuGet/NPM configuration, build
  scripts, application code, tests, snapshots, generated sources, or lockfiles.
  Do not change NPM scripts or unrelated package metadata.
- Preserve existing user changes, comments, formatting, MSBuild conditions, and
  dependency range conventions. Never reset or discard someone else's work.
- If an upgrade requires changes outside the allowlist, select a compatible
  version or leave that dependency unchanged and explain why.
- Do not commit, push, create a pull request, or add a report file.

## 1. Inspect the repository and prerequisites

1. Record the initial Git status and existing changes, including untracked files.
   Capture the starting dependency versions and shared MSBuild property values
   from the working tree, not just `HEAD`, for the final before/after report.
2. Locate dependency manifests, their consuming projects, package sources, NPM
   workspaces, build scripts, and actual test projects. Exclude generated,
   vendored, and installed dependency directories.
3. Read [Directory.Packages.props](../../../Directory.Packages.props),
   [Directory.Build.props](../../../Directory.Build.props), and
   [global.json](../../../global.json). Inspect framework overrides, project
   references, test runners, and configuration-dependent build behavior.
4. Check `dotnet --info`, `dotnet --list-sdks`, `dotnet --list-runtimes`,
   `node --version`, and `npm --version`. A usable SDK must support `net11.0`
   and all earlier requested targets; test runtimes and Node must satisfy the
   projects' requirements. Do not edit SDK pins or install system-wide tools
   without approval. Report unavailable prerequisites as blockers, not passes.
5. Before installing or building, prepare an isolated, disposable validation copy
   outside the working tree. Include the current source, proposed manifest
   changes, and relevant pre-existing user changes, but exclude `.git` and
   generated/install directories. A clean checkout of `HEAD` alone is not enough.
   Preserve relative paths and required resources. If external references or
   credentials prevent equivalent validation there, report the limitation.

Run restore, install, build, and test commands in that copy. Script builds can
rewrite tracked `src\BuildConstants.ts` files, so they must not run in the
original working tree. Generated outputs in the disposable copy are acceptable;
never copy them back.

## 2. Identify and assess updates

- Query the configured NuGet sources and NPM registry for actual published
  versions. Use package metadata and official release notes, not guessed versions
  or a textual sort of version strings. Never disclose feed credentials.
- Default to the latest compatible stable release for stable dependencies.
  Do not introduce prereleases into stable dependency lines without approval.
  For an existing prerelease dependency, compare the latest compatible prerelease
  and stable alternative; do not downgrade it automatically.
- Keep .NET framework package families within their corresponding major version:
  net8.0 uses 8.x, net9.0 uses 9.x, net10.0 uses 10.x, and net11.0 uses 11.x.
  Respect existing preview/RC requirements when no suitable stable release exists.
- Check supported TFMs, minimum SDK/Node versions, peer dependencies, related
  package constraints, release notes, breaking changes, and known advisories.
  Update tightly coupled package families together.
- For NPM, inspect both `dependencies` and `devDependencies`, plus any existing
  peer/optional dependencies and overrides. Preserve range styles unless a
  justified dependency constraint change is necessary. An `npm outdated` exit
  code indicating outdated packages is not itself a registry failure.
- Distinguish external dependencies from repository-owned package/version
  relationships; do not unintentionally switch local development to unrelated
  published package versions.
- Present a concise proposal with ecosystem, package/property, current version,
  proposed version, compatibility constraints, and release-note links. Ask for
  approval before potentially breaking major upgrades or uncertain changes.
  Proceed with compatible, non-breaking updates without unnecessary questions.

### Important: shared versions in Directory.Packages.props

This repository uses NuGet Central Package Management. Its `PropertyGroup`
centralizes common versions, including `RuntimeVersion8`, `AspNetCoreVersion8`,
`EfCoreVersion8`, and the corresponding properties for .NET 9, 10, and 11.
Conditional `ItemGroup` sections select packages for each `TargetFramework`.

When a `PackageVersion` uses an expression such as
`Version="$(AspNetCoreVersion8)"`, update the property in `PropertyGroup`, not
each consuming attribute. Keep the expressions and framework conditions intact.
Before changing a shared property, identify **every** package that consumes it
and verify that the proposed version is published and compatible for all of
them. Do not assume that runtime, ASP.NET Core, and EF Core packages always
publish identical version numbers.

Update literal `PackageVersion` values directly only when they are not already
property-backed. Do not inline shared values, duplicate version declarations,
remove framework groups, or move version definitions into other files.

## 3. Apply compatible changes

Make focused version edits only in the allowed manifests. Do not run broad
upgrade commands that can rewrite project files or lockfiles. Do not use
`npm audit fix`, `--force`, or `--legacy-peer-deps` to hide incompatibilities.

Refresh the validation copy with the exact proposed manifests before each
validation attempt. If a candidate fails because of an upgrade, investigate it
and try a compatible version or revert only that attempted version change,
preserving the initial user state. Then rerun the affected checks and complete
the final validation matrix against the final manifest contents.

## 4. Required final validation

Use the repository's real build scripts and test runners. Capture each command,
its exit code, and its result. Do not skip required checks merely because no
dependency needed updating. Do not suppress new errors or relax warnings,
dependency constraints, or tests to obtain a successful result.

### A. Restore and build both libraries for all four target frameworks

Validate these projects:

- [Microsoft.FluentUI.AspNetCore.Components.csproj](../../../src/Core/Microsoft.FluentUI.AspNetCore.Components.csproj)
- [Microsoft.FluentUI.AspNetCore.Components.Charts.csproj](../../../src/Charts/Microsoft.FluentUI.AspNetCore.Components.Charts.csproj)

Both use `TargetNetVersions` from `Directory.Build.props`; a default Debug build
does not prove that all frameworks compile. Run **eight explicit restore/build
pairs**, using the same configuration and framework properties for each pair.
Run sequentially because project references can share intermediate outputs.

For each project path and each TFM in `net8.0`, `net9.0`, `net10.0`, `net11.0`,
run the following PowerShell command pattern from the validation copy's root:

```powershell
dotnet restore $project -p:Configuration=Release "-p:TargetNetVersions=$tfm"
# Run the build only if this restore succeeded.
dotnet build $project --configuration Release --framework $tfm --no-restore "-p:TargetNetVersions=$tfm"
```

Use these exact project paths:

```text
src\Core\Microsoft.FluentUI.AspNetCore.Components.csproj
src\Charts\Microsoft.FluentUI.AspNetCore.Components.Charts.csproj
```

Check native command exit codes explicitly in any loop; PowerShell does not
automatically stop on a nonzero native exit code. Record a failed restore and
its blocked build separately, then continue with independent matrix entries.
Do not replace this matrix with one default build or a solution restore.

### B. Install and build both NPM projects

In each directory below, run `npm install` followed by its actual build script.
Use `--package-lock=false` so the existing lockfile neither pins the validation
to old resolved versions nor gets rewritten. Do not substitute `npm ci`.

```powershell
Push-Location "src\Core.Scripts"
npm install --package-lock=false
# Run only after a successful install.
npm run build -- --build-mode=Release
Pop-Location

Push-Location "src\Charts.Scripts"
npm install --package-lock=false
# Run only after a successful install.
npm run build -- --build-mode=Release
Pop-Location
```

Run Core.Scripts before Charts.Scripts. Check every exit code and restore the
working directory even on failure. Review resolved top-level versions and peer
dependency errors with `npm ls --depth=0`; do not treat an installation failure
as a successful build. Do not generate or update lockfiles in the original tree.

### C. Execute all actual test projects

Rediscover test projects from their configuration, not just filenames.
Currently inspect at least:

- [Components.Tests.csproj](../../../tests/Core/Components.Tests.csproj)
- [Components.Charts.Tests.csproj](../../../tests/Charts/Components.Charts.Tests.csproj)
- [Components.IntegrationTests.csproj](../../../tests/Integration/Components.IntegrationTests.csproj)
- [McpServer.Tests.csproj](../../../tests/McpServer/McpServer.Tests.csproj)

[Components.Tests.Tools.csproj](../../../tests/Tools/Components.Tests.Tools.csproj)
is a shared helper library, not a separately executable test suite.

Restore/build each real test project and run its complete suite in Release
against the updated dependencies, using its declared framework(s). The library
build matrix does not require forcing every test project onto all four TFMs.
Do not use `--no-build` with stale test assemblies.

The current `global.json` selects Microsoft.Testing.Platform. With SDK 10+,
use named project arguments and pass MTP options directly, without a `--`
separator. For compatible MTP projects, the command pattern is:

```powershell
dotnet test --project $testProject --configuration Release
```

Inspect each project's runner before executing it: the integration project
currently references legacy xUnit/VSTest packages. If it is not MTP-compatible,
build it and run its freshly built test assembly with `dotnet vstest`, resolving
the actual output path from MSBuild rather than guessing it. Do not edit
`global.json` or migrate a test framework to make the command work.

Record passed, failed, and skipped test counts. A zero-test run is not successful
validation. Report runner incompatibilities, missing browser/service prerequisites,
and blocked tests explicitly; do not silently omit a project or accept snapshots.

## 5. Final review and report

1. Inspect the final manifest diff and Git status against the initial state.
   The changes introduced by this operation must include only allowed manifests
   and dependency versions. Leave unrelated user changes untouched.
2. Confirm that the final proposed manifests are exactly those validated.
   Remove only the specifically identified disposable validation workspace and
   temporary files created by this operation; preserve user-owned files.
3. Respond with:
   - A mandatory table of applied NuGet/NPM updates with previous and new
     versions, using the format below.
   - Deferred/rejected upgrades, compatibility reasons, and remaining advisories.
   - A library matrix: two projects by four TFMs, with separate restore/build
     results for all eight combinations.
   - Install/build results for Core.Scripts and Charts.Scripts.
   - Results and test counts for every real test project.
   - Exact validation commands and any failures, blockers, or unexecuted checks.
   - The list of modified manifests and confirmation that no other files changed
     because of this operation.

Use this format for the dependency-update table in the final chat response:

| Ecosystem | Package | Manifest / Target framework | Previous version | New version | Shared MSBuild property |
|-----------|---------|-----------------------------|------------------|-------------|-------------------------|
| NuGet | <package ID> | <manifest and TFM> | <starting version> | <final version> | <property name or N/A> |
| NPM | <package name> | <manifest> | <starting range> | <final range> | N/A |

Include one row per updated package and manifest/framework combination. When a
shared property changes, list every affected NuGet package with its effective
previous and new versions, not just the property expression. For NPM, preserve
the exact manifest ranges, including prefixes such as `^` or `~`.
Include only updates retained in the final manifests, not rejected or reverted
attempts. Always provide the report, even if validation fails. If nothing was
updated, explicitly state "No dependencies were updated" instead of inventing
table entries.

Declare success only if every required restore, build, and test run passed.
Otherwise clearly state that validation is incomplete or failed.

Workflow inspiration:
[Aspire dependency-update skill](https://github.com/microsoft/aspire/blob/main/.agents/skills/dependency-update/SKILL.md).
Use its discover/assess/update/verify approach, not its repository-specific
internal mirroring pipeline or companion scripts.
