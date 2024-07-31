# Changes introduced in this version 5

The following changes have been included in version 5.
Some of these are coding changes,
component changes or Breaking Changes.

## Minor Changes

- ### Scoped Css Bundling

  The csproj contains `<DisableScopedCssBundling>true</DisableScopedCssBundling>` to prevent the bundling of scoped css files.

  Components won't contain the scoped css identifier and the `::deep` is now useless.
  The `...bundle.scp.css` file is generated from the **Components.Scripts** project and automatically included in the **Components** project.

## Breaking Changes

- ### FluentButton

  - ### FormAction
    The `Action` property has been renamed to `FormAction`.

  - ### FormEncType
    The `Enctype` property has been renamed to `FormEncType`.

  - ### FormMethod
    The `Method` property has been renamed to `FormMethod`.

  - ### FormNoValidate
    The `NoValidate` property has been renamed to `FormNoValidate`.

  - ### FormTarget
    The `Target` property has been renamed to `FormTarget`.

  - ### Appearance
      The `Appearance` property has been updated to use the `ButtonAppearance` enum
      instead of `Appearance`.

      |v3 & v4|v5|
      |---|---|
      |`Appearance.Neutral`    |`ButtonAppearance.Default`|
      |`Appearance.Accent`     |`ButtonAppearance.Primary`|
      |`Appearance.Lightweight`|`ButtonAppearance.Transparent`|
      |`Appearance.Outline`    |`ButtonAppearance.Outline`|
      |`Appearance.Stealth`    |`ButtonAppearance.Default`|
      |`Appearance.Filled`     |`ButtonAppearance.Default`|
      |                        |`ButtonAppearance.Subtle`|
      |                        |`ButtonAppearance.Transparent`|

  - ### CurrentValue
    The `CurrentValue` property has been removed. Use `Value` instead.
