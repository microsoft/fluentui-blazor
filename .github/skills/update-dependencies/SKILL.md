---
name: update-dependencies
description: "Check and update NuGet and NPM dependencies and NPM lockfiles, then validate .NET 8, 9, and 10 builds, .NET 11 builds when a compatible SDK is already installed, and bUnit test projects only. Use when asked to update or upgrade repository dependencies."
user-invocable: true
disable-model-invocation: true
---

# Update and validate dependencies

Update this repository's NuGet and NPM dependencies to the newest compatible
versions, then complete every validation below. Inspect the repository's current
configuration rather than relying on assumptions about SDKs, scripts, or runners.

## Work rules: local changes only

- Use the current local branch. Do not create or switch branches.
- Modify files locally only; do not create local commits.
- Do not publish anything to Git or GitHub: no pushes, pull requests, comments,
  or remote status changes.
- Keep all changes uncommitted in the current working tree.

## Strict change boundary

- Only dependency-version changes in repository-owned files named
  `Directory.Packages.props` or `package.json`, plus corresponding NPM-generated
  `package-lock.json` updates, are allowed.
- Generate or update `package-lock.json` with NPM to keep dependency resolutions,
  integrity hashes, and related lockfile metadata synchronized with the manifests.
  Do not edit lockfiles manually or introduce unrelated lockfile changes.
- Do not modify project files, `global.json`, NuGet/NPM configuration, build
  scripts, application code, tests, snapshots, generated sources, or other lockfiles.
  Do not change NPM scripts or unrelated package metadata.
- Preserve existing user changes, comments, formatting, MSBuild conditions, and
  dependency range conventions. Never reset or discard someone else's work.
- If an upgrade requires changes outside the allowlist, select a compatible
  version or leave that dependency unchanged and explain why.
- Do not add a report file.

## 1. Inspect the repository and prerequisites

1. Record the initial Git status and existing changes, including untracked files.
   Capture the starting dependency versions and shared MSBuild property values
   and existing NPM lockfiles from the working tree, not just `HEAD`, for the
   final before/after report.
2. Locate dependency manifests, their consuming projects, package sources, NPM
   workspaces, build scripts, and actual test projects. Exclude generated,
   vendored, and installed dependency directories.
3. Read [Directory.Packages.props](../../../Directory.Packages.props),
   [Directory.Build.props](../../../Directory.Build.props), and
   [global.json](../../../global.json). Inspect framework overrides, project
   references, test runners, and configuration-dependent build behavior.
4. Check `dotnet --info`, `dotnet --list-sdks`, `dotnet --list-runtimes`,
   `node --version`, and `npm --version`. A usable SDK must support the required
   `net8.0`, `net9.0`, and `net10.0` targets; test runtimes and Node must satisfy
   the projects' requirements. Include `net11.0` validation only when an already
   installed SDK supports it, including an appropriate .NET 11 Preview or RC SDK.
   Otherwise skip .NET 11 validation and report the reason; this optional skip
   is not a blocker. Do not download or install an SDK just to validate .NET 11.
   Do not edit SDK pins or install system-wide tools without approval. Report
   unavailable prerequisites for required checks as blockers, not passes.
5. Before installing or building, prepare an isolated, disposable validation copy
   outside the working tree. Include the current source, proposed manifest
   changes, existing NPM lockfiles, and relevant pre-existing user changes, but exclude `.git` and
   generated/install directories. A clean checkout of `HEAD` alone is not enough.
   Preserve relative paths and required resources. If external references or
   credentials prevent equivalent validation there, report the limitation.

Run restore, install, build, and test commands in that copy. Script builds can
rewrite tracked `src\BuildConstants.ts` files, so they must not run in the
original working tree. Generated outputs in the disposable copy are acceptable;
never copy them back, except for the validated `package-lock.json` files explicitly
allowed in section B.

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

### Important: TypeScript and @typescript-eslint compatibility

Upgrading to TypeScript 7 can cause parser/lint errors when the selected
`@typescript-eslint` packages do not support it. Keep TypeScript on the latest
compatible **5.x** version by default in this repository.

