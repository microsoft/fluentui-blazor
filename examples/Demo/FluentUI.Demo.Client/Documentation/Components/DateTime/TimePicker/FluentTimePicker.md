---
title: TimePicker
order: 0003
route: /DateTime/TimePicker
---

# TimePicker

**FluentTimePicker** offers a control that's optimized for selecting a time from a
drop-down list or using free-form input to select a predefined time.
If the user write an invalid time, the input field content will be deleted when the input field loses focus.

You can use the `Culture` parameter to display the time in different format patterns;
and to set the time format displayed in the input field.

{{ TimePickerDefault }}

> [!NOTE] When you bind a DateTime value to the TimePicker, the date part is ignored in the input field.
> But the date part is preserved in the bound value.
> For nullable DateTime (`DateTime?`), the value will be deleted when the user clears the input field
> using `Delete` key.

## Culture

You can set the culture of the TimePicker to display the hours and minutes in different
formats. Use the `Culture` parameter to set the desired time format.
We are using the `Culture.DateTimeFormat.ShortTimePattern` property to display the time with the correct pattern (e.g. `HH:mm`).

Example: `Culture="@(new CultureInfo("fr"))"` will display the time in French (`HH:mm`).

{{ TimePickerCulture }}

## RenderStyle

The TimePicker can be rendered in different modes: `FluentUI`, `Native`.

The `FluentUI` rendering style uses the Fluent UI styles and components.
This mode provides a consistent look and feel with other Fluent UI components.
This mode provides all the features of the component.

The `native` rendering style uses the default browser styles.
This mode is useful when you want to use a very simple TimePicker with a **mobile device**.
In this case, the mobile picker will be used. This could be useful to use the native Android or iOS date picker.

> [!WARNING] This mode is very limited in features and does not support the UI customization (`DisabledTimeFunc`, `StartHour`, ...).
> The following parameters are ignored: `Culture`, `StartHour`, `EndHour`, `DisabledTimeFunc`.

{{ TimePickerRendering }}

## Value type

The **FluentTimePicker** component is a generic component, so you can use it with date types such as `DateTime?`, `DateTime`, `TimeOnly?` or `TimeOnly`.
Blazor will automatically infer the type based on the value you provide to the `Value` parameters.
You can also explicitly set the type using the generic type parameter: `TValue` (i.e. `TValue="TimeOnly?"`).

{{ TimePickerTypes }}
