---
title: SplashScreen
route: /SplashScreen
icon: ArrowClockwise
---

# SplashScreen

A **SplashScreen** is a lightweight loading dialog displayed while your application starts up, loads data,
or performs a background task. It follows the v5 dialog model and can be shown through the `DialogService`
using the standard `FluentSplashScreen` component or a custom content component.

Content is provided through the `SplashScreenContent` object, which can be created and passed to the dialog service

The splash screen closes automatically after `DisplayTime` (default = 4000ms) elapses, and the `SplashScreenContent` object can
also be updated while it is visible through `UpdateLabels(...)`.

## Default splash screen

{{ DialogSplashScreenDefault Files=Code:DialogSplashScreenDefault.razor }}

## Custom splash screen

A custom splash screen can be created by implementing a component and passing its state through
`DialogOptions.Parameters`. This uses the normal dialog pipeline instead of `SplashScreenContent`.
In this example the splash screen is displayed for 7 seconds.

{{ DialogSplashScreenCustom Files=Code:DialogSplashScreenCustom.razor;CustomSplashScreen:CustomSplashScreen.razor;CSS:CustomSplashScreen.razor.css;CustomSplashScreenState:CustomSplashScreenState.cs }}

## Notes

- `DisplayTime` is expressed in milliseconds.
- Set `DisplayTime = 0` when the dialog should stay open until your own task completes.
- `UpdateLabels(...)` lets you change the loading text or message without closing the dialog.
- The default splash screen has no logo. Set `Logo` in `SplashScreenContent` when you want to show a branded or custom asset.

## API Documentation

{{ API Type=SplashScreenContent Properties=All }}

