---
name: Fluent UI Blazor v5 — Usage Guide
description: >
  Provides accurate coding patterns for building Blazor applications with the
  Microsoft.FluentUI.AspNetCore.Components v5 NuGet package. Covers service
  registration, theming (CSS-based, no FluentDesignTheme), layout, navigation,
  dialogs, data grid, forms, icons, and common pitfalls.
---

# Fluent UI Blazor v5 — Usage Skill

This skill helps AI assistants generate **correct, idiomatic code** for
applications that consume the `Microsoft.FluentUI.AspNetCore.Components`
**v5** NuGet package. v5 targets **.NET 9+** and wraps
`@fluentui/web-components` v3.

> **Critical**: v5 is a major rewrite. Many v4 APIs are removed or renamed.
> Always verify patterns against this skill before answering.

---

## Quick Setup Checklist

```csharp
// Program.cs
builder.Services.AddFluentUIComponents();
```

```razor
@* App.razor / MainLayout.razor — add providers once at root *@
<FluentProviders />
```

```html
<!-- index.html / _Host.cshtml — required CSS & JS -->
<link href="_content/Microsoft.FluentUI.AspNetCore.Components/css/default-fuib.css"
      rel="stylesheet" />
```

The `<FluentProviders />` component replaces the need to manually add
`<FluentDialogProvider />`, `<FluentTooltipProvider />`, and
`<FluentKeyCodeProvider />` separately.

---

## Key v5 Differences from v4

| Area | v4 | v5 |
|---|---|---|
| Target framework | net8.0 | net9.0 / net10.0 |
| Theming | `<FluentDesignTheme>` component | CSS custom properties via JS (`setTheme`), `body[data-theme]` attribute |
| Toast / MessageBar | `IToastService`, `IMessageService` | **Removed** — use `FluentToast` component directly or custom solution |
| Navigation | `FluentNavMenu`, `FluentNavLink`, `FluentNavGroup` | `FluentNav`, `FluentNavItem`, `FluentNavCategory`, `FluentNavSectionHeader` |
| Dialog API | `IDialogContentComponent<T>`, complex reference | Simplified: `IDialogService.ShowDialogAsync<TDialog>(DialogOptions)` |
| List binding | `FluentListBase<TOption>`, `SelectedOptions` | `FluentListBase<TOption, TValue>`, `SelectedItems` |
| Progress | `FluentProgress` | `FluentProgressBar` (`FluentProgress` = obsolete shim) |
| Appearance enum | `Appearance.Accent`, `.Stealth`, etc. | `ButtonAppearance.Primary`, `.Transparent`, etc. |
| Switch | `CheckedMessage`/`UncheckedMessage` child content | Removed — label only via `Label` parameter |
| Component base | Property injection | Constructor injection of `LibraryConfiguration` |
| Margin / Padding | Not on components | Available on **every** component via `Margin` and `Padding` parameters |
| Icons packages | Separate NuGet packages | Bundled (core icons), with optional extended packages |

---

## Theming

v5 replaces `<FluentDesignTheme>` with a **JavaScript + CSS custom property** model.

### Apply a theme

```razor
@inject IJSRuntime JSRuntime

<FluentButton OnClick="@SwitchTheme">Toggle theme</FluentButton>

@code {
    private async Task SwitchTheme()
    {
        await JSRuntime.InvokeVoidAsync("Blazor.theme.switchTheme");
    }
}
```

### Detect current mode

```javascript
// body[data-theme="dark"] → dark mode
// body without data-theme  → light mode
Blazor.theme.isDarkMode()   // returns boolean
Blazor.theme.isSystemDark() // returns boolean
```

### Conditional rendering by theme

```razor
<FluentIcon Class="hidden-if-light"
            Value="@(new Icons.Filled.Size20.WeatherSunny())" />
<FluentIcon Class="hidden-if-dark"
            Value="@(new Icons.Filled.Size20.WeatherMoon())" />
```

### CSS variables

All Fluent design tokens are available as CSS custom properties:
`var(--colorBrandBackground)`, `var(--colorNeutralForeground1)`,
`var(--fontSizeBase300)`, `var(--borderRadiusMedium)`, etc.

C# constants are available via `StylesVariables` and `SystemColors`:

```csharp
// In component styles
style="@($"background: {SystemColors.Neutral.Background1}")"
// Equivalent to: background: var(--colorNeutralBackground1);

style="@($"border-radius: {StylesVariables.Borders.Radius.Medium}")"
// Equivalent to: border-radius: var(--borderRadiusMedium);
```

