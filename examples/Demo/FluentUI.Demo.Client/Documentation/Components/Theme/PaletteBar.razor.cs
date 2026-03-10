// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Client.Documentation.Components.Theme;

public partial class PaletteBar
{
    [Parameter]
    public Dictionary<string, string> Palette { get; set; } = [];
}
