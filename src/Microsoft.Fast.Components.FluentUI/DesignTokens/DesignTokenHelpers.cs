using System.Drawing;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

public static class DesignTokenHelpers
{
    public static System.Drawing.Color ToColor(this string s) => ColorTranslator.FromHtml(s);

    public static Swatch ToSwatch(this string s) => new Swatch(s);


}
