namespace Microsoft.Fast.Components.FluentUI;

public class TabChangeEventArgs : EventArgs
{
    public string? ActiveId { get; set; }
    public string? AffectedId { get; set; }
}
