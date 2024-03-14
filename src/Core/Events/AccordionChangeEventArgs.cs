namespace Microsoft.Fast.Components.FluentUI;

public class AccordionChangeEventArgs : EventArgs
{
    public string? ActiveId { get; set; }
    public bool Expanded { get; set; }

}
