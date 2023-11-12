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
    public Color? IconColor { get; set; }

    [Parameter]
    public IconInfo? Icon { get; set; }

    [Parameter]
    public EmojiInfo? Emoji { get; set; }

    public async void CopyToClipboardAsync()
    {
        if (Icon != null)
        {
            string value = $"Value=\"@(new Icons.{Icon.Variant}.{Icon.Size}.{Icon.Name}())\"";
            string color = IconColor == Color.Accent ? string.Empty : $" Color=\"@Color.{IconColor}\"";

            string code = $"<FluentIcon {value}{color} />";

            JSModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            var error = await JSModule.InvokeAsync<string>("copyToClipboard", code);

            ToastService.ShowSuccess($"FluentIcon `{Icon.Name}` component declaration copied to clipboard.");
        }
    }
}
