using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.AssetExplorer.Models;

internal class EmojiSearchCriteria
{
    public string SearchTerm { get; set; } = string.Empty;
    public EmojiStyle Style { get; set; } = EmojiStyle.Color;
    public EmojiSkintone Skintone { get; set; } = EmojiSkintone.Default;
}
