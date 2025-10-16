// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Explorers.Components.Controls;

public partial class PreviewCard
{
    private ElementReference ImageElement;

    [Inject]
    public required IJSRuntime JSRuntime { get; set; }

    [Parameter]
    public Color? IconColor { get; set; }

    [Parameter]
    public IconInfo? Icon { get; set; }

    [Parameter]
    public EmojiInfo? Emoji { get; set; }

    private string FullName
    {
        get
        {
            if (Icon != null)
            {
                return $"{Icon.Variant}.{Icon.Size}.{Icon.Name}";
            }

            if (Emoji != null)
            {
                return $"{Emoji.Group}.{Emoji.Style}.{Emoji.Skintone}.{Emoji.Name}".Replace("_", "");
            }

            return string.Empty;
        }
    }

    private string GetCodeToCopy(bool fullNamespace)
    {
        var ns = fullNamespace ? "Microsoft.FluentUI.AspNetCore.Components." : "";

        if (Icon != null)
        {
            // Icons.[IconVariant].[IconSize].[IconName]
            var value = $"Value=\"@(new {ns}Icons.{FullName}())\"";
            var color = IconColor == Color.Accent ? string.Empty : $" Color=\"@Color.{IconColor}\"";
            return $"<FluentIcon {value}{color} />";
        }

        if (Emoji != null)
        {
            // Emojis.[Group].[Style].[Skintone].[Name]
            var value = $"Value=\"@(new {ns}Emojis.{FullName}())\"";
            return $"<FluentEmoji {value} />";
        }

        return string.Empty;
    }

    public async Task CopyToClipboardAsync()
    {
        await JSRuntime.InvokeVoidAsync("copyToClipboard", GetCodeToCopy(fullNamespace: false), ImageElement);
    }

    private async Task OnRightClick(MouseEventArgs e)
    {
        await JSRuntime.InvokeVoidAsync("showContextMenu",
            e.ClientX,
            e.ClientY,
            new
            {
                element = ImageElement,
                simplified = GetCodeToCopy(fullNamespace: false),
                full = GetCodeToCopy(fullNamespace: true),
                name = Icon?.Name ?? Emoji?.Name
            });
    }
}
