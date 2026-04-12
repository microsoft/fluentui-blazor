// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The ColorPicker component is used to select a color from a palette.
/// </summary>
public partial class FluentColorPicker : FluentComponentBase
{
    /// <summary />
    public FluentColorPicker(LibraryConfiguration configuration) : base(configuration)
    {
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-color-picker")
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("width", Width)
        .AddStyle("height", Height)
        .Build();

    /// <summary>
    /// Gets or sets the view of the color picker.
    /// </summary>
    [Parameter]
    public ColorPickerView View { get; set; } = ColorPickerView.SwatchPalette;

    /// <summary>
    /// Gets or sets the orientation of the color items.
    /// Default is <see cref="Orientation.Vertical"/>.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Vertical;

    /// <summary>
    /// Gets or sets the width of the color picker.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }
    
    /// <summary>
    /// Gets or sets the height of the color picker.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the currently selected color.
    /// </summary>
    [Parameter]
    public string? SelectedColor { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the selected color changes.
    /// </summary>
    [Parameter]
    public EventCallback<string> SelectedColorChanged { get; set; }

    /// <summary>
    /// Gets or sets the icon displayed for the selected color.
    /// </summary>
    [Parameter]
    public Icon SelectedIcon { get; set; } = new CoreIcons.Regular.Size20.Checkmark();

    // -------------------------------------------------------------------------
    // Color Wheel
    // -------------------------------------------------------------------------

    private record WheelHexData(string Points, string Color, string CxStr, string CyStr);

    private static readonly IReadOnlyList<string> WheelColors =
    [
        "#008080", "#00806D", "#008057", "#008040", "#008029", "#008014", "#008000",
        "#006D80", "#199595", "#19957F", "#199565", "#19954A", "#199530", "#199519",
        "#148000", "#005780", "#197F95", "#39ABAA", "#39AB90", "#39AB72", "#39AB53",
        "#39AB39", "#309519", "#298000", "#004080", "#196595", "#3990AB", "#60C0C0",
        "#60C0A1", "#60C07F", "#60C060", "#53AB39", "#4A9519", "#408000", "#002980",
        "#194A95", "#3972AB", "#60A1C0", "#8ED5D5", "#8ED5B2", "#8ED58E", "#7FC060",
        "#72AB39", "#659519", "#578000", "#001480", "#193095", "#3953AB", "#607FC0",
        "#8EB2D5", "#C3EAEA", "#C3EAC3", "#B2D58E", "#A1C060", "#90AB39", "#7F9519",
        "#6D8000", "#000080", "#191995", "#3939AB", "#6060C0", "#8E8ED5", "#C3C3EA",
        "#FFFFFF", "#EAEAC3", "#D5D58E", "#C0C060", "#ABAB39", "#959519", "#808000",
        "#140080", "#301995", "#5339AB", "#7F60C0", "#B28ED5", "#EAC3EA", "#EAC3C3",
        "#D5B28E", "#C0A160", "#AB9039", "#957F19", "#806D00", "#290080", "#4A1995",
        "#7239AB", "#A160C0", "#D58ED5", "#D58EB2", "#D58E8E", "#C07F60", "#AB7239",
        "#956519", "#805700", "#400080", "#651995", "#9039AB", "#C060C0", "#C060A1",
        "#C0607F", "#C06060", "#AB5339", "#954A19", "#804000", "#570080", "#7F1995",
        "#AB39AB", "#AB3990", "#AB3972", "#AB3953", "#AB3939", "#953019", "#802900",
        "#6D0080", "#951995", "#95197F", "#951965", "#95194A", "#951930", "#951919",
        "#801400", "#800080", "#80006D", "#800057", "#800040", "#800029", "#800014",
        "#800000",
    ];

    private static readonly IReadOnlyList<WheelHexData> WheelHexagons =
        [.. GenerateColorWheelHexagons(rings: 6, hexSize: 14.5)];

    private WheelHexData? SelectedWheelHex =>
        string.IsNullOrEmpty(SelectedColor)
            ? null
            : WheelHexagons.FirstOrDefault(h => IsSelectedColor(h.Color));

    /// <summary>
    /// Generates the hexagonal grid for the color wheel.
    /// Uses flat-top hexagons in axial (q, r) coordinates.
    /// Colors are sourced from the <see cref="WheelColors"/> array.
    /// </summary>
    private static IEnumerable<WheelHexData> GenerateColorWheelHexagons(int rings, double hexSize)
    {
        var sqrt3 = Math.Sqrt(3.0);
        var polySize = hexSize - 0.5; // slight gap between hexagons
        var inv = System.Globalization.CultureInfo.InvariantCulture;
        var colorIndex = 0;

        for (var q = -rings; q <= rings; q++)
        {
            var rMin = Math.Max(-rings, -q - rings);
            var rMax = Math.Min(rings, -q + rings);

            for (var r = rMin; r <= rMax; r++)
            {
                // Flat-top hex grid: center pixel position
                var cx = hexSize * 1.5 * q;
                var cy = hexSize * sqrt3 * (r + q / 2.0);

                // Build SVG polygon points for a flat-top hexagon (vertex i at 60°*i)
                var sb = new StringBuilder();
                for (var i = 0; i < 6; i++)
                {
                    var angleRad = Math.PI / 3.0 * i;
                    var vx = cx + polySize * Math.Cos(angleRad);
                    var vy = cy + polySize * Math.Sin(angleRad);
                    if (i > 0)
                    {
                        sb.Append(' ');
                    }

                    sb.Append(vx.ToString("F2", inv));
                    sb.Append(',');
                    sb.Append(vy.ToString("F2", inv));
                }

                yield return new WheelHexData(
                    Points: sb.ToString(),
                    Color: WheelColors[colorIndex++],
                    CxStr: cx.ToString("F2", inv),
                    CyStr: cy.ToString("F2", inv));
            }
        }
    }

    private async Task ColorSelectHandlerAsync(string color)
    {
        if (!string.Equals(SelectedColor, color, StringComparison.OrdinalIgnoreCase))
        {
            SelectedColor = color;

            if (SelectedColorChanged.HasDelegate)
            {
                await SelectedColorChanged.InvokeAsync(color);
            }
        }
    }

    /// <summary>
    /// Determines whether the specified color is the currently selected color.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    private bool IsSelectedColor(string color)
    {
        return string.Equals(SelectedColor, color, StringComparison.OrdinalIgnoreCase);
    }

    private static readonly IReadOnlyList<string> DefaultColors =
    [
        // Red
        "#FFE4E9", "#FFCDD2", "#EE9A9A", "#E57373", "#EE534F",
        "#F44236", "#E53935", "#C9342D", "#C32C28", "#B61C1C",
        // Rose
        "#FFD2E7", "#F9BBD0", "#F48FB1", "#F06292", "#EC407A",
        "#EA1E63", "#D81A60", "#C2175B", "#AD1457", "#890E4F",
        // Mauve
        "#F8D5FF", "#E1BEE8", "#CF93D9", "#B968C7", "#AA47BC",
        "#9C28B1", "#8E24AA", "#7A1FA2", "#6A1B9A", "#4A148C",
        // Violet
        "#E7DBFF", "#D0C4E8", "#B39DDB", "#9675CE", "#7E57C2",
        "#673BB7", "#5D35B0", "#512DA7", "#45289F", "#301B92",
        // Bleu foncé
        "#DCE1FF", "#C5CAE8", "#9EA8DB", "#7986CC", "#5C6BC0",
        "#3F51B5", "#3949AB", "#303E9F", "#283593", "#1A237E",
        // Bleu
        "#D2F5FF", "#BBDEFA", "#90CAF8", "#64B5F6", "#42A5F6",
        "#2196F3", "#1D89E4", "#1976D3", "#1564C0", "#0E47A1",
        // Cyan
        "#CAFCFF", "#B3E5FC", "#81D5FA", "#4FC2F8", "#28B6F6",
        "#03A9F5", "#039BE6", "#0288D1", "#0277BD", "#00579C",
        // Bleu-Vert
        "#C9FFFF", "#B2EBF2", "#80DEEA", "#4DD0E2", "#25C6DA",
        "#00BCD5", "#00ACC2", "#0098A6", "#00828F", "#016064",
        // Bleu-vert foncé
        "#C9F6F3", "#B2DFDC", "#80CBC4", "#4CB6AC", "#26A59A",
        "#009788", "#00887A", "#00796A", "#00695B", "#004C3F",
        // Vert
        "#DFFDE1", "#C8E6CA", "#A5D6A7", "#80C783", "#66BB6A",
        "#4CB050", "#43A047", "#398E3D", "#2F7D32", "#1C5E20",
        // Green-Yellow
        "#F4FFDF", "#DDEDC8", "#C5E1A6", "#AED582", "#9CCC66",
        "#8BC24A", "#7DB343", "#689F39", "#548B2E", "#33691E",
        // Green-Yellow-Light
        "#FFFFD9", "#F0F4C2", "#E6EE9B", "#DDE776", "#D4E056",
        "#CDDC39", "#C0CA33", "#B0B42B", "#9E9E24", "#817716",
        // Yellow
        "#FFFFDA", "#FFFAC3", "#FFF59C", "#FFF176", "#FFEE58",
        "#FFEB3C", "#FDD734", "#FAC02E", "#F9A825", "#F47F16",
        // Yellow-Orange
        "#FFFFC9", "#FFECB2", "#FFE083", "#FFD54F", "#FFC928",
        "#FEC107", "#FFB200", "#FF9F00", "#FF8E01", "#FF6F00",
        // Orange
        "#FFF7C9", "#FFE0B2", "#FFCC80", "#FFB64D", "#FFA827",
        "#FF9700", "#FB8C00", "#F67C01", "#EF6C00", "#E65100",
        // Orange Dark
        "#FFE3D2", "#FFCCBB", "#FFAB91", "#FF8A66", "#FF7143",
        "#FE5722", "#F5511E", "#E64A19", "#D74315", "#BF360C",
        // Marron
        "#EEE3DF", "#D7CCC8", "#BCABA4", "#A0887E", "#8C6E63",
        "#7B5347", "#6D4D42", "#5D4038", "#4D342F", "#3E2622",
        // Grey
        "#FFFFFF", "#F5F5F5", "#EEEEEE", "#E0E0E0", "#BDBDBD",
        "#9E9E9E", "#757575", "#616161", "#424242", "#212121",
        // Bleu gris
        "#E5F0F4", "#CED9DD", "#B0BFC6", "#90A4AD", "#798F9A",
        "#607D8B", "#546F7A", "#465A65", "#36474F", "#273238"
    ];
}
