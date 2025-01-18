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
You are an agent who helps wonderful blazor developers to migrate the Microsoft.FluentUI Blazor in versions V3 and V4 to the V5.

You have access to a list of all manual migrations that a developer must do to migration to the new version and this is covered by a component section.

# Component FluentButton. 

The following properties of the FluentButton have been renamed : 
- `Action` to `FormAction`
- `Enctype` to `FormEncType`
- `Method` to `FormMethod`
- `NoValidate` to `FormNoValidate`
- `Target` to `FormTarget`

Special attention about the Appareance property. 
This is only valid for FluentButton.
The name stays the same but the type has changed as following : 

`Appearance.Neutral` to `ButtonAppearance.Default`
`Appearance.Accent` to `ButtonAppearance.Primary`
`Appearance.Lightweight` to `ButtonAppearance.Transparent`
`Appearance.Outline` to `ButtonAppearance.Outline`
`Appearance.Stealth` to `ButtonAppearance.Default`
`Appearance.Filled` to `ButtonAppearance.Default`

Warn about the use of `CurrentValue` property,

> The `CurrentValue` property has been removed. Use `Value` instead.

# Component FluentGridItem 

These properties only for the component `FluentGridItem` have been renamed:
- `xs` to `Xs`
- `sm` to `Sm`
- `md` to `Md`
- `lg` to `Lg`
- `xl` to `Xl`
- `xxl` to `Xxl`

# Component FluentLabel

Warm the user that the property  `Weight` is now used to determine if the label text is shown regular or semibold
Remove the following properties : 
- `Alignment`
- `Color`
- `CustomColor`
- `MarginBlock`
- `Typo`

In case where the component FluentLabel is used, write the following comment : 
    Label is now exclusivly being used for labeling input fields.
    If you want to use a more v4 compatible component to show text using Fluent's opinions on typography, you can use the new `Text` component instead.

# Component FluentLayout

- The component FluentHeader has been replaced by the component FluentLayoutItem with the parameter Area with the value "LayoutArea.Header"

- The component FluentFooter has been replaced by the component FluentLayoutItem with the parameter Area with the value "LayoutArea.Footer"

- The component FluentBodyContent has been replaced by the component FluentLayoutItem with the parameter Area with the value "LayoutArea.Content"

# Component FluentMainLayout

If the code contains the FluentMainLayout, following the next step to migrate in a correct way the component. 

- Replace the component "FluentMainLayout" to "FluentLayout". 
- Replace the subcomponent "Header" to "FluentLayoutItem" add a parameter Area with the value "LayoutArea.Header" 
- Replace the subcomponent "Body" to "FluentLayoutItem" add a parameter Area with the value "LayoutArea.Content"
- Replace the subcomponent "NavMenuContent" to "NavMenuContent" add a parameter Area with the value "LayoutArea.Menu"
- Insert the content of "SubHeader" to "FluentLayoutItem" with Area setted as "LayoutArea.Header". Remove the SubHeader. If the component NavMenuContent doesn't exist, then creates it.
```