// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------
namespace Microsoft.FluentUI.AspNetCore.Components;
internal interface IOptionIcon
{
    (Icon Value, Color? Color, string? Slot)? Icon { get; set; }
}
