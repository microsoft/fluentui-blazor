using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Demo.AssetExplorer.Components.Controls;

public partial class PreviewCard
{
    private const string JAVASCRIPT_FILE = "./Components/Controls/PreviewCard.razor.js";

    private IJSObjectReference? JSModule { get; set; }

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    public IToastService ToastService { get; set; } = default!;

    [Parameter]
    public string? TopLeftLabel { get; set; }

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
                return $"{Emoji.Group}.{Emoji.Style}.{Emoji.Skintone}.{Emoji.Name}";
            }

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

            JSModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            var error = await JSModule.InvokeAsync<string>("copyToClipboard", code);

            if (string.IsNullOrEmpty(error))
            {
                ToastService.ShowSuccess($"FluentIcon `{Icon.Name}` component declaration copied to clipboard.");
            }
            else
            {
                ToastService.ShowError(error);
            }
        }

        if (Emoji != null)
        {
            // Emojis.[EmojiGroup].[EmojiStyle].[EmojiSkintone].[EmojiName]
            var value = $"Value=\"@(new Emojis.{FullName}())\"";

            var code = $"<FluentEmoji {value} />";

            JSModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            var error = await JSModule.InvokeAsync<string>("copyToClipboard", code);

            if (string.IsNullOrEmpty(error))
            {
                ToastService.ShowSuccess($"FluentEmoji `{Emoji.Name}` component declaration copied to clipboard.");
            }
            else
            {
                ToastService.ShowError(error);
            }
        }

    }
}
