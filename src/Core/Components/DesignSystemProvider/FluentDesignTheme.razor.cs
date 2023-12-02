using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentDesignTheme : ComponentBase
{
    /// <summary>
    /// Gets or sets the Theme mode: Dark, Light, or browser System theme.
    /// </summary>
    [Parameter]
    public DesignThemeModes Mode { get; set; } = DesignThemeModes.System;

    /// <summary>
    /// Gets or sets the Accent base color.
    /// </summary>
    [Parameter]
    public string? CustomColor { get; set; }

    /// <summary>
    /// Gets or sets the application to defined the Accent base color.
    /// </summary>
    [Parameter]
    public OfficeColor? OfficeColor { get; set; }

    /// <summary />
    private string? GetColor()
    {
        if (CustomColor != null)
        {
            return CustomColor;
        }

        if (OfficeColor != null && OfficeColor.HasValue)
        {
            return Enum.GetName(OfficeColor.Value);
        }

        return null;
    }

    private string? GetMode()
    {
        return Mode switch
        {
            DesignThemeModes.Dark => "dark",
            DesignThemeModes.Light => "light",
            _ => null
        };
    }
}