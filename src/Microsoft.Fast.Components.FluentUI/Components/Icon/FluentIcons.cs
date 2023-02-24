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
        new IconModel("Filter", IconSize.Size20, IconVariant.Regular)
    };

    public static IEnumerable<IconModel> GetIconMap(IconSize[] sizes, IconVariant[] variants)
    {
        return FullIconMap.Where(x => sizes.Contains(x.Size) && variants.Contains(x.Variant));
    }

    public static bool IconAvailable(IconConfiguration configuration, FluentIcon icon)
    {
        if (configuration.PublishedAssets)
            return GetIconMap(configuration.Sizes, configuration.Variants).Any(x => x.Name == icon.Name && x.Size == icon.Size && x.Variant == icon.Variant);
        else
            return LibraryUsedIcons.Any(x => x.Name == icon.Name && x.Size == icon.Size && x.Variant == icon.Variant);

    }
}
