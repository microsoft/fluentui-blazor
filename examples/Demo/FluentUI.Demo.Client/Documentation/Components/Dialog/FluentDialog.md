---
title: Dialog
route: /Dialog
---

# Dialog

`FluentDialog` is a window overlaid on either the primary window or another dialog window.
Windows under a modal dialog are inert. That is, users cannot interact with content outside an active dialog window.
Inert content outside an active dialog is typically visually obscured or dimmed so it is difficult to discern, and in some implementations,
attempts to interact with the inert content cause the dialog to close.

## Best practices

### Do
 - Dialog boxes consist of a header (`TitleTemplate`), content (`ChildContent`), and footer (`ActionTemplate`),
   which should all be included inside a body (`FluentDialogBody`).
 - Validate that people’s entries are acceptable before closing the dialog. Show an inline validation error near the field they must correct.
 - Modal dialogs should be used very sparingly—only when it's critical that people make a choice or provide information before they can proceed.
   Thee dialogs are generally used for irreversible or potentially destructive tasks. They're typically paired with an backdrop without a light dismiss.

### Don't
 - Don't use more than three buttons between Dialog'`ActionTemplate`.
 - Don't open a `FluentDialog` from a `FluentDialog`.
 - Don't use a `FluentDialog` with no focusable elements

## Usage

The simplest way is to use the DialogService to display a dialog box.
By injecting this service, you have `ShowDialogAsync` methods at your disposal.
You can pass the **type of Dialog** to be displayed and the **options** to be passed to that window.

Once the user closes the dialog window, the `ShowDialogAsync` method returns a `DialogResult` object containing the return data.

Any Blazor component can be used as a dialog type.

```csharp
@inject IDialogService DialogService

var result = await DialogService.ShowDialogAsync<SimpleDialog>(options =>
{
    // Options
});

if (result.Cancelled == false)
{
    ...
}
```

The **SimpleDialog** window can inherit from the `FluentDialogInstance` class to have access to the `DialogInstance` property, which contains
all parameters defined when calling `ShowDialogAsync`, as well as the `CloseAsync` and `CancelAsync` methods.
Using `CloseAsync` you can pass a return object to the parent component.
It is then retrieved in the `DialogResult` like in the example below.

```xml
@inherits FluentDialogInstance

<FluentDialogBody>
    Content dialog
</FluentDialogBody>
```

> **Note:** The `FluentDialogBody` component is required to display the dialog window correctly.

By default, the `FluentDialogInstance` class will offer two buttons, one to validate and one to cancel (**OK** and **Cancel**).
You can override the actions of these buttons by overriding the `OnActionClickedAsync` method.

{{ DialogServiceDefault Files=Code:DialogServiceDefault.razor;SimpleDialog:SimpleDialog.razor;PersonDetails:PersonDetails.cs }}

## Customized

The previous `FluentDialogInstance` object is optional. You can create your own custom dialog box in two different ways.

🔹Using the `ShowDialogAsync` method options to pass parameters to your dialog box.
   Several **options** can be used to customize the dialog box, such as styles, title, button text, etc.

```csharp
var result = await DialogService.ShowDialogAsync<CustomizedDialog>(options =>
{
    options.Header.Title = "Dialog Title";
    options.Alignment = DialogAlignment.Default;
    options.Style = "max-height: 400px;";
    options.Parameters.Add("Name", "John");  // Simple data will send to the dialog "Name" property.
});
```

🔹Using the `FluentDialogBody` component to create a custom dialog box.
   This component allows you to create a dialog box with:
    - `TitleTemplate`: Title of the dialog box.
    - `TitleActionTemplate`: (optional) Action to be performed on the title dismiss icon.
    - `ChildContent`: Content of the dialog box.
    - `ActionsTemplate`: Footer of the dialog box, containing close and cancel buttons.

   To have a reference to the `DialogInstance` object, you must use the `CascadingParameter` attribute.
   This object allows you to retrieve the dialog options and to close the dialog box and pass a return object to the parent component.

```xml
<FluentDialogBody>

    <TitleTemplate>
        @Dialog?.Options.Header.Title
    </TitleTemplate>

    <ChildContent>
        Content dialog
    </ChildContent>

    <ActionTemplate>
        <FluentButton OnClick="@(e => Dialog.CancelAsync())">Cancel</FluentButton>
        <FluentButton OnClick="@(e => Dialog.CloseAsync())" Appearance="ButtonAppearance.Primary">OK</FluentButton>
    </ActionTemplate>

</FluentDialogBody>

@code
{
    [CascadingParameter]
    public required IDialogInstance Dialog { get; set; }
}
```

{{ DialogServiceCustomized Files=Code:DialogServiceCustomized.razor;CustomizedDialog:CustomizedDialog.razor;PersonDetails:PersonDetails.cs }}

## Data exchange between components

🔹You can easily send data from your main component to the dialog box
by using the `options.Parameters.Add()` method in the `ShowDialogAsync` method options.

`Parameters` is a dictionary that allows you to send any type of data to the dialog box.
the **key is the name of the property** in the dialog box, and the **value** is the data to be sent.

**Main component**
```csharp
PersonDetails John = new() { Age = "20" };

var result = await DialogService.ShowDialogAsync<CustomizedDialog>(options =>
{
    options.Parameters.Add(nameof(SimpleDialog.Name), "John");  // Simple type
    options.Parameters.Add(nameof(SimpleDialog.Person), John);  // Updatable object
});
```

> **Note:** The `nameof` operator is used to avoid typing errors when passing parameters.
> The key must match the name of the property in the dialog box.
> If the key does not match, an exception will be thrown.

**SimpleDialog**
```csharp
<FluentDialogBody>
    ...
</FluentDialogBody>

@code {
    // A simple type is not updatable
    [Parameter]
    public string? Name { get; set; }

    // A class is updatable
    [Parameter]
    public PersonDetails Person { get; set; } = new();
}
```

In .NET, simple types are passed by value, while objects are passed by reference.
So if you modify an object in the dialog box, the changes will also be visible in the main component.
This is not the case for simple types.

🔹You can also send data from the dialog box to the main component by using the `CloseAsync` method.
This method allows you to pass a return object to the parent component.

**Dialog box**
```csharp
protected override async Task OnActionClickedAsync(bool primary)
{
    if (primary)
    {
        await DialogInstance.CloseAsync("You clicked on the OK button");
    }
    else
    {
        await DialogInstance.CancelAsync();
    }
}
```

In the main component, you can retrieve the return object from the `DialogResult` object.
And you can check if the dialog box was closed by clicking on the **OK** or **Cancel** button, using the `Cancelled` property.

> **Note:** The `CancelAsync` method can not pass a return object to the parent component.
> But the `CloseAsync` method can pass a"Canceled" result object to the parent component.
> `await DialogInstance.CloseAsync(DialogResult.Cancel("You clicked on the Cancel button"));`

## Without the DialogService

You can also use the `FluentDialogBody` component directly in your component, without using the `DialogService` and the `FluentDialog` component.

{{ DialogBodyDefault }}

## API DialogService

{{ API Type=DialogService }}

## API FluentDialogBody

{{ API Type=FluentDialogBody }}

## API FluentDialog

{{ API Type=FluentDialog }}
