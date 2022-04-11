using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentIcon
{
    [Inject]
    private IconService? IconService { get; set; }

    private string? name;
    private string? folder;
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
    public string? Name
    {
        get => name;
        set
        {
            name = value;
            folder = name?.Replace("_", "");
        }
    }

    /// <summary>
    /// Gets or sets the size of the icon. Defaults to 20. Not all sizes are available for all icons
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IconSize Size { get; set; } = IconSize.Size24;

    [Parameter]
    public string? Slot { get; set; } = null;

    /// <summary>
    /// Gets or sets a collection of additional attributes that will be applied to the input element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? AdditionalAttributes { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        if (IconService is not null)
        {
            string t = await IconService.HttpClient.GetStringAsync($"_content/Microsoft.Fast.Components.FluentUI/icons/{folder}/{ComposedName}.svg");

            if (UseAccentColor)
                t = t.Replace("<path ", "<path fill=\"var(--accent-fill-rest)\"");
            if (!string.IsNullOrEmpty(Slot))
                t = t.Replace("<svg ", $"<svg slot=\"{Slot}\" ");

            svg = (MarkupString)t;
        }
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
