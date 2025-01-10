---
title: Panel
route: /Panel
---

# Panel (Drawer)

A Panel (or a Drawer) is a secondary content surface that slides in from one edge of a layout.
Use panels for supplemental info and simple actions related to the main content.

For short information that's related to a specific part of the main layout, a panel may be overwhelming.
Try a tooltip or a popover instead. If you need people to confirm an action they're trying to take,
use a dialog.

## Usage

A panel is part of the dialog box category and is used in the same way.
The simplest way is to use the **DialogService** to display a panel.
By injecting this service, you have `ShowPanelAsync` methods at your disposal.
You can pass the **type of Dialog** to be displayed and the **options** to be passed to that window.

Once the user closes the dialog window, the `ShowPanelAsync` method returns a `DialogResult` object containing the return data.

👉 See the [Dialog](/Dialog) documentation for more information.

> **Note:** The `ShowPanelAsync` method is identical to the `ShowDialogAsync` method.
> With the exception of the **default value** of the `options.Alignment` property, which is `DialogAlignment.End`.
> And the resulting HTML code is slightly different, using a `fluent-drawer` instead of a `fluent-dialog`.
> The visual rendering and animation of opening and closing is also different.

## Alignment

The panel can be aligned to the left (start) or right (end) of the screen.

By default, the panel is open at the **right (end)** of the screen.
But you can customize this position using the `options.Alignment` property.

```csharp
var result = await DialogService.ShowPanelAsync<Dialog.SimpleDialog>(options =>
{
    options.Alignment = DialogAlignment.Start;
});
```

{{ DialogServicePanel Files=Code:DialogServicePanel.razor;SimpleDialog:SimpleDialog.razor;PersonDetails:PersonDetails.cs }}
