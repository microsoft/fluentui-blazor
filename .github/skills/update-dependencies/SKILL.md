---
name: update-dependencies
description: "Update repository NuGet and NPM dependencies and lockfiles to the latest compatible versions, then validate library builds and bUnit tests. Use when asked to update or upgrade repository dependencies."
user-invocable: true
disable-model-invocation: true
---

# Update and validate dependencies

Update dependencies using the repository's current configuration, not assumptions
about SDKs, scripts, or test runners.

## Execution contract

- Work on the current local branch. Do not create or switch branches, commit,
  push, create pull requests or comments, or change remote status.
- Allow only dependency-version edits in repository-owned
  `Directory.Packages.props` and `package.json` files, plus corresponding
  NPM-generated `package-lock.json` updates.
- Do not manually edit lockfiles, delete them to force upgrades, or introduce
  unrelated resolution changes. Preserve compatible locked resolutions.
- Do not modify project files, `global.json`, NuGet/NPM configuration, build
  scripts, NPM scripts or unrelated metadata, code, tests, snapshots, generated
  sources, or other lockfiles. Do not add a report file.
- Preserve existing user changes, comments, formatting, MSBuild conditions, and
  dependency range conventions. Never reset or discard someone else's work.
- If an upgrade requires changes outside this boundary, select a compatible
  version or leave the dependency unchanged and explain why.
- Run all restore, install, build, and test commands in an isolated disposable
  copy outside the working tree. Asset builds can rewrite generated asset
  outputs. Never copy generated outputs back, except for validated NPM
  lockfiles.
- Do not install system-wide tools or edit SDK pins without approval.

### .NET framework policy

Keep framework package families within their corresponding major version:
`net8.0` uses 8.x, `net9.0` uses 9.x, and `net10.0` uses 10.x.
This v4 branch supports only .NET 8, 9, and 10. Do not introduce .NET 11
frameworks, packages, version properties, or validation targets.

Validate all three supported frameworks; missing SDK/runtime prerequisites are
blockers, not intentional skips. Verify every shared-property consumer using
NuGet metadata before updating its version.

## 1. Inspect and capture the starting state

1. Record Git status, including untracked files and existing changes. Capture
   starting dependency versions, shared MSBuild values, and existing NPM
   lockfiles from the working tree, not just `HEAD`, for the final comparison.
2. Locate repository-owned manifests, consuming projects, package sources, NPM
   workspaces, build scripts, and actual test projects. Exclude generated,
   vendored, and installed dependency directories.
3. Read [Directory.Packages.props](../../../Directory.Packages.props) and
   [Directory.Build.props](../../../Directory.Build.props). This branch has no
   `global.json`; do not assume an SDK pin or Microsoft.Testing.Platform
   selection. Inspect framework declarations, project references, runners, and
   configuration-dependent build behavior.
4. Check `dotnet --info`, `dotnet --list-sdks`, `dotnet --list-runtimes`,
   `node --version`, and `npm --version` against project requirements.
5. Before installing or building, prepare the validation copy with current
   source, relevant pre-existing user changes, manifests, lockfiles, and required
   resources. Preserve relative paths; exclude `.git` and generated/install
   directories. A clean checkout of `HEAD` alone is not sufficient. Report
   limitations caused by external references or unavailable credentials.

### v4 repository map

- Library: `src\Core\Microsoft.FluentUI.AspNetCore.Components.csproj`, declaring
  `net8.0;net9.0;net10.0` directly. There is no `TargetNetVersions` property or
  Charts library to validate.
- NPM: `src\Core.Assets\package.json` and its `package-lock.json`. The build is
  `npm run build`, without a Release/build-mode argument. Use Node 22.x or newer
  as documented in [the contribution guide](../../../docs/contributing.md).
- bUnit: `tests\Core\Microsoft.FluentUI.AspNetCore.Components.Tests.csproj`,
  targeting `net10.0` and using xUnit/VSTest.
- Template validation has its own `Directory.Packages.props` disabling Central
  Package Management, with versions in project files. Template source projects
  also contain replacement tokens and literal versions. Do not edit these
  project files or replacement tokens under this skill's manifest-only contract;
  report affected dependencies as outside scope rather than claiming that
  every repository package was upgraded.

## 2. Select compatible versions

- Query configured NuGet sources and the NPM registry for published versions.
  Use metadata and official release notes, not guessed versions or textual
  version sorting. Never disclose feed credentials.
- Default to the latest compatible stable release. Do not introduce prereleases
  into stable lines without approval. For existing prereleases, compare the
  latest compatible prerelease and stable alternative; do not automatically
  downgrade. Respect framework preview/RC requirements.
- Check supported TFMs, minimum SDK/Node versions, peer dependencies, related
  constraints, breaking changes, and known advisories. Update tightly coupled
  families together.
- Inspect NPM `dependencies`, `devDependencies`, and existing peer/optional
  dependencies and overrides. Preserve range styles unless a justified
  constraint change is necessary. An `npm outdated` exit code indicating
  outdated packages is not itself a registry failure.
- Distinguish external dependencies from repository-owned package/version
  relationships; do not switch local development to unrelated published packages.
