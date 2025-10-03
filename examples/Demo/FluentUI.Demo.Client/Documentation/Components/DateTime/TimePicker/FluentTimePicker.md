---
title: TimePicker
order: 0003
route: /DateTime/TimePicker
---

# TimePicker

{{ TimePickerDefault }} 

## Value type

The **FluentTimePicker** component is a generic component, so you can use it with date types such as `DateTime?`, `DateTime`, `TimeOnly?` or `TimeOnly`.  
Blazor will automatically infer the type based on the value you provide to the `Value` parameters.  
You can also explicitly set the type using the generic type parameter: `TValue` (i.e. `TValue="TimeOnly?"`).

{{ TimePickerTypes }}
