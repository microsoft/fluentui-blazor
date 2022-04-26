using System.Drawing;

namespace Microsoft.Fast.Components.FluentUI
{
    public static class ColorHelpers
    {
        public static System.Drawing.Color ToColor(this string s)
        {
            return ColorTranslator.FromHtml(s);
        }
    }
}
