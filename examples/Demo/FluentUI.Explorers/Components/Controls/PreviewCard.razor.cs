// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Explorers.Components.Controls;

public partial class PreviewCard
{
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

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    private void CardClick()
    {

    }
}
