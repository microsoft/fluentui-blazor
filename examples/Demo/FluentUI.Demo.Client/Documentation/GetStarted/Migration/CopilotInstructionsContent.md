---
title: GitHub Copilot custom instructions
route: /Migration/gh-copilot-custom-instructions
hidden: true
---

To streamline your migration process, we provide custom instructions for GitHub Copilot in the `copilot-instructions.md` file. These instructions help Copilot understand our codebase conventions, naming patterns, and migration requirements.

By importing these custom instructions into your GitHub Copilot settings, you'll get more accurate and context-aware suggestions specifically tailored to FluentUI Blazor migration tasks.

Example benefits:
- Migrate complete file following new namespacing and new parameters
- Renaming old names from V3-V4 to V5

> ⚠️ Note
>
> - This feature is currently in public preview and is subject to change.
> - Custom instructions are currently only supported for **Copilot Chat** in **VS Code** and **Visual Studio**.

The complete documentation is available here [Adding custom instructions for GitHub Copilot](https://docs.github.com/en/copilot/customizing-copilot/adding-custom-instructions-for-github-copilot)

Once your migration is done, you can remove the file or the part of the FluentUI migration.

#### How to enable custom instruction

1. In the root of your repository, create a file named `.github/copilot-instructions.md`.

   Create the `.github` directory if it does not already exist.

2.  Copy the content of the migration instruction inside the `copilot-instruction.md` file

    > Whitespace between instructions is ignored, so the instructions can be written as a single paragraph, each on a new line, or separated by blank lines for legibility.

#### Enable custom instruction in Visual Studio or Visual Studio Code. 

GitHub's team has a perfect step-by-step void to enable : [Enabling or disabling custom instructions](https://docs.github.com/en/copilot/customizing-copilot/adding-custom-instructions-for-github-copilot#enabling-or-disabling-custom-instructions).

# GitHub Copilot Custom Instructions for FluentUI Blazor Migration

The following content contains specialized instructions for GitHub Copilot to assist in migrating components to FluentUI Blazor. These instructions help Copilot understand our:

- Component architecture
- Code conventions
- Namespace organization
- Migration patterns
- TypeScript integration

## How to Use

Copy the content below and paste it into your GitHub Copilot custom instructions settings to enable context-aware assistance during migration tasks.

## Custom instructions' content

```
{{ INCLUDE FILE=copilot-instructions }}
```