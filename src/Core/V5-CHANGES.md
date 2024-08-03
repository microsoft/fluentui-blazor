# Changes introduced in this version 5

The following changes have been included in version 5.
Some of these are coding changes,
component changes (flagged with 🔃) or Breaking Changes (flagged with 💥).

## General

- ### Scoped Css Bundling

  The csproj contains `<DisableScopedCssBundling>true</DisableScopedCssBundling>`
  and `<ScopedCssEnabled>false</ScopedCssEnabled>` to prevent the bundling of scoped css files.

  Components won't contain the scoped css identifier and the `::deep` is now useless.
  The `...bundle.scp.css` file is generated from the **Components.Scripts** project and automatically included in the **Components** project.

## FluentButton

  - ### New properties
    `Size`,  `Shape`, `DisabledFocusable` are new properties.

  - ### Renamed properties 🔃
    The `Action` property has been renamed to `FormAction`.

    The `Enctype` property has been renamed to `FormEncType`.

    The `Method` property has been renamed to `FormMethod`.

    The `NoValidate` property has been renamed to `FormNoValidate`.

    The `Target` property has been renamed to `FormTarget`.

  - ### Removed properties💥
    The `CurrentValue` property has been removed. Use `Value` instead.

  - ### Appearance 💥
      The `Appearance` property has been updated to use the `ButtonAppearance` enum
      instead of `Appearance` enum.

      `ButtonAppearance` enum has the following values:
      - `Default`
      - `Primary`
      - `Outline`
      - `Subtle`
      - `Transparent`

    **Migration from v4 to v5:**

	You can use the `ToButtonAppearance()` method to convert the `Appearance` property to the `ButtonAppearance` enum.
	```csharp	
	@using Microsoft.FluentUI.AspNetCore.Components.Migration

	<FluentButton Appearance="Appearance.Accent.ToButtonAppearance()">Click</FluentButton>
	//                                          ^^^^^^^^^^^^^^^^^^^^
	```

      |v3 & v4|v5|
      |---|---|
      |`Appearance.Neutral`    |`ButtonAppearance.Default`|
      |`Appearance.Accent`     |`ButtonAppearance.Primary`|
      |`Appearance.Lightweight`|`ButtonAppearance.Transparent`|
      |`Appearance.Outline`    |`ButtonAppearance.Outline`|
      |`Appearance.Stealth`    |`ButtonAppearance.Default`|
      |`Appearance.Filled`     |`ButtonAppearance.Default`|
