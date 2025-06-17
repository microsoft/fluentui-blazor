// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------
namespace Microsoft.FluentUI.AspNetCore.Components;
internal interface IOptionIcon
{
    (Icon Value, Color? Color, string? Slot)? Icon { get; set; }
}
