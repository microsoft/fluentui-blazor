using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;


[EventHandler("oncheckedchange", typeof(CheckboxChangeEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("ontabchange", typeof(TabChangeEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("onselectedchange", typeof(TreeChangeEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("onexpandedchange", typeof(TreeChangeEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("ondateselected", typeof(CalendarSelectEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
// This accordion event needs to wait until necessary changes are merged in the web components script
//[EventHandler("onaccordionchange", typeof(AccordionChangeEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("ondismiss", typeof(EventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("onmenuchange", typeof(EventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("onscrollstart", typeof(HorizontalScrollEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("onscrollend", typeof(HorizontalScrollEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("oncellfocus", typeof(DataGridCellFocusEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("onrowfocus", typeof(DataGridRowFocusEventArgs), enableStopPropagation: true, enablePreventDefault: true)]

public static class EventHandlers
{
}
