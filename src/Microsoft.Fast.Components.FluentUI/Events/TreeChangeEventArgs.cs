using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class TreeChangeEventArgs : EventArgs
{
    public ElementReference? AffectedItem { get; set; }
}
