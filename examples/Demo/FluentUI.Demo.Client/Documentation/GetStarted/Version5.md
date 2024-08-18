---
title: Migration to v5
route: /MigrationV5
---

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

- ### ToAttributeValue()

  This extension method was updated to return
  1. The `[Description]` attribute value **without changing the case** (upper/lower case)
  2. The enumeration value **converted to lowercase** string, if the '[Description]' attribute is not found.

  Examples:
  - `[Description("MyValue")]` => `MyValue`
  - `[Description("my-value")]` => `my-value`
  - `enum Color { Default, Primary }` => `default`, `primary`

  

- ### JavaScript Interop

  We have migrated all **JavaScript** files to **TypeScript**.
  These files are
    - Either global to all components, in which case they are included in the **Components.Scripts** project
    - Or attached to a component and collocated under the component.

  The `.ts` files are compiled into `.js` and included in the application bundle or in the library itself.

  > ⚠️ The functions are included in the global namespace `Microsoft.FluentUI.Blazor.[Component]`.

  To import a JavaScript file in a component, call the `FluentComponentBase.ImportJavaScriptModuleAsync` method.

  Example

  ```ts
  export namespace Microsoft.FluentUI.Blazor.Button {
    /**
     * Returns a string "Hello World"
     * @returns
     */
    export function MyFunction(): string {
      return "Hello World";
    }
  }
  ```

  ```csharp
  public partial class FluentButton : FluentComponentBase
  {
      private const string JAVASCRIPT_FILE = JAVASCRIPT_ROOT + "Button/FluentButton.razor.js";

      protected override async Task OnAfterRenderAsync(bool firstRender)
      {
          if (firstRender)
          {
              // Import the JavaScript module
              var jsModule = await ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);

              // Call a function from the JavaScript module
              await jsModule.InvokeAsync<string>("Microsoft.FluentUI.Blazor.Button.MyFunction");
          }
      }
  }
  ```

 - ### Unit Test and Code Coverage

   We have added unit tests for all components and services.
   The code coverage is now at 99% for the **Core** project.

   You will find more details on how to create unit tests in the `/docs/unit-tests.md` file.

## Color Enumeration

  - ### Renamed values 🔃
    `Default` is equivalent of previous `Neutral` and `Primary` is equivalent of previous `Accent` values.

  - ### Removed values💥
    `Neutral`, `Accent`, `Fill`, `FillInverse` values have been flagged as `Obsolete` and will be removed in the next version.

    |v3 & v4|v5|
    |---|---|
    |`Color.Neutral`      (--neutral-foreground-rest)     | `Color.Default`       (--colorNeutralForeground1)        |
    |`Color.Accent`       (--accent-fill-rest)            | `Color.Primary`       (--colorBrandForeground1)          |
    |`Color.Fill`         (--neutral-fill-rest)           | `Color.Default`       (--colorNeutralForeground1)        |
    |`Color.FillInverse`  (--neutral-fill-inverse-rest)   | `Color.Lightweight`   (--colorNeutralForegroundInverted) |

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

## FluentGridItem

  - ### Renamed properties 🔃
    These properties have been renamed to comply with the Blazor naming convention (Pascal case):
    - `xs`, `sm`, `md`, `lg`, `xl`, `xxl` properties have been renamed to
    - `Xs`, `Sm`, `Md`, `Lg`, `Xl`, `Xxl`.

    If you don't rename them correctly, you'll probably get a compilation error like this one:
    ```   
    InvalidOperationException: Unable to set property 'sm' on object of type 'Microsoft.FluentUI.AspNetCore.Components.FluentGridItem'.
    The error was: Unable to cast object of type 'System.String' to type 'System.Nullable`1[System.Int32]'.
    ```

## FluentLayout and FluentMainLayout

- ### New components

  The `FluentLayout` component has been introduced to replace the `FluentLayout` and `FluentMainLayout` components.
  This new component is based on the CSS `grid` element to simplify the usage and customization of the layout
  (including on mobile device).

   ```xml
   <FluentLayout>
     <FluentLayoutItem Area="LayoutArea.Header">Header</FluentLayoutItem>
     <FluentLayoutItem Area="LayoutArea.Menu">Menu</FluentLayoutItem>
     <FluentLayoutItem Area="LayoutArea.Content">Content</FluentLayoutItem>
     <FluentLayoutItem Area="LayoutArea.Aside">Aside</FluentLayoutItem>
     <FluentLayoutItem Area="LayoutArea.Footer">Footer</FluentLayoutItem>
   </FluentLayout>
   ```

- ### Removed components💥
  The `FluentHeader`, `FluentBodyContent`, `FluentFooter`, `FluentMainLayout` components have been removed.

  Use the `FluentLayoutItem Area="..."` component instead.
