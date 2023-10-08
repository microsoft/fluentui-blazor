using System.Text.Json;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

public class Theme
{
    public StandardLuminance SelectedTheme { get; set; } = StandardLuminance.LightMode;
    public string SelectedAccentColor { get; set; } = OfficeColor.Default.GetDescription()!;
    public LocalizationDirection SelectedDirection { get; set; } = LocalizationDirection.rtl;

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }

    public static Theme? FromJson(string json)
    {
        return JsonSerializer.Deserialize<Theme>(json);
    }
}
