---
name: setup-fluentui-blazor
description: "Install Fluent UI Blazor and its MCP server for the user's IDE, migrate all non-system pages and components, configure the layout and navigation, clean up assets, and verify interactive rendering and compilation."
agent: agent
---

# Migrate this application to Fluent UI Blazor

## 1. Mandatory first action: ask which feed to use

Before reading any files, consulting documentation, running commands, or making
changes, ask the user only the following question, then wait for their answer:

> Which feed would you like to use for Fluent UI Blazor?
> - **nuget.org**: the latest stable version compatible with the project.
> - **Preview**: the latest compatible daily build of
>   `Microsoft.FluentUI.AspNetCore.Components` from the documented official feed.
>   Daily builds may contain breaking changes and are not intended for production.
>   All other packages, including the MCP server, must come from nuget.org.

Use an interactive question tool if one is available. Do not choose a feed on
the user's behalf or configure anything before receiving their answer.

### Execution rules after the answer

- Perform the actual application changes and validation. Documentation review,
  installation instructions, or an MCP configuration proposal alone do not
  complete this task.
- Read independent files and sources in parallel where possible. Keep one
  inventory and one set of verified package, feed, and rendering decisions for
  this execution. Reuse them rather than repeating discovery for every file.
- Make coherent groups of changes before validating them. Reuse existing
  configuration and already-satisfied requirements instead of adding duplicate
  references, providers, styles, or installations.
- Ask for unresolved application decisions or required approvals, grouping
  related questions where possible. Do not ask again about choices already
  confirmed. Report blockers explicitly; never silently change scope or APIs.
- MCP is not a general prerequisite. By default, install it for future work in
  step 9, after application validation. For an uncertain conversion, use the
  targeted lookup in step 5, bringing forward only the necessary setup, then
  resume that conversion. Do not restart the entire migration or let an
  MCP-only issue block work that can use the official sources.

## 2. Consult the official sources on every execution

Read application references 1-6 at the start of each execution, not just their
summaries. Read MCP references 7-9 when step 9 or a targeted lookup needs them.
These are living references: do not substitute remembered versions, feed URLs,
APIs, or procedures for their current contents. Within the same execution,
reuse retrieved documentation unless the selected version changes or additional
details are needed.

