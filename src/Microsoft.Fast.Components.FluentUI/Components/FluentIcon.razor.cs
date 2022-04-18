using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentIcon
{
    [Inject]
    private IconService IconService { get; set; } = default!;

    private MarkupString svg;

    /// <summary>
    /// Gets or sets the variant to use: filled (true) or regular (false)
    /// </summary>
    [Parameter]
    [EditorRequired]
    public bool Filled { get; set; }

    /// <summary>
    /// Gets or sets the use of the current Fluent Accent Color
    /// </summary>
    [Parameter]
    public bool UseAccentColor { get; set; } = true;

    /// <summary>
    /// Gets or sets the name of the icon. Can be specified by using const value from <c ref="FluentIcons" />
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string Name { get; set; } = String.Empty;

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
    /// Gets or sets a collection of additional attributes that will be applied to the input element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? AdditionalAttributes { get; set; }

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

        if (UseAccentColor)
            result = result.Replace("<path ", "<path fill=\"var(--accent-fill-rest)\"");
        if (!string.IsNullOrEmpty(Slot))
            result = result.Replace("<svg ", $"<svg slot=\"{Slot}\" ");

        svg = (MarkupString)result;

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