---

## Layout & Navigation

### FluentLayout

```razor
<FluentLayout>
    <FluentLayoutItem Area="LayoutArea.Header">
        <FluentLayoutHamburger />
        <span>My App</span>
    </FluentLayoutItem>

    <FluentLayoutItem Area="LayoutArea.Menu">
        <FluentNav>
            <FluentNavItem Href="/" Icon="@(new Icons.Regular.Size20.Home())">
                Home
            </FluentNavItem>
            <FluentNavCategory Label="Settings" Icon="@(new Icons.Regular.Size20.Settings())">
                <FluentNavItem Href="/settings/profile">Profile</FluentNavItem>
                <FluentNavItem Href="/settings/account">Account</FluentNavItem>
            </FluentNavCategory>
            <FluentNavSectionHeader Label="Admin" />
            <FluentNavItem Href="/admin">Dashboard</FluentNavItem>
        </FluentNav>
    </FluentLayoutItem>

    <FluentLayoutItem Area="LayoutArea.Content">
        @Body
    </FluentLayoutItem>

    <FluentLayoutItem Area="LayoutArea.Footer">
        <span>&copy; 2025</span>
    </FluentLayoutItem>
</FluentLayout>
```

> **Common mistake**: Using `FluentNavMenu` / `FluentNavLink` / `FluentNavGroup` (v4 names). These do **not** exist in v5.

---

## Dialogs

### Show a simple dialog

```csharp
@inject IDialogService DialogService

private async Task OpenDialog()
{
    var options = new DialogOptions
    {
        Header = { Title = "Confirm action" },
        Size = DialogSize.Medium,
        Modal = true,
    };

    var result = await DialogService.ShowDialogAsync<ConfirmDialog>(options);
    if (!result.Cancelled)
    {
        // result.Value contains the returned data
    }
}
```

### Dialog component

```razor
@* ConfirmDialog.razor *@
<p>Are you sure you want to proceed?</p>
<FluentButton Appearance="ButtonAppearance.Primary"
              OnClick="@Confirm">Yes</FluentButton>
<FluentButton OnClick="@Cancel">No</FluentButton>

@code {
    [CascadingParameter]
    public IDialogInstance Dialog { get; set; } = default!;

    private async Task Confirm()
        => await Dialog.CloseAsync(DialogResult.Ok(true));

    private async Task Cancel()
        => await Dialog.CloseAsync(DialogResult.Cancel());
}
```

### Show a drawer

```csharp
var result = await DialogService.ShowDrawerAsync<DrawerContent>(options =>
{
    options.Header.Title = "Settings";
    options.Alignment = DialogAlignment.End;
    options.Size = DialogSize.Small;
});
```

### Pass parameters to the dialog component

```csharp
var options = new DialogOptions
{
    Header = { Title = "Edit item" },
    Parameters = new Dictionary<string, object?>
    {
        ["ItemId"] = selectedItemId,
        ["ItemName"] = selectedItemName,
    },
};
await DialogService.ShowDialogAsync<EditDialog>(options);
```

> **Common mistake**: Implementing `IDialogContentComponent<T>` (v4 pattern). In v5, dialog components are plain Razor components that receive `IDialogInstance` as a cascading parameter.

---

## Data Grid

```razor
<FluentDataGrid Items="@people" Pagination="@pagination">
    <PropertyColumn Property="@(p => p.Name)" Sortable="true" />
    <PropertyColumn Property="@(p => p.Email)" />
    <TemplateColumn Title="Actions">
        <FluentButton Size="ButtonSize.Small"
                      OnClick="@(() => Edit(context))">
            Edit
        </FluentButton>
    </TemplateColumn>
</FluentDataGrid>

<FluentPaginator State="@pagination" />

@code {
    private IQueryable<Person> people = GetPeople().AsQueryable();
    private PaginationState pagination = new() { ItemsPerPage = 10 };
}
```

### Remote data with ItemsProvider

```csharp
<FluentDataGrid ItemsProvider="@dataProvider" Virtualize="true" ItemSize="46">
    <PropertyColumn Property="@(p => p.Name)" Sortable="true" />
</FluentDataGrid>

@code {
    private GridItemsProvider<Person> dataProvider = default!;

    protected override void OnInitialized()
    {
        dataProvider = async request =>
        {
            var result = await Http.GetFromJsonAsync<PagedResult<Person>>(
                $"api/people?start={request.StartIndex}&count={request.Count}");
            return GridItemsProviderResult.From(result!.Items, result.TotalCount);
        };
    }
}
```

