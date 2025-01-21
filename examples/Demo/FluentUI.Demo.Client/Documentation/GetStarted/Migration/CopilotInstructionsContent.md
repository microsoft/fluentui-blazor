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

### How to enable custom instruction

1. In the root of your repository, create a file named `.github/copilot-instructions.md`.
   Create the `.github` directory if it does not already exist.

2. Click on the following link to view and copy the content of this **migration instruction**
   inside the `copilot-instructions.md` file.

   📥 <a href="/copilot-instructions.md" target="_blank">copilot-instructions.md</a>

3. In your existing project, after adding the new FluentUI-Blazor v5,
   you can start using the custom instructions to help you migrate your code.

   Example of Copilot instructions: "Can you migrate this file to the new FluentUI-Blazor v5?

### Enable custom instruction in Visual Studio or Visual Studio Code. 

GitHub's team has a perfect step-by-step void to enable : [Enabling or disabling custom instructions](https://docs.github.com/en/copilot/customizing-copilot/adding-custom-instructions-for-github-copilot#enabling-or-disabling-custom-instructions).
