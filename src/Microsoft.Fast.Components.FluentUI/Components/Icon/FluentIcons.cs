namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentIcons
{
    public static List<IconModel> LibraryUsedIcons = new()
    {
        new IconModel("ArrowSortUp", IconSize.Size20, IconVariant.Regular),
        new IconModel("ArrowSortDown", IconSize.Size20, IconVariant.Regular),
        new IconModel("ChevronDoubleLeft", IconSize.Size20, IconVariant.Regular),
        new IconModel("ChevronDoubleRight", IconSize.Size20, IconVariant.Regular),
        new IconModel("ChevronLeft", IconSize.Size20, IconVariant.Regular),
        new IconModel("ChevronRight", IconSize.Size20, IconVariant.Regular),
        new IconModel("Filter", IconSize.Size20, IconVariant.Regular),
        new IconModel("Info", IconSize.Size20, IconVariant.Regular),
        new IconModel("Info", IconSize.Size32, IconVariant.Regular)
    };

    private static readonly ushort SizesCount = (ushort)Enum.GetValues(typeof(IconSize)).Length;
    public static readonly Dictionary<string, uint> IconsAvailability;

    static FluentIcons()
    {
        IconsAvailability =
            FullIconMap
                .GroupBy(x => x.Name)
                .ToDictionary(
                    g => g.Key,
                    g =>
                    {
                        uint mask = 0;
                        foreach (var icon in g)
                        {
                            mask |= GetAvailabilityBitMask(icon.Size, icon.Variant);
                        }

                        return mask;
                    });
    }

    public static IEnumerable<IconModel> GetIconMap(IconSize[] sizes, IconVariant[] variants)
    {
        return FullIconMap.Where(x => sizes.Contains(x.Size) && variants.Contains(x.Variant));
    }

    public static bool IconAvailable(IconConfiguration configuration, FluentIcon icon)
    {
        if (configuration.PublishedAssets)
            return configuration.Variants.Contains(icon.Variant) &&
                   configuration.Sizes.Contains(icon.Size) &&
                   IconsAvailability.TryGetValue(icon.Name, out var iconAvailability) &&
                   (iconAvailability & GetAvailabilityBitMask(icon.Size, icon.Variant)) != 0;
        else
            return LibraryUsedIcons.Any(x => x.Name == icon.Name && x.Size == icon.Size && x.Variant == icon.Variant);
    }

    private static uint GetAvailabilityBitMask(IconSize size, IconVariant variant)
    {
        return 1U << GetAvailabilityBitPosition(size, variant);
    }

    private static int GetAvailabilityBitPosition(IconSize size, IconVariant variant)
    {
        var variantOffset = (int)variant * SizesCount;
        var sizeOffset = size switch
        {
            IconSize.Size10 => 0,
            IconSize.Size12 => 1,
            IconSize.Size16 => 2,
            IconSize.Size20 => 3,
            IconSize.Size24 => 4,
            IconSize.Size28 => 5,
            IconSize.Size32 => 6,
            IconSize.Size48 => 7,
            _ => throw new ArgumentOutOfRangeException(nameof(size), size, null)
        };

        return variantOffset + sizeOffset;
    }
}
