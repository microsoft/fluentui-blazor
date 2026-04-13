// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

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

    private static readonly IReadOnlyList<string> WheelColors =
    [
        "#003366", "#336699", "#3366CC", "#003399", "#000099", "#0000CC", "#000066",
        "#000066", "#000066", "#0099CC", "#0066CC", "#0066CC", "#0066CC", "#0066CC", "#0066CC",
        "#669999", "#009999", "#33CCCC", "#00CCFF", "#0099FF", "#0066FF", "#3366FF", "#3333CC", "#666699",
        "#339966", "#00CC99", "#00FFCC", "#00FFFF", "#33CCFF", "#3399FF", "#6699FF", "#6666FF", "#6600FF", "#6600CC",
        "#339933", "#00CC66", "#00FF99", "#66FFCC", "#66FFFF", "#66CCFF", "#99CCFF", "#9999FF", "#9966FF", "#9933FF", "#9900FF",
        "#006600", "#00CC00", "#00FF00", "#66FF99", "#99FFCC", "#CCFFFF", "#CCCCFF", "#CC99FF", "#CC66FF", "#CC33FF", "#CC00FF", "#9900CC",
        "#003300", "#009933", "#33CC33", "#66FF66", "#99FF99", "#CCFFCC", "#FFFFFF", "#FFCCFF", "#FF99FF", "#FF66FF", "#FF00FF", "#CC00CC", "#660066",
        "#336600", "#009900", "#66FF33", "#99FF66", "#CCFF99", "#FFFFCC", "#FFCCCC", "#FF99CC", "#FF66CC", "#FF33CC", "#CC0099", "#993399",
        "#333300", "#669900", "#99FF33", "#CCFF66", "#FFFF99", "#FFCC99", "#FF9999", "#FF6699", "#FF3399", "#CC3399", "#990099",
        "#666633", "#99CC00", "#CCFF33", "#FFFF66", "#FFCC66", "#FF9966", "#FF6666", "#FF0066", "#CC6699", "#993366",
        "#999968", "#CDCD07", "#FFFF04", "#FFCD05", "#FF9B37", "#FF6B09", "#FF5454", "#CD0569", "#690638",
        "#A07243", "#D0A218", "#FFA216", "#D17519", "#FF4719", "#FF1818", "#D01414", "#A21645",
        "#704010", "#EAE0CB", "#D35126", "#A85126", "#A82525", "#942828", "#A75050",
    ];
}
