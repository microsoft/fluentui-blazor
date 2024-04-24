using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.AssetExplorer.Models;

internal class IconSearchCriteria
{
    public string SearchTerm { get; set; } = string.Empty;
    public IconVariant Variant { get; set; } = IconVariant.Regular;
    public int Size { get; set; } = 20;
    public Color Color { get; set; } = Color.Accent;
}
