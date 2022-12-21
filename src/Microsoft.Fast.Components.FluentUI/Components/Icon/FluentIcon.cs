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
    private string? _iconUrl;
    private string? _iconUrlFallback;
    private string? _color;
    private string? _folder;
    private string? _prevResult;

    private int _size;


    [Inject]
    private StaticAssetService StaticAssetService { get; set; } = default!;

    [Inject]
    private CacheStorageAccessor CacheStorageAccessor { get; set; } = default!;

    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    [Parameter]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="IconVariant"/> to use. 
    /// Defaults to Regular
    /// </summary>
    [Parameter]
    public IconVariant Variant { get; set; } = IconVariant.Regular;

    /// <summary>
    /// Gets or sets the icon drawing and fill color. 
    /// Value comes from the <see cref="FluentUI.Color"/> enumeration. Defaults to Accent.
    /// </summary>
    [Parameter]
    public Color Color { get; set; } = Color.Accent;

    /// <summary>
    /// Gets or sets the icon drawing and fill _color to a custom value.
    /// Needs to be formatted as an HTML hex _color string (#rrggbb or #rgb)
    /// ⚠️ Only available when Color is set to Color.Custom.
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
    /// Gets or sets the <see cref="IconSize"/> of the icon. Defaults to 24. Not all sizes are available for all icons.
    /// </summary>
    [Parameter]
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


    //protected override async Task OnParametersSetAsync()
    protected override void OnParametersSet()
    {
        string? nc = NeutralCultureName ?? null;

        if (!FluentIcons.IconMap.Any(x => x.Name == Name && x.Size == Size && x.Variant == Variant))
        {
            throw new ArgumentException($"The requested icon ({Name}, {Size}, {Variant}) is not available in the collection");
        }

        if (Color != Color.Custom)
        {
            if (CustomColor != null)
                throw new ArgumentException("CustomColor can only be used when Color is set to IconColor.Custom. ");
            else
                _color = Color.ToAttributeValue();
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
                if (!Regex.IsMatch(CustomColor, "^(?:#([a-fA-F0-9]{6}|[a-fA-F0-9]{3}))|var\\(--.*\\)$"))
#endif
                    throw new ArgumentException("CustomColor must be a valid HTML hex color string (#rrggbb or #rgb) or CSS variable. ");
                else
                    _color = CustomColor;
            }
        }

        _folder = FluentIcons.IconMap.First(x => x.Name == Name).Folder;

        _iconUrl = $"{ICON_ROOT}/{_folder}{(nc is not null ? "/" + nc : "")}/{ComposedName}.svg";
        _iconUrlFallback = $"{ICON_ROOT}/{_folder}/{ComposedName}.svg";
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        string? result;

        if (!string.IsNullOrEmpty(_iconUrl))
        {
            result = await StaticAssetService.GetAsync(_iconUrl);
            if (string.IsNullOrEmpty(result))
            {
                _iconUrlFallback = $"{ICON_ROOT}/{_folder}/{ComposedName}.svg";
                result = await StaticAssetService.GetAsync(_iconUrlFallback);

                if (string.IsNullOrEmpty(result))
                {
                    return;
                }
            }

            result = result.Replace("<path ", $"<path fill=\"{_color}\"");

            string pattern = "<svg (?<attributes>.*?)>(?<path>.*?)</svg>";
            Regex regex = new(pattern);
            MatchCollection matches = regex.Matches(result);
            Match? match = matches.FirstOrDefault();

            if (match is not null)
                result = match.Groups["path"].Value;


            if (_prevResult != result)
            {
                _svg = result;
                _prevResult = result;
                _size = Convert.ToInt32(Size);

                StateHasChanged();
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
                result = $"{Name}_{(int)Size}_{Variant.ToAttributeValue()}";

            return result;
        }
    }

    private static HttpRequestMessage CreateMessage(string url) => new(HttpMethod.Get, url);

#if NET7_0_OR_GREATER
    [GeneratedRegex("^(?:#(?:[a-fA-F0-9]{6}|[a-fA-F0-9]{3}))|var\\(--.*\\)$")]
    private static partial Regex CheckRGBString();
#endif

}
