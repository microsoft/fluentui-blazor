---
title: Migration SplashScreen
route: /Migration/SplashScreen
hidden: true
---

### Splash screen service changes

In V5, splash screens are still displayed through the injected `IDialogService`.
Keep a `FluentDialogProvider` in your layout.

`ShowSplashScreenAsync` returns an `ISplashScreenInstance` as soon as the splash screen is displayed.
Use this instance to update the content and close the dialog after your startup work completes.

| V4 | V5 |
|----|----|
| `DialogParameters<SplashScreenContent>` | `SplashScreenOptions` for a custom component, or a content configuration callback for the default splash screen |
| `parameters.Content` | `options.SplashScreen` |
| `IDialogReference` | `ISplashScreenInstance` |
| `ShowSplashScreenAsync(parameters)` | `ShowSplashScreenAsync(content => { ... })` |
| `ShowSplashScreenAsync<T>(parameters)` | `ShowSplashScreenAsync<T>(options)` or `ShowSplashScreenAsync<T>(options => { ... })` |
| `ShowSplashScreenAsync(typeof(T), parameters)` | `ShowSplashScreenAsync(typeof(T), options)` |
| `splashScreen.UpdateLabels(...)` | `await splash.UpdateAsync(content => { ... })` |
| `DialogService.UpdateDialogAsync(dialog.Id, parameters)` | `await splash.UpdateAsync(content => { ... })` for content updates |
| `await dialog.CloseAsync()` | `await splash.CloseAsync()` |
| `await dialog.Result` | `await splash.Result` (still returns `DialogResult`) |
| Overloads accepting a receiver and result callback | Await `splash.Result`, or use `options.OnStateChange` for lifecycle notifications |

The synchronous `ShowSplashScreen` methods are removed. Use `ShowSplashScreenAsync` instead.

### Content changes

| V4 `SplashScreenContent` | V5 `SplashScreenContent` | Migration |
|--------------------------|--------------------------|-----------|
| `Title`, `SubTitle`, `LoadingText` | Same | No change |
| `Message` (`MarkupString?`) | Same | HTML content is still supported |
| `Logo` | `LogoUrl` | Rename the property |
| `FluentSplashScreen.LOGO` | No equivalent constant | Provide your own logo URL |
| `DisplayTime` | Removed | Close the instance explicitly when loading completes |
| `UpdateLabels(...)` | Removed | Use `ISplashScreenInstance.UpdateAsync(...)` to update content and re-render |
| No equivalent | `LogoMaxWidth` | Optional maximum logo width; defaults to `"500px"` |
| No equivalent | `LoadingPercentage` | Set a value from 0 to 100 to display a progress bar; `null` displays a spinner |

There is no automatic close timer in V5. Replace `DisplayTime` with your startup work
(or `Task.Delay` for a timed demonstration), followed by `await splash.CloseAsync()`.
Simply changing a content property does not request a re-render: use `UpdateAsync`.

### Default splash screen

The following example migrates the V4 demo's two loading steps.
Both versions use an injected `IDialogService DialogService`.

**V4**

```csharp
DialogParameters<SplashScreenContent> parameters = new()
{
    Content = new()
    {
        DisplayTime = 0,
        Title = "Core components",
        SubTitle = "Microsoft Fluent UI Blazor library",
        LoadingText = "Loading...",
        Message = (MarkupString)"some <i>extra</i> text <strong>here</strong>",
        Logo = FluentSplashScreen.LOGO
    }
};

IDialogReference dialog = await DialogService.ShowSplashScreenAsync(parameters);
var splashScreen = (SplashScreenContent)dialog.Instance.Content;

await Task.Delay(2000);
splashScreen.UpdateLabels(loadingText: "Second task...");
await Task.Delay(2000);

await dialog.CloseAsync();
DialogResult result = await dialog.Result;
```

**V5**

