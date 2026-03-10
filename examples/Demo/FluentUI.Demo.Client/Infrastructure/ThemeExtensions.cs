// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Client;

public static class ThemeExtensions
{
    public static object ToJsThemeSettings(this ThemeSettings settings)
        => new
        {
            color = settings.Color,
            hueTorsion = settings.HueTorsion,
            vibrancy = settings.Vibrancy,
            mode = settings.Mode switch
            {
                ThemeMode.Dark => "dark",
                ThemeMode.Light => "light",
                _ => "system",
            },
            isExact = settings.IsExact,
        };
}
