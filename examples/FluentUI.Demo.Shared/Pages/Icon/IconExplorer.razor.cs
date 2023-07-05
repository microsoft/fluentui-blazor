using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared.Pages.Icon;

public partial class IconExplorer
{
    private const int MAX_ICONS = 200;
    private bool SearchInProgress = false;
    private readonly SearchCriteria Criteria = new();
    private Color IconColor = Color.Accent;
    private IEnumerable<IconInfo> IconsFound = Array.Empty<IconInfo>();
    private int IconsCount = 0;
    private IJSObjectReference? _jsModule;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                 "./_content/FluentUI.Demo.Shared/Pages/Icon/IconExplorer.razor.js");
        }
    }

    private async Task HandleSearch()
    {
        if (Criteria.SearchTerm.Length < 2)
        {
            SearchInProgress = false;
            IconsCount = 0;
            return;
        }

        SearchInProgress = true;
        await Task.Delay(1);

        var icons = Icons.AllIcons
                         .Where(i => i.Name.Contains(Criteria.SearchTerm, StringComparison.InvariantCultureIgnoreCase)
                                  && i.Variant == Criteria.Variant
                                  && i.Size == Criteria.Size);

        IconsCount = icons.Count();
        IconsFound = icons.Take(MAX_ICONS).ToArray();

        SearchInProgress = false;
        await Task.Delay(1);
    }

    public async Task HandleColor(ChangeEventArgs args)
    {
        if (!string.IsNullOrEmpty(args.Value?.ToString()))
            IconColor = Enum.Parse<Color>((string)args.Value);

        await HandleSearch();
    }

    public async void HandleClick(IconInfo icon)
    {
        string Text = $$"""<FluentIcon Icon="@Icons.{{icon.Variant}}.{{icon.Size}}.{{icon.Name}}" Color="@Color.{{IconColor}}"/>""";

        if (_jsModule is not null)
        {
            await _jsModule.InvokeVoidAsync("copyText", Text);
        }
    }

    public async Task HandleSize(ChangeEventArgs args)
    {
        if (!string.IsNullOrEmpty(args.Value?.ToString()))
            Criteria.Size = Enum.Parse<IconSize>((string)args.Value);

        await HandleSearch();
    }

    public async Task HandleVariant(ChangeEventArgs args)
    {
        if (!string.IsNullOrEmpty(args.Value?.ToString()))
            Criteria.Variant = Enum.Parse<IconVariant>((string)args.Value);

        await HandleSearch();
    }

    private class SearchCriteria
    {
        public string SearchTerm { get; set; } = string.Empty;
        public IconVariant Variant { get; set; } = IconVariant.Regular;
        public IconSize Size { get; set; } = IconSize.Size24;
    }
}
