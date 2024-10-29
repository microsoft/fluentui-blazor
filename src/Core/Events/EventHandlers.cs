using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

[EventHandler("oncheckedchange", typeof(CheckboxChangeEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("onswitchcheckedchange", typeof(CheckboxChangeEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("onradiogroupchange", typeof(ChangeEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("onsliderchange", typeof(ChangeEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("ontabchange", typeof(TabChangeEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("onselectedchange", typeof(TreeChangeEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("onexpandedchange", typeof(TreeChangeEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("onaccordionchange", typeof(AccordionChangeEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("ondialogdismiss", typeof(DialogEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("onmenuchange", typeof(MenuChangeEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("onscrollstart", typeof(HorizontalScrollEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("onscrollend", typeof(HorizontalScrollEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("onclosecolumnoptions", typeof(EventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("onclosecolumnresize", typeof(EventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("ontooltipdismiss", typeof(EventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("onsplitterresized", typeof(SplitterResizedEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("onsplittercollapsed", typeof(SplitterCollapsedEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("oncontrolinput", typeof(ChangeEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("oncomboboxchange", typeof(ChangeEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
public static class EventHandlers
{
}
