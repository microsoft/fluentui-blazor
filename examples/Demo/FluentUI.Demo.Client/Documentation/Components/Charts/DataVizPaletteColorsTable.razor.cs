// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Charts;

namespace FluentUI.Demo.Client.Documentation.Components.Charts;

public partial class DataVizPaletteColorsTable
{
    private static IReadOnlyList<DataVizPaletteColorRow> Colors { get; } =
        Enum.GetValues<DataVizPalette>()
            .Where(x => x != DataVizPalette.Custom)
            .Select(x => new DataVizPaletteColorRow(
                x.ToString(),
                x.ToDataVizPaletteHex(),
                x.ToDataVizPaletteHex(isDarkTheme: true),
                x.ToDataVizPaletteHex() != x.ToDataVizPaletteHex(isDarkTheme: true)))
            .ToArray();

    private sealed record DataVizPaletteColorRow(string Name, string LightValue, string DarkValue, bool HasDarkVariant);
}
