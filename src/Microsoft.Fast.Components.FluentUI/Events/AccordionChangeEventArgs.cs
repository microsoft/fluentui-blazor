namespace Microsoft.Fast.Components.FluentUI;

public class AccordionChangeEventArgs : EventArgs
{
    public string? ActiveId { get; set; }
    public string? AffectedId { get; set; }
}
