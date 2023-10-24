using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared.Pages.Emoji;

public partial class EmojiExplorer
{
    private const int MAX_ICONS = 200;
    private bool SearchInProgress = false;
    private readonly SearchCriteria Criteria = new();
    private IEnumerable<EmojiInfo> EmojisFound = Array.Empty<EmojiInfo>();
    private int EmojisCount = 0;
    private IJSObjectReference? _jsModule;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                 "./_content/FluentUI.Demo.Shared/Pages/Emoji/EmojiExplorer.razor.js");
        }
    }

    private async Task HandleSearch()
    {
        if (Criteria.SearchTerm.Length < 2)
        {
            SearchInProgress = false;
            EmojisCount = 0;
            return;
        }

        SearchInProgress = true;
        await Task.Delay(1);

        var emojis = Emojis.AllEmojis
                           .Where(i => i.Name.Contains(Criteria.SearchTerm, StringComparison.InvariantCultureIgnoreCase)
                                && (i.Group == Criteria.Group) && (i.Style == Criteria.Style) && (i.Skintone == Criteria.Skintone));

        EmojisCount = emojis.Count();
        EmojisFound = emojis.Take(MAX_ICONS).ToArray();

        SearchInProgress = false;
        await Task.Delay(1);
    }

    public async Task HandleSkintone(ChangeEventArgs args)
    {
        if (!string.IsNullOrEmpty(args.Value?.ToString()) && args.Value?.ToString() != "-1")
            Criteria.Skintone = Enum.Parse<EmojiSkintone>((string)args.Value!);

        await HandleSearch();
    }

    public async Task HandleStyle(ChangeEventArgs args)
    {
        if (!string.IsNullOrEmpty(args.Value?.ToString()) && args.Value?.ToString() != "-1")
            Criteria.Style = Enum.Parse<EmojiStyle>((string)args.Value!);

        await HandleSearch();
    }

    public async Task HandleGroup(ChangeEventArgs args)
    {
        if (!string.IsNullOrEmpty(args.Value?.ToString()) && args.Value?.ToString() != "-1")
            Criteria.Group = Enum.Parse<EmojiGroup>((string)args.Value!);

        await HandleSearch();
    }

    public async Task HandleSize(ChangeEventArgs args)
    {
        if (!string.IsNullOrEmpty(args.Value?.ToString()) && args.Value?.ToString() != "-1")
            Criteria.Size = Enum.Parse<EmojiSize>((string)args.Value!);

        await HandleSearch();
    }


    public async void HandleClick(EmojiInfo emoji)
    {
        //Emojis.SmileysEmotion.Color.Default.RollingOnTheFloorLaughing


        string Text = $$"""<FluentEmoji Value="@(new Emojis.{{emoji.Group}}.{{emoji.Style}}.{{emoji.Skintone}}.{{emoji.Name}}())" Width="{{(int)Criteria.Size!}}px" />""";

        if (_jsModule is not null)
        {
            await _jsModule.InvokeVoidAsync("copyText", Text.Replace("_", ""));
        }
    }

    private class SearchCriteria
    {
        public string SearchTerm { get; set; } = string.Empty;
        public EmojiGroup Group { get; set; } = EmojiGroup.Objects;
        public EmojiStyle Style { get; set; } = EmojiStyle.Color;
        public EmojiSkintone Skintone { get; set; } = EmojiSkintone.Default;
        public EmojiSize Size { get; set; } = EmojiSize.Size32;
    }
}