```csharp
ISplashScreenInstance splash = await DialogService.ShowSplashScreenAsync(content =>
{
    content.Title = "Core components";
    content.SubTitle = "Microsoft Fluent UI Blazor library";
    content.LoadingText = "Loading...";
    content.Message = (MarkupString)"some <i>extra</i> text <strong>here</strong>";
    content.LogoUrl = "/images/my-logo.png";
});

await Task.Delay(2000);
await splash.UpdateAsync(content => content.LoadingText = "Second task...");
await Task.Delay(2000);

await splash.CloseAsync();
DialogResult result = await splash.Result;
```

Replace the delays with your application's initialization tasks, and provide an existing image
at the configured logo URL (or omit `LogoUrl`).

### Custom splash screen

In V4, a custom component implemented `IDialogContentComponent<SplashScreenContent>` and
received a `Content` parameter. In V5, it can be a regular Razor component with a parameter
named **`SplashScreen`** of type `SplashScreenContent`. No dialog interface or base class is required.

**V4 - `CustomSplashScreen.razor` (simplified)**

```razor
@implements IDialogContentComponent<SplashScreenContent>

<FluentDialogHeader Visible="false" />
<FluentDialogBody>
    <img src="@Content.Logo" alt="" />
    <h3>@Content.Title</h3>
    <FluentProgress />
    <p>@Content.LoadingText</p>
</FluentDialogBody>
<FluentDialogFooter Visible="false" />

@code {
    [Parameter]
    public SplashScreenContent Content { get; set; } = default!;
}
```

**V5 - `CustomSplashScreen.razor`**

```razor
<FluentDialogBody>
    <FluentStack Orientation="Orientation.Vertical"
                 HorizontalAlignment="HorizontalAlignment.Center">
        <img src="@SplashScreen.LogoUrl" alt="" />
        <FluentText>@SplashScreen.Title</FluentText>
        <FluentSpinner />
        <FluentText>@SplashScreen.LoadingText</FluentText>
    </FluentStack>
</FluentDialogBody>

@code {
    [Parameter]
    public required SplashScreenContent SplashScreen { get; set; }
}
```

Remove the V4 `FluentDialogHeader` and `FluentDialogFooter` components.
The V5 header and footer are configured through dialog options; leave them unconfigured
for a splash screen without a header or action buttons.
If your V4 custom component closed itself in `OnAfterRenderAsync` using `Content.DisplayTime`
and a cascading `FluentDialog`, move that timing and closing logic into the calling method:

```csharp
ISplashScreenInstance splash = await DialogService.ShowSplashScreenAsync<CustomSplashScreen>(options =>
{
    options.Width = "500px";
    options.Height = "300px";
    options.SplashScreen.Title = "Water drinking 101";
    options.SplashScreen.LoadingText = "Filling the re-useable bottles...";
    options.SplashScreen.Message = (MarkupString)"Don't drink <strong>too</strong> much water!";
    options.SplashScreen.LogoUrl = "/images/my-logo.png";
});

for (int i = 0; i < 5; i++)
{
    await Task.Delay(1000);
    await splash.UpdateAsync(content =>
        content.LoadingText = $"Filling the re-useable bottles... {i + 1}");
}

await Task.Delay(2000);
await splash.CloseAsync();
DialogResult result = await splash.Result;
```

You can also create a `SplashScreenOptions` instance and pass it to
`ShowSplashScreenAsync<CustomSplashScreen>(options)`.
The `SplashScreen` property is initialized by the options object: configure its members rather
than assigning a new `SplashScreenContent` instance.

The service supplies `options.SplashScreen` to the component and re-renders it after each
`UpdateAsync` call. Use `options.Parameters` for any additional component parameters;
the `"SplashScreen"` key is managed by the service.

### Dialog options and dismissal

`SplashScreenOptions` inherits `DialogOptions`. Set `Width`, `Height`, or `Modal` there
when displaying a custom splash screen. V4's `PreventDismissOnOverlayClick` option is removed.
V5 splash screens prevent dismissal with the Escape key by default
(`PreventDismissOnEscape = true`); set it explicitly to `false` if Escape should close the dialog.

For the shared dialog option changes, see [FluentDialog migration](/Migration/Dialog).
For runnable V5 examples, see [Splash screen](/SplashScreen).
