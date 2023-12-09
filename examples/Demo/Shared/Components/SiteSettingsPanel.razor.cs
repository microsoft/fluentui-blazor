using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Components;

public partial class SiteSettingsPanel
{
    public DesignThemeModes Mode { get; set; }

    public OfficeColor? OfficeColor { get; set; }

    public bool Direction { get; set; } = true;

    private IEnumerable<DesignThemeModes> AllModes => Enum.GetValues<DesignThemeModes>();

    private IEnumerable<OfficeColor?> AllOfficeColors
    {
        get
        {
            return Enum.GetValues<OfficeColor>().Select(i => (OfficeColor?)i).Union(new[] { (OfficeColor?)null });
        }
    }
}