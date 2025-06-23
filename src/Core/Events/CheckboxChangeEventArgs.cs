// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------
namespace Microsoft.FluentUI.AspNetCore.Components;

public class CheckboxChangeEventArgs : EventArgs
{
    public bool? Checked { get; set; }
    public bool? Indeterminate { get; set; }
}
