# Copilot Instructions

## Project Guidelines

- In this repository, component script changes must be made in the corresponding .ts file, not generated .js files.

## Skills

- When writing, reviewing, or refactoring C# / .NET / Blazor code, or when authoring unit tests with xUnit and bUnit, load and follow the [csharp-naming-conventions](skills/csharp-naming-conventions/SKILL.md) skill. Read the `SKILL.md` file with the `read_file` tool before producing code so the conventions, layout rules, and testing guidelines are applied.

## Browser Automation Guidelines

- When browser automation initially fails in this workspace, retry Playwright actions instead of assuming the browser backend is unavailable.