See [DATAGRID.md](references/DATAGRID.md) for advanced patterns (EF adapter,
OData, column options, resize, custom sort).

---

## Forms & Input Components

v5 input components extend `InputBase<TValue>` and implement `IFluentField`,
providing built-in form validation integration.

```razor
<EditForm Model="@model" OnValidSubmit="@Submit">
    <DataAnnotationsValidator />

    <FluentTextField @bind-Value="model.Name"
                     Label="Name"
                     Required="true" />

    <FluentNumberField @bind-Value="model.Age"
                       Label="Age"
                       Min="0" Max="150" />

    <FluentSelect TOption="string"
                  TValue="string"
                  Items="@countries"
                  @bind-Value="model.Country"
                  OptionText="@(c => c)"
                  OptionValue="@(c => c)"
                  Label="Country" />

    <FluentCheckbox @bind-Value="model.AcceptTerms"
                    Label="I accept the terms" />

    <FluentButton Type="ButtonType.Submit"
                  Appearance="ButtonAppearance.Primary">
        Submit
    </FluentButton>
</EditForm>
```

### List components — two type parameters

All list-based components (`FluentSelect`, `FluentCombobox`, `FluentListbox`)
now take **two** type parameters: `TOption` (the item type) and `TValue` (the
bound value type).

```razor
<FluentSelect TOption="Country"
              TValue="int"
              Items="@countries"
              @bind-Value="model.CountryId"
              OptionText="@(c => c.Name)"
              OptionValue="@(c => c.Id)"
              Label="Country" />

@* Multiple selection *@
<FluentListbox TOption="string"
               TValue="string"
               Items="@tags"
               Multiple="true"
               SelectedItems="@selectedTags"
               SelectedItemsChanged="@(items => selectedTags = items)"
               OptionText="@(t => t)"
               OptionValue="@(t => t)" />
```

> **Common mistake**: Using `SelectedOptions` or `SelectedOptionsChanged` (v4 names). In v5, use `SelectedItems` / `SelectedItemsChanged`.

---

## Icons

Icons are instantiated as objects and passed via `Value`, `IconStart`, or
`IconEnd` parameters:

```razor
@* Standalone icon *@
<FluentIcon Value="@(new Icons.Regular.Size24.Add())" />

@* Icon with color *@
<FluentIcon Value="@(new Icons.Filled.Size20.Heart().WithColor("red"))" />

@* Icon in a button *@
<FluentButton IconStart="@(new Icons.Regular.Size20.Add())"
              Appearance="ButtonAppearance.Primary">
    Add item
</FluentButton>

@* Icon from image URL *@
<FluentIcon Value="@Icon.FromImageUrl("/images/logo.png")" />
```

Icon naming: `Icons.{Variant}.{Size}.{Name}`
- Variants: `Regular`, `Filled`
- Sizes: `Size16`, `Size20`, `Size24`, `Size28`, `Size32`, `Size48`

---

## Margin & Padding (NEW in v5)

Every component supports `Margin` and `Padding` parameters using a
spacing-utility syntax:

```razor
<FluentCard Margin="Margin.All4" Padding="Padding.Horizontal3 Padding.Vertical2">
    Content with spacing
</FluentCard>
```

---

## Common Pitfalls

1. **Missing `<FluentProviders />`** — Dialogs, tooltips, and keyboard shortcuts
   will silently fail without this component in your layout.
2. **Using v4 component names** — `FluentNavMenu` → `FluentNav`,
   `FluentProgress` → `FluentProgressBar`, `FluentDesignTheme` → removed.
3. **Using `IToastService`** — Toast service does not exist in v5.
4. **`SelectedOptions` on lists** — Renamed to `SelectedItems`.
5. **Single type parameter on lists** — `FluentSelect<string>` is v4.
   v5 requires `FluentSelect<TOption, TValue>`.
6. **`Appearance.Accent`** — Use `ButtonAppearance.Primary` instead.
7. **Targeting net8.0** — v5 requires net9.0 or later.
8. **Implementing `IDialogContentComponent`** — Not needed in v5. Dialog components
   are plain Razor components receiving `IDialogInstance` as a cascading parameter.

---

## Reference Files

- [SETUP.md](references/SETUP.md) — Full setup guide for Blazor Server, WebAssembly, and Auto modes
- [DATAGRID.md](references/DATAGRID.md) — Advanced data grid patterns
- [THEMING.md](references/THEMING.md) — Detailed theming, design tokens, and CSS variables reference
