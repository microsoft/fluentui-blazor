using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;


[EventHandler("oncheckedchange", typeof(CheckboxChangeEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("ontabchange", typeof(TabChangeEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("onselectedchange", typeof(TreeChangeEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("onexpandedchange", typeof(TreeChangeEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("ondateselected", typeof(CalendarSelectEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("onaccordionchange", typeof(AccordionChangeEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
public static class EventHandlers
{
}