Recheck this restriction on every dependency-update run: consult the
[official TypeScript support policy](https://typescript-eslint.io/users/dependency-versions/#typescript),
release notes, and peer dependencies for the exact proposed versions of
`@typescript-eslint/parser` and `@typescript-eslint/eslint-plugin`.
A permissive peer dependency range or a successful `npm install` alone does not
prove official support. If support for a newer TypeScript major is confirmed,
request approval before upgrading, then run the existing lint scripts, where
available, as well as all required builds and tests. Do not assume this
compatibility restriction is permanent; report the findings and any deferred
TypeScript upgrade in the final report.

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
upgrade commands that can rewrite project files or unrelated lockfiles. Generate
the corresponding NPM lockfile updates in the validation copy. Do not use
`npm audit fix`, `--force`, or `--legacy-peer-deps` to hide incompatibilities.

Refresh the validation copy with the exact proposed manifests and current
candidate lockfiles before each validation attempt. If a candidate fails because
of an upgrade, investigate it and try a compatible version or revert only that
attempted version change, preserving the initial user state. Regenerate the
affected NPM lockfiles as needed. Then rerun the affected checks and complete
the final validation matrix against the final manifest and lockfile contents.

## 4. Required final validation

Use the repository's real build scripts and test runners. Capture each command,
its exit code, and its result. Do not skip required checks merely because no
dependency needed updating. Do not suppress new errors or relax warnings,
dependency constraints, or tests to obtain a successful result.

### A. Restore and build both libraries for the applicable target frameworks

Validate these projects:

- [Microsoft.FluentUI.AspNetCore.Components.csproj](../../../src/Core/Microsoft.FluentUI.AspNetCore.Components.csproj)
- [Microsoft.FluentUI.AspNetCore.Components.Charts.csproj](../../../src/Charts/Microsoft.FluentUI.AspNetCore.Components.Charts.csproj)

Both use `TargetNetVersions` from `Directory.Build.props`; a default Debug build
does not prove that all frameworks compile. Validate `net8.0`, `net9.0`, and
`net10.0`. Add `net11.0` only when the installed SDK supports it; otherwise mark
both .NET 11 entries as intentionally skipped, not failed or blocked.

Run **six explicit restore/build pairs**, or **eight when .NET 11 is available**,
using the same configuration and framework properties for each pair.
Run sequentially because project references can share intermediate outputs.

Set `$tfms` to the applicable list, then run the following PowerShell command
pattern for each project path and each `$tfm` from the validation copy's root:

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

Check native command exit codes explicitly in any loop; PowerShell does not
automatically stop on a nonzero native exit code. Record a failed restore and
its blocked build separately, then continue with independent matrix entries.
Do not replace this matrix with one default build or a solution restore.

### B. Install and build both NPM projects

In each directory below, run `npm install` followed by its actual build script.
Use `--package-lock=true` to generate or update `package-lock.json` alongside the
proposed manifest. Keep existing locked resolutions when compatible; do not
delete lockfiles to force unrelated dependency upgrades. Do not use
`--package-lock=false` or substitute `npm ci` for this update step.

For NPM setup, authentication errors (`E401` / `401 Unable to authenticate`),
or feed caching problems, consult the
[Core.Scripts README](../../../src/Core.Scripts/README.md).
It also explains why a feed cache miss can appear as an authentication error.
Apply its troubleshooting guidance within this skill's change boundary: do not
modify NPM configuration or discard existing lockfiles, and obtain approval
before installing global tools or clearing the global NPM cache.

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

Run Core.Scripts before Charts.Scripts. Check every exit code and restore the
working directory even on failure. Review resolved top-level versions and peer
dependency errors with `npm ls --depth=0`; do not treat an installation failure
as a successful build.

After successful installs and builds, verify that each lockfile agrees with its
final `package.json`. Copy only those validated `package-lock.json` files back to
their corresponding locations in the original working tree. Preserve unrelated
user changes, and do not overwrite a lockfile changed concurrently since the
initial capture. If a manifest changes after validation, refresh its lockfile
and rerun the affected install/build checks before copying it back.

### C. Execute bUnit test projects only

Rediscover bUnit test projects from their package references and configuration,
not just filenames. Currently these include:

- [Components.Tests.csproj](../../../tests/Core/Components.Tests.csproj)
- [Components.Charts.Tests.csproj](../../../tests/Charts/Components.Charts.Tests.csproj)
- [FluentUI.Demo.DocViewer.Tests.csproj](../../../examples/Tools/FluentUI.Demo.DocViewer.Tests/FluentUI.Demo.DocViewer.Tests.csproj)

[Components.Tests.Tools.csproj](../../../tests/Tools/Components.Tests.Tools.csproj)
is a shared helper library, not a separately executable test suite.

Do not restore, build, or run Playwright/browser integration test projects for
test validation. Do not download browsers or install Playwright tooling for this
workflow. Non-bUnit test projects, including McpServer and DocApiGen test suites,
are also outside the validation scope. Report these exclusions explicitly rather
than treating them as blocked checks or successful test runs.

Restore/build each bUnit test project and run its complete suite in Release
against the updated dependencies, using its declared framework(s). The library
build matrix does not require forcing every test project onto all four TFMs.
Do not use `--no-build` with stale test assemblies.

Pass the applicable library TFM list from section A through `TargetNetVersions`
for test restore/build/run commands. This prevents referenced libraries from
requesting .NET 11 when its SDK is absent, without changing test project TFMs or
editing project files. Quote the semicolon-separated property value as shown;
do not use `%3B`, which can break framework negotiation for project references.

The current `global.json` selects Microsoft.Testing.Platform. With SDK 10+,
use named project arguments and pass MTP options directly, without a `--`
separator. For compatible MTP projects, the command pattern is:

```powershell
$targetNetVersionsArgument = '-p:TargetNetVersions="' + ($tfms -join ';') + '"'
dotnet test --project $testProject --configuration Release $targetNetVersionsArgument
```

Inspect each bUnit project's runner before executing it. Do not edit `global.json`
or migrate a test framework to make the command work.

Record passed, failed, and skipped test counts. A zero-test run is not successful
validation. Report runner incompatibilities, missing prerequisites for bUnit
tests, and blocked bUnit tests explicitly; do not silently omit an in-scope
project or accept snapshots.

## 5. Final review and report

1. Inspect the final manifest and lockfile diff and Git status against the initial
   state. The changes introduced by this operation must include only allowed
   dependency-version edits and corresponding NPM-generated lockfile updates.
   Leave unrelated user changes untouched.
2. Confirm that the final proposed manifests and lockfiles are exactly those validated.
   Remove only the specifically identified disposable validation workspace and
   temporary files created by this operation; preserve user-owned files.
3. Respond with:
   - A mandatory table of applied NuGet/NPM updates with previous and new
     versions, using the format below.
   - Deferred/rejected upgrades, compatibility reasons, and remaining advisories.
   - A library matrix: two projects by four TFMs, with separate restore/build
     results for every applicable combination; explicitly mark .NET 11 as
     intentionally skipped when a compatible SDK is not already installed.
   - Install/build results and lockfile updates for Core.Scripts and Charts.Scripts.
   - Results and test counts for every bUnit test project, plus a clear statement
     that Playwright/browser and other non-bUnit test suites were excluded by scope.
   - Exact validation commands and any failures, blockers, or unexecuted checks.
   - The list of modified manifests and lockfiles, with previous/new resolved
     versions for lockfile-only dependency updates, and confirmation that no
     other files changed because of this operation.

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
An intentional .NET 11 skip due to an unavailable local SDK, and the exclusion of
non-bUnit tests, do not prevent success. Otherwise clearly state that validation
is incomplete or failed.

Workflow inspiration:
[Aspire dependency-update skill](https://github.com/microsoft/aspire/blob/main/.agents/skills/dependency-update/SKILL.md).
Use its discover/assess/update/verify approach, not its repository-specific
internal mirroring pipeline or companion scripts.
