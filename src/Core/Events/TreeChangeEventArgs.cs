namespace Microsoft.FluentUI.AspNetCore.Components;

public class TreeChangeEventArgs : EventArgs
{
    public string? AffectedId { get; set; }

    public bool? Selected { get; set; }

    public bool? Expanded { get; set; }
}
