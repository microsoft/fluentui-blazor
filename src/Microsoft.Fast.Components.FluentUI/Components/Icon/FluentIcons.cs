
namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentIcons
{
    public static IEnumerable<IconModel> GetIconMap(IconSize[] sizes, IconVariant[] variants)
    {
        return FullIconMap.Where(x => sizes.Contains(x.Size) && variants.Contains(x.Variant));
    }
}
