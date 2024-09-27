---
title: Migration General
route: /Migration/General
hidden: true
---

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
