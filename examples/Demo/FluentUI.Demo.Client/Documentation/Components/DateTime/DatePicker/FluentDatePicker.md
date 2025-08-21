---
title: DatePicker
order: 0002
route: /DateTime/DatePicker
---

# DatePicker

XXX

## Views

The DatePicker can be displayed in different views: day, month, and year.  
The user can switch between these views using the title item: the month name or the year number.

{{ DatePickerDefault }} 

## DoubleClickToDate

The DatePicker can be configured to allow double-clicking on the input field.
When the user double-clicks, the specified date is set in the input field.

In this example, the parameter is set to "Today": `DoubleClickToDate="@DateTime.Today"`.

{{ DatePickerDoubleClickToDate }}

## Culture

You can set the culture of the calendar to display the month and day names in different languages.
Use the `Culture` parameter to set the culture.

Example: `Culture="@(new CultureInfo("fr"))"` will display the calendar in French.

{{ DatePickerCulture }}

## RenderingMode

The DatePicker can be rendered in different modes: `FluentUI`, `Browser`.

The `FluentUI` rendering mode uses the Fluent UI styles and components.
This mode provides a consistent look and feel with other Fluent UI components.
This mode provides all the features of the component.

The `Browser` rendering mode uses the default browser styles.
This mode is useful when you want to use a very simple DatePicker with a **mobile device**.
In this case, the mobile picker will be used. This could be useful to use the native Android or iOS date picker.

> [!WARNING]
> 
> This mode is very limited in features and does not support the UI customization (`DisabledDateFunc`, `DaysTemplate`, ...).
> The following parameters are ignored: `Culture`, `DayFormat`, `DisabledDateFunc`, `DisabledCheckAllDaysOfMonthYear`,
> `DisabledSelectable`, `DaysTemplate`, `PickerMonthChanged`.

{{ DatePickerRendering }}

## API FluentDatePicker

{{ API Type=FluentDatePicker }}
