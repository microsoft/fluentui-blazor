# Copilot Instructions

## Project Guidelines

- In this repository, component script changes must be made in the corresponding .ts file, not generated .js files.
- Before debugging UI issues with Playwright, verify the Playwright connection first; if it is unavailable, close the browser or tab and retry before proceeding.
- In the fluentui-blazor charts workspace, the `.txt` files in `wwwroot/sources/` are auto-generated. Do not manually create them.
- When adding enum-backed component state in this repo, use the existing ToAttributeValue extension for emitted attribute strings, place enums under `src/Core/Enums`, and follow existing enum implementation style (XML docs + Description attributes).

## Skills

- When writing, reviewing, or refactoring C# / .NET / Blazor code, or when authoring unit tests with xUnit and bUnit, load and follow the [csharp-naming-conventions](skills/csharp-naming-conventions/SKILL.md) skill. Read the `SKILL.md` file with the `read_file` tool before producing code so the conventions, layout rules, and testing guidelines are applied.

## Browser Automation Guidelines

- When browser automation initially fails in this workspace, retry Playwright actions instead of assuming the browser backend is unavailable.

