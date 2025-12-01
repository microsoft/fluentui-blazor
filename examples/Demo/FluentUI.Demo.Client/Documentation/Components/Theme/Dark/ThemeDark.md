---
title: Light-Dark
route: /Theme/Dark
---

# Light-Dark Themes

FluentUI provides a **dark theme** that can be applied to your application. 
This theme is designed to provide a visually appealing and comfortable user experience in low-light environments.

You can easily switch between light and dark themes using the provided JavaScript functions: 
`Blazor.theme.setDarkTheme` and `Blazor.theme.setLightTheme`.

## Example

```csharp
[Inject]
public required IJSRuntime JSRuntime { get; set; }

private bool DarkTheme { get; set; };

public async Task SwitchThemeAsync()
{
    DarkTheme = !DarkTheme;
    await JSRuntime.InvokeVoidAsync(DarkTheme ? "Blazor.theme.setDarkTheme" : "Blazor.theme.setLightTheme");

    // Or simply toggle the theme
    // await JSRuntime.InvokeVoidAsync("Blazor.theme.switchTheme");
}
```

> [!NOTE] Soon you will be able to use the Blazor `FluentDesignTheme` component to manage themes in a more declarative way.

## body data-theme

The `<body data-theme>` attribute is used to specify the current theme of the application.  
When the dark theme is applied, this attribute will be set to `data-theme="dark"`.  
When the light theme is applied, this attribute will be missing.  

For example, open the browser DevTools and switch between light and dark themes using the top-right button.
You will see the `data-theme` attribute change accordingly.

**Force a theme**

The theme is automatically managed by Fluent UI Blazor when the application is first loaded (Startup).  
You can force a specific theme by directly adding the attribute `data-theme="dark"` or `data-theme="light"`
in the `<body>` tag of your `index.html` or `_Host.cshtml` file.

**Style**

This attribute lets you easily apply specific CSS styles based on the current theme.

```css
body {
  background-color: #ffffff;
  color: #000000;
}

body[data-theme="dark"] {
  background-color: #121212;
  color: #ffffff;
}
```

## themeChanged event

A JavaScript `themeChanged` event is triggered each time the `data-theme` attribute changes.

```html
<script>
    document.body.addEventListener('themeChanged', function (e) {
        console.log('Theme changed: isDark=', e.detail.isDark);
    });
</script>
```

The theme does not update by default when the user changes the system or browser theme, and it is not saved across sessions.
You can listen to the `themeChanged` event to implement your own logic, such as saving the user's preference in local storage or cookies.
Or to automatically switch themes based on system preferences.

```html
<script>
    document.body.addEventListener('themeChanged', function (e) {
        if (e.detail.isDark) {
            // Apply dark theme
            Blazor.theme.setDarkTheme();
        } else {
            // Apply light theme
            Blazor.theme.setLightTheme();
        }        
    });
</script>
```
