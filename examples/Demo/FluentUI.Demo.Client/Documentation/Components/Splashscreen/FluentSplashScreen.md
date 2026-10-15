---
title: Splash screen
route: /SplashScreen
icon: ArrowClockwiseDashes
---

# Splash screen

A **Splash screen** is a dialog displayed for a few seconds (for example while the application is loading),
whose content is updated and closed by the calling method.

It uses the `DialogService` to display the dialog.

`ShowDialogAsync` waits for the dialog to be closed before returning a `DialogResult`.
For a splash screen (or any dialog that must be driven by the calling method),
use `ShowSplashScreenAsync`: it returns an `ISplashScreenInstance` as soon as the dialog is displayed.

 - `UpdateAsync` updates the dialog options (including `Parameters`) and re-renders the dialog content.
 - `CloseAsync` closes the dialog.

By default, the splash screen cannot be closed with the **Escape** key (`PreventDismissOnEscape = true`).

```csharp
var splash = await DialogService.ShowSplashScreenAsync<SplashScreenDialog>(options =>
{
    options.Parameters.Add(nameof(SplashScreenDialog.Message), "Loading...");
});

await Task.Delay(2000);
await splash.UpdateAsync(options => options.Parameters[nameof(SplashScreenDialog.Message)] = "Hello World");

await Task.Delay(2000);
await splash.CloseAsync();
```

{{ SplashScreenDefault Files=Code:SplashScreenDefault.razor;SplashScreenDialog:SplashScreenDialog.razor }}

## API SplashScreenInstance

{{ API Type=SplashScreenInstance }}
