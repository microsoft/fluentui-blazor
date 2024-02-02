namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The standard luminance values for light and dark mode.
/// </summary>
public enum StandardLuminance
{
    /// <summary>
    /// Light mode luminance
    /// </summary>
    LightMode,

    /// <summary>
    /// Dark mode luminance
    /// </summary>
    DarkMode
}

public static class StandardLuminanceExtensions
{
    private const float _lightmode = 0.98f;
    private const float _darkmode = 0.15f;

    public static float GetLuminanceValue(this StandardLuminance value)
    {
        return value == StandardLuminance.LightMode ? _lightmode : _darkmode;
    }
}
