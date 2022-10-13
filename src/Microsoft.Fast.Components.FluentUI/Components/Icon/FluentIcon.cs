using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI.Helpers;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentIcon : FluentComponentBase
{
    [Inject]
    private IconService IconService { get; set; } = default!;

    private string? _svg;
    private int _size;

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


    protected override async Task OnParametersSetAsync()
    {
        const string iconpath = "_content/Microsoft.Fast.Components.FluentUI/icons";
        string result = string.Empty;

        string? nc = NeutralCultureName ?? null;

        string folder = FluentIcons.IconMap.First(x => x.Name == Name).Folder;


        string url = $"{iconpath}/{folder}";

        if (nc is not null)
            url += $"/{nc}";

        url += $"/{ComposedName}.svg";

        try
        {

            HttpResponseMessage? response = await IconService.HttpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            else
            {
                // Fall back to original icon
                url = $"{iconpath}/{folder}/{ComposedName}.svg";
                response = await IconService.HttpClient.GetAsync(url);
                result = await response.Content.ReadAsStringAsync();

            }
        }
        catch (Exception)
        {
            result = "Icon not found";
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

    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
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


}
