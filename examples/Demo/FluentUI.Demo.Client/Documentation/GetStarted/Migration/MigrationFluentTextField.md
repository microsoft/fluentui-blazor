---
title: Migration FluentTextField, FluentNumberField, FluentSearch
route: /Migration/TextField
hidden: true
---

### Two components merged into one 

`FluentTextField` and `FluentSearch` have been **removed** in V5. They are all replaced by `FluentTextInput`.

### Component mapping

| V4 Component | V5 Replacement |
|-------------|----------------|
| `FluentTextField` | `FluentTextInput` |
| `FluentSearch` | `FluentTextInput` with a search icon in `StartTemplate` |

### FluentTextField → FluentTextInput

| V4 Property | V5 Property | Change |
|-------------|-------------|--------|
| `TextFieldType` (`TextFieldType?`) | `TextInputType` (`TextInputType?`) | Enum renamed |
| `Appearance` (`FluentInputAppearance`) | `Appearance` (`TextInputAppearance`) | Enum renamed |
| `Disabled` | `Disabled`  | Type changed from `bool` to `bool?` |
| `Size` (`int?`) | `Size` (`TextInputSize?`) | Changed from pixel count to enum |
| `InputMode` (`InputMode?`) | `InputMode` (`TextInputMode?`) | Enum renamed |
| `ChildContent` | — | **Removed** — use `StartTemplate`/`EndTemplate` |
| `Maxlength` | `MaxLength` | Casing changed |
| `Minlength` | `MinLength` | Casing changed |
| `DataList` | `DataList` | Same |


### FluentSearch migration

```razor
<!-- V4 -->
<FluentSearch @bind-Value="searchText"
            Appearance="FluentInputAppearance.Outline" />

<!-- V5 -->
<FluentTextInput @bind-Value="searchText"
                Appearance="TextInputAppearance.Outline"
                Placeholder="Search...">
    <StartTemplate>
        <FluentIcon Value="@(new Icons.Regular.Size16.Search())" />
    </StartTemplate>
</FluentTextInput>
```

### Appearance mapping

| V4 `FluentInputAppearance` | V5 `TextInputAppearance` |
|---------------------------|-------------------------|
| `FluentInputAppearance.Outline` | `TextInputAppearance.Outline` |
| `FluentInputAppearance.Filled` | `TextInputAppearance.FilledDarker` |

Migration helper available: `FluentInputAppearance.Filled.ToTextInputAppearance()`
