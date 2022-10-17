using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI.Helpers;
using Microsoft.Fast.Components.FluentUI.Infrastructure;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentIcon : FluentComponentBase
{
    private const string ICON_ROOT = "_content/Microsoft.Fast.Components.FluentUI/icons";
    private string? _svg;
    private int _size;
    private string? _iconUrl, _iconUrlFallback;


    [Inject]
    private IconService IconService { get; set; } = default!;

    [Inject]
    private CacheStorageAccessor CacheStorageAccessor { get; set; } = default!;

    /// <summary>
    /// Gets or sets the variant to use: filled (true) or regular (false)
    /// </summary>
    [Parameter]
    [EditorRequired]
    public bool Filled { get; set; }

    /// <summary>
    /// Gets or sets the icon drawing and fill color <see cref="IconColor"/>
    /// </summary>
    [Parameter]
    public IconColor Color { get; set; } = IconColor.Accent;

    /// <summary>
    /// Gets or sets the name of the icon. Can be specified by using const value from <c ref="FluentIcons" />
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the size of the icon. Defaults to 24. Not all sizes are available for all icons
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IconSize Size { get; set; } = IconSize.Size24;

    /// <summary>
    /// Gets or sets the slot where the icon is displayed in
    /// </summary>
    [Parameter]
    public string? Slot { get; set; } = null;

    /// <summary>
    /// Gets or sets the two letter neutral culture variant used. Not all neutral cultures are available for all icons
    /// </summary>
    [Parameter]
    public string? NeutralCultureName { get; set; } = null;

    /// <summary>
    /// Allows for capturing a mouse click on an icon
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }


    protected override void OnParametersSet()
    {
        string? nc = NeutralCultureName ?? null;
        string folder = FluentIcons.IconMap.First(x => x.Name == Name).Folder;

        _iconUrl = $"{ICON_ROOT}/{folder}{(nc is not null ? "/" + nc : "")}/{ComposedName}.svg";
        _iconUrlFallback = $"{ICON_ROOT}/{folder}/{ComposedName}.svg";
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        string result;
        if (firstRender && !string.IsNullOrEmpty(_iconUrl) && !string.IsNullOrEmpty(_iconUrlFallback))
        {
            HttpRequestMessage? message = CreateMessage(_iconUrl);
            // Get the result from the cache
            result = await CacheStorageAccessor.GetAsync(message);

            if (string.IsNullOrEmpty(result))
            {
                //It is not in the cache, get it from the InconService (download)
                HttpResponseMessage? response = await IconService.HttpClient.SendAsync(message);

                // If unsuccesfull, try with the fallback url. Maybe a non existing neutral culture was specified
                if (!response.IsSuccessStatusCode)
                {
                    message = CreateMessage(_iconUrlFallback);
                    response = await IconService.HttpClient.SendAsync(message);
                }

                // Store the result in the cache and get it as well
                result = await CacheStorageAccessor.PutAndGetAsync(message, response);
                //result = await CacheStorageAccessor.GetAsync(message);
            }

            result = result.Replace("<path ", $"<path fill=\"var({Color.ToAttributeValue()})\"");

            string pattern = "<svg (?<attributes>.*?)>(?<path>.*?)</svg>";
            Regex regex = new(pattern);
            MatchCollection matches = regex.Matches(result);
            Match? match = matches.FirstOrDefault();

            if (match is not null)
                result = match.Groups["path"].Value;

            _svg = result;
            _size = Convert.ToInt32(Size);

            StateHasChanged();
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (!string.IsNullOrEmpty(_svg))
        {
            builder.OpenElement(0, "svg");
            builder.AddAttribute(1, "slot", Slot);
            builder.AddAttribute(2, "class", Class);
            builder.AddAttribute(3, "style", Style);
            builder.AddMultipleAttributes(4, AdditionalAttributes);
            builder.AddAttribute(5, "width", _size);
            builder.AddAttribute(6, "height", _size);
            builder.AddAttribute(7, "viewBox", $"0 0 {_size} {_size}");
            builder.AddAttribute(8, "xmlns", "http://www.w3.org/2000/svg");
            builder.AddAttribute(9, "onclick", OnClickHandler);
            builder.AddMarkupContent(10, _svg);
            builder.CloseElement();
        }
    }

    protected async Task OnClickHandler(MouseEventArgs e)
    {
        if (OnClick.HasDelegate)
            await OnClick.InvokeAsync(e);
    }

    public string ComposedName
    {
        get
        {
            string result = "";
            if (Name is not null)
                result = $"{Name}_{(int)Size}_{(Filled ? "filled" : "regular")}";

            return result;
        }
    }

    public static HttpRequestMessage CreateMessage(string url) => new HttpRequestMessage(HttpMethod.Get, url);
}
