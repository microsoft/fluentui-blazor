using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared.Pages.Icon;
public partial class IconPage : IAsyncDisposable
{
    [Inject]
    private ILogger<IconPage> Logger { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    private IconService IconService { get; set; } = default!;

    private IJSObjectReference? _jsModule;

    private List<IconModel>? icons = new();
    private int totalCount = 0;
    private readonly int maxResults = 50;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                 "./_content/FluentUI.Demo.Shared/Pages/Icon/IconPage.razor.js");
        }
    }

    public void HandleStyle()
    {
        if (Form.Filled && Form.Regular || !Form.Filled && !Form.Regular)
            Form.Style = null;
        else if (Form.Filled && !Form.Regular)
            Form.Style = true;
        else if (!Form.Filled && Form.Regular)
            Form.Style = false;

        HandleSearch();
    }

    public void HandleSize(ChangeEventArgs args)
    {
        if (!string.IsNullOrEmpty(args.Value?.ToString()))
            Form.Size = Enum.Parse<IconSize>((string)args.Value);
        else
            Form.Size = IconSize.Size24;

        HandleSearch();
    }

    public void HandleColor(ChangeEventArgs args)
    {
        if (!string.IsNullOrEmpty(args.Value?.ToString()))
            Form.Color = Enum.Parse<Color>((string)args.Value);

        HandleSearch();
    }

    public void HandleSearchField(ChangeEventArgs args)
    {
        Form.Searchterm = (string?)args.Value;

        HandleSearch();
    }

    public void HandleSearch()
    {
        icons?.Clear();

        IconVariant? variant = null;
        if (Form.Style is not null)
        {
            variant = Form.Style.Value ? IconVariant.Filled : IconVariant.Regular;
        }

        FluentIconSearcher searcher = new(IconService);

        icons = searcher.WithName(Form.Searchterm?.Trim())
            .AsVariant(variant)
            .WithSize(Form.Size)
            .ToList(maxResults);

        totalCount = searcher.ResultCount();

        StateHasChanged();
    }

    public void HandleReset()
    {
        Form.Searchterm = "";
        Form.Size = IconSize.Size32;
        Form.Style = null;
        Form.Color = Color.Accent;

        icons?.Clear();
    }

    public async void HandleClick(IconModel icon)
    {
        Logger.LogInformation($"You clicked on {icon.Name}");

        string Text = $$"""<FluentIcon Name="@FluentIcons.{{icon.Name}}" Size="@IconSize.{{icon.Size}}" Variant="@IconVariant.{{icon.Variant}}" Color="@Color.{{Form.Color}}"/>""";

        if (_jsModule is not null)
        {
            await _jsModule.InvokeVoidAsync("copyText", Text);
        }
    }

    private class FormModel
    {
        public IconSize Size { get; set; }

        public string? Searchterm { get; set; }

        public bool? Style { get; set; }

        public bool Filled { get; set; }

        public bool Regular { get; set; }

        public Color Color { get; set; }
    }

    private readonly FormModel Form = new() { Size = IconSize.Size24, Searchterm = "", Style = null, Color = Color.Accent };

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_jsModule is not null)
            {
                await _jsModule.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {
            // The JSRuntime side may routinely be gone already if the reason we're disposing is that
            // the client disconnected. This is not an error.
        }
    }
}