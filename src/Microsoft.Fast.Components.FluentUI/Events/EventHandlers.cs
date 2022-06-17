using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;


[EventHandler("onfluentcheckedchange", typeof(CheckboxChangeEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("ontabchange", typeof(TabChangeEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
public static class EventHandlers
{
}
