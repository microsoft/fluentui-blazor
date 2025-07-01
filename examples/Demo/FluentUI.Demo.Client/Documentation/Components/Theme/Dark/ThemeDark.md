---
title: Theme / Light-Dark
route: /theme/dark
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
}
```

> [!NOTE] Soon you will be able to use the Blazor `FluentDesignTheme` component to manage themes in a more declarative way.

#  body data-theme

The `<body data-theme>` attribute is used to specify the current theme of the application.  
When the dark theme is applied, this attribute will be set to `data-theme="dark"`.  
When the light theme is applied, this attribute will be missing.  

For example, open the browser DevTools and switch between light and dark themes using the top-right button.
You will see the `data-theme` attribute change accordingly.

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

**Client side**

A JavaScript `themeChanged` event is triggered each time the `data-theme` attribute changes.

```html
<script>
    document.body.addEventListener('themeChanged', function (e) {
        console.log('Theme changed: isDark=', e.detail.isDark);
    });
</script>
```