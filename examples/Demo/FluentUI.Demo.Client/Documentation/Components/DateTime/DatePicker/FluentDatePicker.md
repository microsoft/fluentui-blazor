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

## API FluentCalendar

{{ API Type=FluentCalendar }}