- Present a concise proposal: ecosystem, package/property, current and proposed
  versions, constraints, and release-note links. Ask for approval before
  potentially breaking major upgrades or uncertain changes; otherwise proceed.

### Shared NuGet versions

This repository uses Central Package Management with shared properties such as
`RuntimeVersion8`, `AspNetCoreVersion8`, and `EfCoreVersion8`, and corresponding
properties for .NET 9 and 10. Preserve the separate `netstandard2.0` package
group and all framework-conditional groups.

For `Version="$(AspNetCoreVersion8)"`, update the property in `PropertyGroup`,
not the consuming attribute. Identify every consumer and verify that the
proposed version is published and compatible for all of them. Runtime, ASP.NET
Core, and EF Core packages do not necessarily publish identical versions.

Update literal `PackageVersion` values directly only when not property-backed.
Preserve expressions and framework conditions; do not inline shared values,
duplicate declarations, remove framework groups, or move version definitions.

The central `Microsoft.FluentUI.AspNetCore.Components*` versions are v4 sample
package pins, not ordinary external dependencies. Keep them aligned with the
branch's release version; do not upgrade them to v5 or change them as part of
an external dependency refresh.

### Web component compatibility policy

For this v4 branch, keep `@fluentui/web-components` on **2.x** and
`@microsoft/fast-element` on **1.x**. Select the latest compatible versions
within those major lines and preserve the existing range style without allowing
the next major. These constraints apply to both manifest ranges and their
resolved lockfile versions. Do not propose or apply newer majors as part of this
dependency-update workflow; report them as outside the v4 compatibility policy.

### TypeScript policy

Keep TypeScript on the latest compatible **5.x** version by default.
Reevaluate this repository policy on each run using the
[official release notes](https://www.typescriptlang.org/docs/handbook/release-notes/),
the actual `src\Core.Assets\package.json` toolchain, and peer dependencies.
Do not assume the v5 `@typescript-eslint` toolchain is present.

If a newer TypeScript major is compatible with the configured toolchain, request
approval before upgrading. Run existing lint scripts, where available; do not
invent a lint script or treat an esbuild bundle as a TypeScript type check.
Report the findings and any deferred TypeScript upgrade.

## 3. Apply and validate

1. Make focused version edits in allowed manifests only. Avoid broad upgrade
   commands that rewrite other files. Do not use `npm audit fix`, `--force`, or
   `--legacy-peer-deps` to hide incompatibilities.
2. Refresh the validation copy with the exact proposed manifests and candidate
   lockfiles before each attempt.
3. **Read and follow [the validation procedure](./references/validation.md)
   before running validation.** Complete all required checks even when no
   dependency needed updating. Capture every command, exit code, and result.
4. If an upgrade fails, investigate and choose a compatible version or revert
   only that attempted version edit, preserving the initial user state.
   Regenerate affected lockfiles and rerun affected checks. Complete the final
   matrix against the final manifests and lockfiles.
5. Copy back only lockfiles whose final manifests, installs, and builds were
   validated. Do not overwrite a lockfile changed concurrently since capture.
   If a manifest changes after validation, refresh its lockfile and repeat the
   affected checks before copying.

Do not suppress new errors or relax warnings, dependency constraints, or tests
to obtain a successful result.

## 4. Review and report

Compare final manifest/lockfile diffs and Git status with the initial state.
Confirm that only allowed edits were introduced, unrelated user changes remain
intact, and final manifests/lockfiles are exactly those validated. Remove only
the specifically identified disposable workspace and temporary files created
by this operation.

Always report in chat, including when validation fails:

1. Applied updates using the table below. Include one row per package and
   manifest/framework combination. For shared properties, expand every affected
   package to its effective previous/new versions. Preserve exact NPM ranges,
   including `^` or `~`. Omit rejected/reverted attempts; if nothing changed,
   state "No dependencies were updated".
2. Deferred/rejected upgrades, compatibility reasons, and remaining advisories.
3. Library matrix: Core by .NET 8, 9, and 10, with separate restore/build
   results for all three frameworks.
4. Install/build results and lockfile updates for `src\Core.Assets`; results
   and passed/failed/skipped counts for every bUnit project. Explicitly identify
   browser/Playwright and other non-bUnit suites as excluded, not blocked/passed.
5. Exact validation commands, failures, blockers, and unexecuted checks.
6. Modified manifests/lockfiles, previous/new resolved versions for lockfile-only
   updates, and confirmation that no other files changed because of this run.

| Ecosystem | Package | Manifest / Target framework | Previous version | New version | Shared MSBuild property |
|-----------|---------|-----------------------------|------------------|-------------|-------------------------|
| NuGet | <package ID> | <manifest and TFM> | <starting version> | <final version> | <property name or N/A> |
| NPM | <package name> | <manifest> | <starting range> | <final range> | N/A |

Declare success only if all required restores, builds, and tests passed.
Excluded non-bUnit suites do not prevent success. Missing prerequisites or
unexecuted required checks mean validation is incomplete or failed.

Workflow inspiration:
[Aspire dependency-update skill](https://github.com/microsoft/aspire/blob/main/.agents/skills/dependency-update/SKILL.md).
Use its discover/assess/update/verify approach, not its repository-specific
internal mirroring pipeline or companion scripts.
