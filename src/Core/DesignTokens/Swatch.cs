using System.Drawing;

namespace Microsoft.FluentUI.AspNetCore.Components.DesignTokens;

public class Swatch
{
    public float R { get; set; }
    public float G { get; set; }
    public float B { get; set; }
    public float RelativeLuminance { get; set; }

    private readonly System.Drawing.Color Color;

    public Swatch()
    {
        R = 0;
        G = 0;
        B = 0;
        RelativeLuminance = 0;
    }

    public Swatch(string s)
    {
        Color = ColorTranslator.FromHtml(s);
        R = Normalize(Color.R, 0, 255);
        G = Normalize(Color.G, 0, 255);
        B = Normalize(Color.B, 0, 255);

        RelativeLuminance = RgbToRelativeLuminance();
    }

    public Swatch(byte red, byte green, byte blue)
    {
        R = Normalize(red, 0, 255);
        G = Normalize(green, 0, 255);
        B = Normalize(blue, 0, 255);

        Color = System.Drawing.Color.FromArgb(red, green, blue);

        RelativeLuminance = RgbToRelativeLuminance();
    }

    public Swatch(System.Drawing.Color color)
    {
        Color = color;
        R = Normalize(Color.R, 0, 255);
        G = Normalize(Color.G, 0, 255);
        B = Normalize(Color.B, 0, 255);

        RelativeLuminance = RgbToRelativeLuminance();
    }

    private float RgbToRelativeLuminance() => RgbToLinearLuminance(
            LuminanceHelper(R),
            LuminanceHelper(G),
            LuminanceHelper(B)
        );

    private static float RgbToLinearLuminance(float r, float g, float b) => (r * 0.2126f) + (g * 0.7152f) + (b * 0.0722f);

    private static float LuminanceHelper(float i)
    {
        if (i <= 0.03928)
        {
            return i / 12.92f;
        }
        return (float)Math.Pow((i + 0.055f) / 1.055f, 2.4f);
    }

    private static float Normalize(float value, int min, int max)
    {
        if (value <= min)
        {
            return 0.0f;
        }

        if (value >= max)
        {
            return 1.0f;
        }

        return value / (max - min);
    }

    private static float Denormalize(float value, int min, int max) => min + (value * (max - min));
}
