# Localization

Localization allows the text in some components to be translated.

## Explanation

**Fluent UI Blazor** itself provides English language strings for texts found in.
To customize translations in **Fluent UI Blazor**, the developer can register a
custom `IFluentLocalizer` implementation as a service, and register this custom localizer
in the `Program.cs` file, during the service registration.

```csharp
builder.Services.AddFluentUIComponents(config => config.Localizer = new CustomFluentLocalizer());
```

## How to add a new resource string?

In this library, to add a new resource string, follow these steps:

1. Open the file **LanguageResource.resx** and add a new entry with the name `<prefix>_<item>`.
   Where `<prefix>` is the name of the component, and `<item>` is the name of the string.
   Example:
     - `MessageBox_ButtonYes` for the Yes button label, of the MessageBox component.
     - `FluentLayoutHamburger_Title` for the title of the Layout Hamburger component.

2. The `FluentComponentBase` class provides a `Localizer` property that can be used to access the
   resource strings. The `Localizer` property is an instance of `IFluentLocalizer`, which
   is registered in the service collection.
   You can use the `Localizer` property to access the resource strings in your component.
   The `Localization.LanguageResource` class contains the list of all resource string constants to use.

   Example:
   ```csharp
   protected override void OnInitialized()
   {
       Title = Localizer[Localization.LanguageResource.FluentLayoutHamburger_Title];
   }
   ```
