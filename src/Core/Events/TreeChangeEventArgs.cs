namespace Microsoft.Fast.Components.FluentUI;

public class TreeChangeEventArgs : EventArgs
{
    public string? AffectedId { get; set; }

    public bool? Selected { get; set; }

    public bool? Expanded { get; set; }
}
