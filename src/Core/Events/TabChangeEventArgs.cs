// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------
namespace Microsoft.FluentUI.AspNetCore.Components;

public class TabChangeEventArgs : EventArgs
{
    public string? ActiveId { get; set; }
    public string? AffectedId { get; set; }
}
