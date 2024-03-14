namespace Microsoft.FluentUI.AspNetCore.Components;

public class AccordionChangeEventArgs : EventArgs
{
    public string? ActiveId { get; set; }
    public bool Expanded { get; set; }

}
