using System.Drawing;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
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
    /// Gets or sets the id.
    /// </summary>
    [Parameter]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the variant to use: filled (true) or regular (false)
    /// </summary>
    [Parameter]
    [EditorRequired]
    public bool Filled { get; set; }

    /// <summary>
    /// Gets or sets the icon drawing and fill color. Value comes from the <see cref="IconColor"/> enumeration
    /// </summary>
    [Parameter]
    public IconColor Color { get; set; } = IconColor.Accent;

    /// <summary>
    /// Gets or sets the icon drawing and fill color to a custom value.
    /// Needs to be formatted as an HTML hex color string (#rrggbb or #rgb)
    /// ⚠️ Only available when IconColor is set to IconColor.Custom.
    /// </summary>
    [Parameter]
    public string? CustomColor { get; set; }

    /// <summary>
    /// Gets or sets the name of the icon. Can be specified by using const value from <see cref="FluentIcons" />
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
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Allows for capturing a mouse click on an icon
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }


    protected override async Task OnParametersSetAsync()
    {
        string? color = IconColor.Neutral.ToAttributeValue();
        
        string result;
        string? nc = NeutralCultureName ?? null;

        if (Color != IconColor.Custom)
        {
            if (CustomColor != null)
                throw new ArgumentException("CustomColor can only be used when Color is set to IconColor.Custom. ");
            else
                color = $"var({Color.ToAttributeValue()})";
        }
        else
        {
            if (CustomColor is null)
                throw new ArgumentException("CustomColor must be set when Color is set to IconColor.Custom. ");
            else
            {
#if NET7_0_OR_GREATER
                if (!CheckRGBString().IsMatch(CustomColor))
#else
                if (!Regex.IsMatch(CustomColor, "^#([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$"))
#endif
                    throw new ArgumentException("CustomColor must be a valid HTML hex color string (#rrggbb or #rgb). ");
                else
                    color = CustomColor;
             }
        }

        string folder = FluentIcons.IconMap.First(x => x.Name == Name).Folder;


        
        _iconUrl = $"{ICON_ROOT}/{folder}{(nc is not null ? "/" + nc : "")}/{ComposedName}.svg";
        _iconUrlFallback = $"{ICON_ROOT}/{folder}/{ComposedName}.svg";

        if (!string.IsNullOrEmpty(_iconUrl) && !string.IsNullOrEmpty(_iconUrlFallback))
        {
            HttpRequestMessage? message = CreateMessage(_iconUrl);
            // Get the result from the cache
            result = await CacheStorageAccessor.GetAsync(message);

            if (string.IsNullOrEmpty(result))
            {
                //It is not in the cache, get it from the InconService (download)
                HttpResponseMessage? response = await IconService.HttpClient.SendAsync(message);

                // If unsuccesfull, try with the fallback url. Maybe a non existing neutral culture was specified
                if (!response.IsSuccessStatusCode && _iconUrl != _iconUrlFallback)
                {
                    message = CreateMessage(_iconUrlFallback);

                    // Check if fallback exists in cache
                    result = await CacheStorageAccessor.GetAsync(message);
                    if (string.IsNullOrEmpty(result))
                    {
                        // If not in cache, get it from the InconService (download)
                        response = await IconService.HttpClient.SendAsync(message);

                        if (response.IsSuccessStatusCode)
                            // Store the response in the cache and get the result
                            result = await CacheStorageAccessor.PutAndGetAsync(message, response);
                        else
                            result = string.Empty;
                    }
                }
                else
                {
                    // Store the response in the cache and get the result
                    result = await CacheStorageAccessor.PutAndGetAsync(message, response);
                }
            }

            if (!string.IsNullOrEmpty(result))
            {
                result = result.Replace("<path ", $"<path fill=\"{color}\"");

                string pattern = "<svg (?<attributes>.*?)>(?<path>.*?)</svg>";
                Regex regex = new(pattern);
                MatchCollection matches = regex.Matches(result);
                Match? match = matches.FirstOrDefault();

                if (match is not null)
                    result = match.Groups["path"].Value;

                _svg = result;
                _size = Convert.ToInt32(Size);

            }
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (!string.IsNullOrEmpty(_svg))
        {
            builder.OpenElement(0, "svg");
            builder.AddAttribute(1, "id", Id);
            builder.AddAttribute(2, "slot", Slot);
            builder.AddAttribute(3, "class", Class);
            builder.AddAttribute(4, "style", Style);
            builder.AddAttribute(5, "width", _size);
            builder.AddAttribute(6, "height", _size);
            builder.AddAttribute(7, "viewBox", $"0 0 {_size} {_size}");
            builder.AddAttribute(8, "xmlns", "http://www.w3.org/2000/svg");
            builder.AddAttribute(9, "onclick", OnClickHandler);
            builder.AddMultipleAttributes(10, AdditionalAttributes);
            builder.AddMarkupContent(11, _svg);
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

    public static HttpRequestMessage CreateMessage(string url) => new(HttpMethod.Get, url);

#if NET7_0_OR_GREATER
    [GeneratedRegex("^#(?:[a-fA-F0-9]{6}|[a-fA-F0-9]{3})$")]
    private static partial Regex CheckRGBString();
#endif
    
}
