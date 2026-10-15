---
title: Splash screen
route: /SplashScreen
icon: ArrowClockwiseDashes
---

# Splash screen

A **Splash screen** is a dialog displayed for a few seconds (for example while the application is loading),
whose content is updated and closed by the calling method.

For a splash screen, use `ShowSplashScreenAsync`: it returns an `ISplashScreenInstance` as soon as the dialog is displayed.

 - `UpdateAsync` updates the five splash screen content properties and re-renders the dialog content.
 - `CloseAsync` closes the dialog.

By default, the splash screen cannot be closed with the **Escape** key (`PreventDismissOnEscape = true`).





The dialog component receives the content through a single `SplashScreen` parameter:

```razor
<FluentDialogBody>
    @SplashScreen.Message
</FluentDialogBody>

@code {
    [Parameter]
    public required SplashScreenContent SplashScreen { get; set; }
}
```

Use this parameter to access `Title`, `SubTitle`, `Logo`, `LoadingText`, and `Message`.
The service passes the `options.SplashScreen` instance to the component and re-renders it after each `UpdateAsync` call.
Additional component parameters can still be configured through `Parameters`; the `SplashScreen` key is managed by the service.

```csharp
var splash = await DialogService.ShowSplashScreenAsync<SplashScreenDialog>(options =>
{
    options.SplashScreen.Message = new MarkupString("Loading...");
});

await Task.Delay(2000);
await splash.UpdateAsync(options => options.Message = new MarkupString("Hello World"));

await Task.Delay(2000);
await splash.CloseAsync();
```

{{ SplashScreenDefault Files=Code:SplashScreenDefault.razor;SplashScreenDialog:SplashScreenDialog.razor }}

## API SplashScreenInstance
