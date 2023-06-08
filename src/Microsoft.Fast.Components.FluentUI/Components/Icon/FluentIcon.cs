using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI.Infrastructure;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentIcon : FluentComponentBase
{
    private const string ICON_ROOT = "_content/Microsoft.Fast.Components.FluentUI/icons";
    private string? _svg;
    private string? _iconUrl;
    private string? _iconUrlFallback;
    private string? _color;

    private int _size;

    private string? _previousId;
    private string? _previousIconUrl;
    private string? _previousColor;
    private string? _previousSlot;
    private string? _previousIconContent;
    private string? _previousClass;
    private string? _previousStyle;
    private IReadOnlyDictionary<string, object>? _previousAdditionalAttributes;

    private bool _iconChanged;
    private bool _shouldRender;

    [Inject]
    private IStaticAssetService StaticAssetService { get; set; } = default!;

    [Inject]
    private IconService IconService { get; set; } = default!;

    protected string? StyleValue => new StyleBuilder()
        .AddStyle("fill", _color)
        .AddStyle("cursor", "pointer", OnClick.HasDelegate)
        .AddStyle(Style)
        .Build();

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
    /// Gets or sets the <see cref="IconSize"/> of the icon. Defaults to 20. Not all sizes are available for all icons.
    /// </summary>
    [Parameter]
    public IconSize Size { get; set; } = IconSize.Size20;

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

        if (!FluentIcons.IconAvailable(IconService.Configuration, this))
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

        _iconUrl = $"{ICON_ROOT}/{Name}{(nc is not null ? "/" + nc : "")}/{ComposedName}.svg";
        _iconUrlFallback = $"{ICON_ROOT}/{Name}/{ComposedName}.svg";

        _iconChanged = GetIconChanged();

        _shouldRender = _iconChanged || GetParametersChanged();
        UpdatePreviousParameters();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!_iconChanged)
        {
            return;
        }

        _iconChanged = false;
        var iconContent = await GetIconContentAsync();

        if (_previousIconContent != iconContent)
        {
            _svg = iconContent;
            _previousIconContent = iconContent;
            _size = Convert.ToInt32(Size);
            StateHasChanged();
        }
    }

    protected override bool ShouldRender()
    {
        return _shouldRender;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (!string.IsNullOrEmpty(_svg))
        {
            builder.OpenElement(0, "svg");
            builder.AddAttribute(1, "id", Id);
            builder.AddAttribute(2, "slot", Slot);
            builder.AddAttribute(3, "class", Class);
            builder.AddAttribute(4, "style", StyleValue);
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

    private async Task<string?> GetIconContentAsync()
    {
        _previousIconUrl = _iconUrl;

        string? result = null;

        if (!string.IsNullOrEmpty(_iconUrl))
        {
            result = await StaticAssetService.GetAsync(_iconUrl);

            if (string.IsNullOrEmpty(result) &&
                !string.IsNullOrEmpty(_iconUrlFallback))
            {
                result = await StaticAssetService.GetAsync(_iconUrlFallback);
            }
        }

        if (string.IsNullOrEmpty(result))
        {
            return result;
        }

        string pattern = "<svg (?<attributes>.*?)>(?<path>.*?)</svg>";
        Regex regex = new(pattern);
        MatchCollection matches = regex.Matches(result);
        Match? match = matches.FirstOrDefault();

        if (match is not null)
            result = match.Groups["path"].Value;

        return result;
    }

    private string ComposedName
    {
        get
        {
            string variant = Variant switch
            {
                IconVariant.Filled => "f",
                IconVariant.Regular => "r",
                _ => ""
            };

            return $"{(int)Size}_{variant}";
        }
    }

    private bool GetIconChanged()
    {
        return _previousIconUrl != _iconUrl;
    }

    private bool GetParametersChanged() =>
        _previousId != Id ||
        _previousColor != _color ||
        _previousSlot != Slot ||
        _previousClass != Class ||
        _previousStyle != Style ||
        !_previousAdditionalAttributes.RenderedAttributesEqual(AdditionalAttributes);

    private void UpdatePreviousParameters()
    {
        _previousId = Id;
        _previousColor = _color;
        _previousSlot = Slot;
        _previousClass = Class;
        _previousStyle = Style;
        _previousAdditionalAttributes = AdditionalAttributes;
    }

#if NET7_0_OR_GREATER
    [GeneratedRegex("^(?:#(?:[a-fA-F0-9]{6}|[a-fA-F0-9]{3}))|var\\(--.*\\)$")]
    private static partial Regex CheckRGBString();
#endif

}
