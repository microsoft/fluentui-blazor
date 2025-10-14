// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Explorers.Components.Controls;

public partial class PreviewCard
{
    [Inject]
    public required IJSRuntime JSRuntime { get; set; }

    [Parameter]
    public Color? IconColor { get; set; }

    [Parameter]
    public IconInfo? Icon { get; set; }

    //[Parameter]
    //public EmojiInfo? Emoji { get; set; }

    private string FullName
    {
        get
        {
            if (Icon != null)
            {
                return $"{Icon.Variant}.{Icon.Size}.{Icon.Name}";
            }

            //if (Emoji != null)
            //{
            //    return $"{Emoji.Group}.{Emoji.Style}.{Emoji.Skintone}.{Emoji.Name}".Replace("_", "");
            //}

            return string.Empty;
        }
    }

    public async void CopyToClipboardAsync()
    {
        if (Icon != null)
        {
            // Icons.[IconVariant].[IconSize].[IconName]
            var value = $"Value=\"@(new Icons.{FullName}())\"";
            var color = IconColor == Color.Accent ? string.Empty : $" Color=\"@Color.{IconColor}\"";

            var code = $"<FluentIcon {value}{color} />";

            await JSRuntime.InvokeVoidAsync("copyToClipboard", code);
        }
    }
}
