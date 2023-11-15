using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.AssetExplorer.Models;

internal class IconSearchCriteria
{
    public string SearchTerm { get; set; } = string.Empty;
    public IconVariant Variant { get; set; } = IconVariant.Regular;
    public IconSize Size { get; set; } = IconSize.Size24;
    public Color Color { get; set; } = Color.Accent;
}
