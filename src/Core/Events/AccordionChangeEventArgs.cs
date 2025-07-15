// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------
namespace Microsoft.FluentUI.AspNetCore.Components;

public class AccordionChangeEventArgs : EventArgs
{
    public string? ActiveId { get; set; }
    public bool Expanded { get; set; }

}
