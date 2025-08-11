---
title: Calendar
order: 0001
route: /DateTime/Calendar
---

# Date and Time pickers

The calendar control lets people select and view a single date or a range of dates in their calendar.
It's made up of 3 separate views: the "day" view, "month" view, and "year" view.

## Example

{{ FluentCalendarDefault }}

## Selections

{{ FluentCalendarSelection }}


## Customization

You can customize the calendar by providing a custom template for the day cells.
In this example, we use a custom template to display the days 5 and 15 in a different color.  
And we also add a `DisabledDate` function to disable the 3th, 10th and 20th of the month.

{{ CalendarDayCustomized }}

## Culture

You can set the culture of the calendar to display the month and day names in different languages.
Use the `Culture` parameter to set the culture.

Example: `Culture="@(new CultureInfo("fr"))"` will display the calendar in French.

{{ CalendarCulture }}


## API FluentCalendar

{{ API Type=FluentCalendar }}
