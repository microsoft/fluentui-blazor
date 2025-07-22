// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------
namespace Microsoft.FluentUI.AspNetCore.Components;

public class DialogEventArgs : EventArgs
{
    public string? Id { get; set; }
    public string? Reason { get; set; }
}