1. [C# and Blazor conventions](https://github.com/microsoft/fluentui-blazor/blob/dev/.github/skills/csharp-naming-conventions/README.md).
   Also follow the links to `SKILL.md` and the rules relevant to the files being
   modified. There is no need to install this skill globally.
2. [Using the Preview feed and daily builds](https://github.com/microsoft/fluentui-blazor/blob/dev/docs/using-latest-daily.md).
3. [Installing Fluent UI Blazor](https://github.com/microsoft/fluentui-blazor/blob/dev/examples/Demo/FluentUI.Demo.Client/Documentation/GetStarted/Installation.md).
4. [NavDefault navigation example using FluentNavItem](https://github.com/microsoft/fluentui-blazor/blob/dev/examples/Demo/FluentUI.Demo.Client/Documentation/Components/Nav/Examples/NavDefault.razor).
5. [FluentLayout and standard layout example](https://github.com/microsoft/fluentui-blazor/blob/dev/examples/Demo/FluentUI.Demo.Client/Documentation/Components/Layout/FluentLayout.md),
   especially the "Standard Layout (header, navigation and footer)" section.
6. [Blazor render modes](https://learn.microsoft.com/aspnet/core/blazor/components/render-modes),
   selecting the ASP.NET Core version that matches the project.
7. [Getting started with the Fluent UI Blazor MCP server](https://github.com/microsoft/fluentui-blazor/blob/dev/examples/Demo/FluentUI.Demo.Client/Documentation/GetStarted/Mcp/GetStarted.md).
   For the final setup step, or when resolving an uncertain conversion with MCP,
   also read the linked installation and version compatibility guides.
8. [MCP configuration in VS Code](https://code.visualstudio.com/docs/agent-customization/mcp-servers).
9. [MCP configuration in Visual Studio](https://learn.microsoft.com/visualstudio/ide/mcp-servers?view=visualstudio).

If GitHub does not display a file's contents, use its official Raw link.
If an essential application reference remains inaccessible, report the blocker
and ask how to proceed; do not invent its contents. Report unavailable MCP
resources separately and continue with accessible, version-appropriate official
sources whenever they resolve the application question.

## 3. Examine the existing application

Identify the relevant solution and projects, target framework, hosting model,
and current render mode: Blazor Web App, Server, standalone WebAssembly, or
another existing model.

Examine the relevant files: project files, NuGet configuration and any central
package version management, `Program.cs`, `_Imports.razor`, `App.razor`,
`Routes.razor`, `MainLayout.razor`, `NavMenu.razor`, their isolated stylesheets,
pages, and the HTML entry points applicable to the detected model.
Inventory the assets in `wwwroot` and their references before deleting anything.
Do not assume that all these files exist or share the same location.

Inventory every application page and Razor component across all relevant
projects, including shared, nested, and non-routable components. The only system
pages/components excluded from Fluent conversion are `ReconnectModal.razor`,
`Error.razor`, and `NotFound.razor`, wherever they are located. Preserve their
system behavior and do not convert their markup to Fluent components.
All other application pages and components are in scope, not only the layout,
navigation, or template demonstration pages.

Preserve the user's changes, routes, features, authorization, authentication,
labels, and site language.
Do not recreate the application from a template or change its framework or
hosting model without approval.

### Resolve compatibility and rendering decisions before editing

- Using the detected target framework and selected feed, determine the latest
  compatible core-package version: stable for nuget.org, daily for Preview.
  Verify that `FluentLayout`, `FluentNav`, and `FluentNavItem` exist in that
  version. Select the required companion packages independently from nuget.org.
- The `dev` branch may differ from the selected release. Consult the package's
  corresponding tag or source version when needed and use its installation
  instructions and APIs, not a mixture of versions.
- Preserve the current interactive rendering strategy when suitable. If a
  Blazor Web App is static SSR only, explain the need and ask the user to select
  a compatible interactive strategy before changing its architecture. Apply
  that decision in step 7, preserving pages that must remain static.
- If an application requirement is incompatible with the selected feed or
  framework, explain it and ask for a decision before editing: change the feed
  or explicitly accept an adjusted requirement. Do not silently substitute
  `FluentNavLink` for `FluentNavItem`, switch to Preview, or change the framework.

## 4. Install and configure Fluent UI Blazor

### Mandatory package routing and Preview nuget.config

The feed choice applies only to the exact package
`Microsoft.FluentUI.AspNetCore.Components`. With **Preview** selected:

| Package | Required source |
|---------|-----------------|
| `Microsoft.FluentUI.AspNetCore.Components` | Official Preview feed from the current daily-build guide |
| Every other package, including `.Icons`, `.Emoji`, and `Microsoft.FluentUI.AspNetCore.McpServer` | nuget.org |

**A repository-root `nuget.config` must exist on disk before restoring packages.**
Create it if missing, or merge into the existing repository NuGet configuration
without creating a duplicate with different filename casing. Follow the
`packageSources` and `packageSourceMapping` example in "Using the Preview feed
and daily builds"; merely registering a source globally or showing an example
in the chat is not sufficient.

For a new configuration, use this structure, replacing the Preview URL
placeholder with the actual URL read from the current daily-build guide:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <add key="preview" value="PREVIEW_FEED_URL_FROM_CURRENT_DAILY_BUILD_GUIDE" />
  </packageSources>
  <packageSourceMapping>
    <packageSource key="nuget.org">
      <package pattern="*" />
      <package pattern="Microsoft.FluentUI.AspNetCore.Components.Icons" />
      <package pattern="Microsoft.FluentUI.AspNetCore.Components.Emoji" />
      <package pattern="Microsoft.FluentUI.AspNetCore.McpServer" />
    </packageSource>
    <packageSource key="preview">
      <package pattern="Microsoft.FluentUI.AspNetCore.Components" />
    </packageSource>
  </packageSourceMapping>
</configuration>
```

The exact core-package mapping takes precedence over the nuget.org wildcard.
Never put `*`, `Microsoft.FluentUI.*`, or any other package under the Preview
mapping. Preserve unrelated sources and mappings needed by the repository when
merging, and remove conflicting Fluent UI mappings. If an existing dependency
requires an exception to this routing policy, explain it and ask before changing
that dependency's source. Do not alter the machine-wide NuGet configuration.
With **nuget.org** selected, do not introduce a Preview source for this migration.
Resolve the core package from nuget.org as well: adjust any existing Fluent UI
Preview mapping left by an earlier execution, without removing unrelated
sources or approved mappings needed by other dependencies.

### Install the application packages

- Install the packages required by the current documentation, including the
  main package and the icons package if the navigation uses icons.
- Use the versions selected in step 3 and the routing above, with explicit,
  reproducible versions. The core and icons packages need not share a version.
  `--prerelease` on nuget.org alone does not select the daily feed.
- Respect central package management when it is in place. Do not disable
  it automatically.
- Where tooling supports it, defer automatic restores until related package and
  configuration edits are ready; restore and build the coherent result in step 8.
- Add the imports, service registrations, styles, any scripts, and providers
  prescribed for the selected version in the appropriate locations.
  Avoid duplicate references and providers. For an application with a client
  project and server prerendering, configure every project and service container
  in which the components run.
- Do not hide restore or build errors. Do not globally disable warnings as
  errors or signature verification to work around an issue; explain any
  blocker that requires a decision.

## 5. Migrate the layout, navigation, pages, and components

### MainLayout.razor

Start from the standard layout in the documentation compatible with the installed
package, rather than retaining the Bootstrap structure.

- Use `FluentLayout` with a header, a navigation area containing `<NavMenu />`,
  a content area rendering `@Body`, and a simple footer.
- Use the components, areas, and parameters actually available in the selected
  version. Do not mix APIs from different versions.
- Provide navigation that works on desktop and mobile using the documented
  Fluent mechanism, without depending on Bootstrap JavaScript.
- Place the required providers in the appropriate render tree, once per
  relevant scope.
- Preserve an accessible semantic structure, the application title, and useful
  error-handling mechanisms.

### NavMenu.razor

`NavMenu.razor` remains a navigation component, not a new routable page.

- Build the navigation with `FluentNav` and **`FluentNavItem`** components,
  following the `NavDefault.razor` example provided in the sources.
- Set **`Style="height: 100%;"` on `FluentNav` itself** so it fills the available
  navigation height on desktop. Preserve any other necessary inline styles and
  ensure the containing layout area provides the available height; do not apply
  the height only to a wrapper or navigation item.
- Adapt the example to the project's actual pages: preserve existing
  destinations, labels, access restrictions, and useful behaviors.
- Configure links, the active state, and exact matching for the home page
  using supported parameters.
- Use icons from the installed package if needed. Add categories or separators
  only if the existing navigation warrants them.
- Do not copy demonstration routes, density controls, demo switches, placeholder
  external links, or sample dialogs into the site.

### Migrate all non-system pages and components

Apply the following rules to every page and component inventoried in step 3,
except `ReconnectModal.razor`, `Error.razor`, and `NotFound.razor`:

- Replace native UI elements with their documented Fluent equivalents where
  available, including text, buttons, links, labels, inputs, and other controls.
  Preserve content, localization, bindings, validation, event handlers, routes,
  authorization, and business logic.
- Convert paragraphs (`<p>`) and headings (`<h1>` through `<h6>`) to
  **`<FluentText>`**, using the typography and semantic parameters supported by
  the installed library version. Preserve heading levels, accessible document
  structure, text emphasis, and paragraph spacing rather than rendering all text
  with identical styling or losing heading semantics.
- Keep **`<table>` as a native HTML table**, including its semantic table
  structure, data, and bindings. **Remove its entire `class` attribute**:
  the library already supplies table styling. Do not replace it with a
  `FluentDataGrid`, keep Bootstrap/custom table classes, or reintroduce a class
  through attribute splatting.
- Retain required Blazor framework components, HTML document metadata,
  structural markup without a suitable Fluent equivalent, and the explicit
  native-table exception. Do not replace framework behavior or literal code
  examples simply because their source mentions HTML tags.
- Do not stop after converting only `MainLayout.razor` and `NavMenu.razor`.
  Review the entire inventory and account for every non-system page/component.

If unsure which Fluent component or parameters to use, **start the Fluent UI
MCP server whose launch command is `fluentui-mcp` through the active IDE/client**.
Discover and invoke its actual documentation tools to resolve the question.
If needed, perform the relevant installation/configuration from step 9 early,
then return to the conversion. Reuse that setup and query the specific API in
question rather than repeating installation or broad discovery for every file.
Check that the advice applies to the installed
library version; do not force a global MCP update or change the library version
merely to match the server. If MCP cannot be used or its documentation is
incompatible, report the limitation and consult the matching official sources.
Do not guess unsupported APIs or silently skip an uncertain conversion; ask the
user if the question remains unresolved.

## 6. Clean up styles and assets

All contents of `wwwroot` may be reorganized for this migration.
Remove assets that are no longer needed after checking their usage:
Bootstrap CSS/JS, source maps, old icons, and template styles.
Also remove their references from components, pages, HTML entry points,
asset configurations, and any PWA manifests or caches.

- **You must keep `wwwroot/app.css`, even if empty**, along with its valid
  reference, for future customizations.
- Remove almost all template styles from this file. Keep only the rules still
  required for functionality, accessibility, or the visual identity that must
  be preserved; briefly explain the rules retained.
- Also clean up obsolete isolated styles in `MainLayout.razor` and
  `NavMenu.razor`. Do not retain the old Bootstrap layout underneath Fluent.
- Search for Bootstrap classes still used in non-system pages and components.
  Replace their useful usages with Fluent components or suitable minimal styles
  before removing the dependency, without rewriting business logic.
- Preserve assets still in use: Blazor scripts, Fluent styles, generated isolated
  styles, images, the favicon, and authentication and PWA assets.
  Retain any minimal styles needed by the three excluded system pages; removing
  Bootstrap must not break their error, not-found, or reconnection behavior.
  Do not delete a file simply because it came from the template.
- Do not modify generated assets in `bin`, `obj`, or packages.

## 7. Explicitly verify the RenderMode

Correct HTML output does not prove that components are interactive.
Verify interactivity in the layout, menu, and providers, not just in a child page.

- In a **Blazor Web App**, apply or verify the strategy agreed in step 3.
- For global interactivity, configure the appropriate boundary at `Routes`
  and a consistent `HeadOutlet` in `App.razor`, where this model applies.
  Verify the service registrations and associated endpoints in `Program.cs`.
- Do not blindly add `@rendermode` to `MainLayout.razor`: its `Body` parameter
  is a `RenderFragment` that cannot cross an SSR/interactive serialization
  boundary. Keep the layout, navigation, and their providers within a compatible
  interactive scope.
- For WebAssembly or Auto, also check component placement in the client project,
  references, and the services required for prerendering.
- A **standalone WebAssembly** application is already interactive: do not
  impose `InteractiveServer` on it. For other models, respect their existing
  interactivity mechanism.
- Preserve pages that must remain static, particularly when authentication
  requires it. Do not systematically disable prerendering: do so only when
  there is an established and explained need.

## 8. Validate the actual result

Validate the edited application, not just the proposed code. First check the
deliverables against the inventory, then run the commands below. If a requirement
was already satisfied, verify it without making unnecessary changes.

### Acceptance checklist

- **Packages and integration:** correct references, imports, services, styles,
  scripts, and providers in every relevant project/runtime, with no duplicates.
  Rendering must follow step 7, not merely produce static HTML.
- **Layout and navigation:** `MainLayout.razor` uses `FluentLayout`, renders
  `@Body`, and includes `<NavMenu />`. `NavMenu.razor` uses `FluentNav` and
  `FluentNavItem`, with `height: 100%;` in `FluentNav`'s `Style` itself, preserving
  any other necessary declarations.
- **Project-wide UI:** every non-system page/component has been accounted for.
  Native UI with a documented Fluent equivalent is migrated; paragraphs and
  headings use `FluentText`; native `<table>` elements have no `class`, including
  one introduced through attribute splatting. Inspect authored Razor markup,
  not HTML generated by Fluent or literal code examples. The three system
  exceptions retain their behavior and are not converted to Fluent.
- **Assets:** no active Bootstrap or deleted-resource references remain.
  `wwwroot/app.css` still exists and is referenced, even if empty; required
  isolated styles, system-page styles, and other retained assets remain usable.

An unchanged original template or edits only to this prompt/MCP configuration
do not satisfy the migration. A successful build alone does not establish
project-wide conversion or interactivity.

### Execute checks and fix migration errors

1. Restore with the effective repository configuration and verify the resolved
   versions and sources. For Preview, confirm that the repository `nuget.config`
   exists and maps only the exact core package to Preview; all other packages
   follow the nuget.org policy and any explicitly approved exceptions. Pass
   that file with `--configfile`, then build with `--no-restore`.
2. Run `dotnet build` on the solution or projects covering the application,
   including any client project. Fix errors introduced by the migration
   and rebuild until it succeeds. Repeat restore only if dependency/configuration
   changes or a restore failure require it; do not reinstall unchanged packages.
3. Run relevant existing tests, if any, using the repository's tooling and
   reusing completed restore/build work where supported. Do not add a new
   testing infrastructure solely for this migration.
4. Start the application with the appropriate profile and verify that it
   responds. Using a browser if available, verify direct loading of an internal
   route, navigation, the active state, the mobile menu, and an actual existing
   interaction, such as the counter if present. Also check for missing assets
   and JavaScript or circuit errors. Verify that navigation fills its available
   desktop height without breaking mobile navigation or clipping menu items.
5. Recheck checklist items affected by any fixes and stop processes started
   solely for these checks.

If a validation step cannot be performed, specify exactly which one, why,
and what remains to be verified. Never report a successful build or test
without actually running it successfully.

## 9. Install the MCP server for the user's future work

By default, this is a separate convenience setup after the application migration
and validation. **The MCP server is not required for routine execution of this
prompt.** The exception is a targeted lookup to resolve an uncertain conversion
in step 5: use these setup instructions earlier if necessary, then resume the
migration. Do not repeat setup if it was already completed. Do not require a live
MCP connection merely to build or validate the application.

Perform the installation and configuration yourself where possible, rather
than merely giving commands to the user. Any MCP-only limitation must leave the
application migration result intact and be reported separately.

### Identify the IDE and execution environment

- Identify **VS Code**, **Visual Studio 2026**, or **another IDE**, its version,
  and the active AI assistant or agent harness from reliable session metadata.
  Do not infer the host solely from installed executables or `.vscode` or `.vs`
  folders. Ask the user if the target client is unclear before configuring it.
- For SSH, WSL, or container sessions, determine the environment in which the
  future MCP process will run before installing or registering it.

### Install the .NET tool from nuget.org only

- Check the current MCP installation prerequisites and existing SDKs and tools.
  Do not install additional prerequisites without approval.
- **The MCP package must come exclusively from nuget.org**, even when the
  component library uses Preview. Choose an appropriate version actually
  published on nuget.org, independently of the library's daily build number.
  Never search the Preview feed for an MCP version matching the core package.
- Check `dotnet tool list --global` and reuse an existing working installation.
  Do not update it merely to match the component-library version. If an update
  is genuinely needed, explain its impact on other projects and ask for approval.
- If missing, use the currently documented installation method. For the global
  .NET tool method, substitute actual values in:

  ```text
  dotnet tool install --global Microsoft.FluentUI.AspNetCore.McpServer --version <version-published-on-nuget.org> --configfile <nuget-org-only-config>
  ```

  Create the temporary configuration with only nuget.org and `<clear />` in
  `packageSources`; pass it explicitly to any install or update command and
  remove it afterward. `--add-source` alone does not exclude other feeds.
- Verify installation with the global tool list and resolve the documented
  launch command, currently `fluentui-mcp`, in the IDE's execution environment.
  Use the resolved executable path if needed instead of overwriting PATH.

### Register the installed command in the correct client

Follow the current MCP guide and client documentation. Prefer a supported
**user-level configuration**, reusable across projects, rather than copying
workspace examples into every repository:

- **VS Code:** resolve the active profile and harness's configuration location.
  Local sessions can use the user-profile `mcp.json`; Copilot harness sessions
  may use Copilot Global configuration. Do not guess a profile path or assume
  `.vscode/mcp.json` is appropriate for every session.
- **Visual Studio 2026:** use its documented user-level location, currently
  `%USERPROFILE%\.mcp.json`, rather than the solution-root workspace example.
- **Another IDE or client:** consult its official documentation for MCP support,
  configuration scope, schema, transport, and launch command. If no suitable
  personal configuration is supported, report the limitation and ask before
  creating a project-level alternative.

Merge only the Fluent UI entry, using the resolved command and required
arguments. Preserve unrelated servers, settings, supported comments, and
secrets. Reuse equivalent entries and obtain approval before replacing a
conflicting one. Validate against the client schema: `servers` and `mcpServers`
are not interchangeable.

### Leave activation for later use unless a lookup is needed now

Validate the saved configuration, but do not require a live MCP tool call,
tool discovery, an IDE restart, or a new chat session solely to complete this
convenience installation. If resolving a question in step 5, start the configured
server, discover its tools, and make the relevant documentation call; do not
substitute a plausible answer for an actual tool result.
Respect trust prompts and organization policies; do not enable blanket approvals.
Report the tool as installed and configured, not as connected unless a connection
has actually been verified. Explain any later activation steps and remind the
user to check the documented library/server compatibility when they use it.
Do not change the application's packages to accommodate this future tool.
If setup cannot be completed, report the exact pending action separately;
it must not invalidate an otherwise successful application migration.

## Final report

Respond briefly in English with:
- The selected feed, installed versions, and effective render mode.
- For Preview, the repository NuGet configuration file and the verified
  core-only Preview mapping.
- The detected IDE and client, MCP tool version, configuration location and scope,
  installation/configuration status, and any activation steps for future use.
  Mention any MCP documentation lookups used to resolve uncertain conversions.
  Distinguish application migration status from MCP setup status; a live MCP
  connection is not otherwise part of this prompt's acceptance criteria.
- Modified files and deleted assets, with relevant reasons.
- Confirmation that all non-system pages/components were reviewed and converted,
  that the three system exceptions were preserved, and that native tables have
  no class attributes.
- The validation commands actually executed and their results.
- Any limitations or remaining checks.

Claim application completion only when the acceptance checklist and executed
checks support it. Report MCP installation, optional lookup usage, and pending
activation separately; its presence, connection, and version are not general
prerequisites for the application migration.
