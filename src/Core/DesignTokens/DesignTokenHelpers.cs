using System.Drawing;

namespace Microsoft.FluentUI.AspNetCore.Components.DesignTokens;

public static class DesignTokenHelpers
{
    public static System.Drawing.Color ToColor(this string s) => ColorTranslator.FromHtml(s);

    public static Swatch ToSwatch(this string s) => new(s);

}
