using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class CheckboxChangeEventArgs : EventArgs
{
    public bool Checked { get; set; }
}

[EventHandler("onfluentcheckedchange", typeof(CheckboxChangeEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
public static class EventHandlers
{
}
