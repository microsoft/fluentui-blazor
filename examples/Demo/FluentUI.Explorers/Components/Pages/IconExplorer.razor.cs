// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Explorers.Components.Pages;

public partial class IconExplorer
{
    private IconSearchCriteria Criteria { get; set; } = new();

    private static async Task StartSearch(FluentKeyPressEventArgs e)
    {
        await Task.Delay(2000); // Simulate a delay for processing
    }

    internal class IconSearchCriteria
    {
        public string SearchTerm { get; set; } = string.Empty;
        public IconVariant Variant { get; set; } = IconVariant.Regular;
        public int Size { get; set; } = 20;
        public Color Color { get; set; } = Color.Accent;
    }
}
