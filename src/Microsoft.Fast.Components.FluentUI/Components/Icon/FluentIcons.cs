namespace Microsoft.Fast.Components.FluentUI;

public static partial class FluentIcons
{
    public static List<IconModel> GetFilteredIcons(IconSize size, IconVariant? variant)
    {
        List<IconModel> data = new();

        if (!variant.HasValue)
            data = IconMap.Where(x => x.Size == size).ToList();
        else
            data = IconMap.Where(x => x.Size == size && x.Variant == variant).ToList();

        return data;
    }

    public static List<IconModel> GetFilteredIcons(string searchterm, IconSize size, IconVariant? variant)
    {
        List<IconModel> data = new();
        if (!string.IsNullOrWhiteSpace(searchterm))
        {
            if (!variant.HasValue)
                data = IconMap.Where(x => x.Name.Contains(searchterm.ToLower()) && x.Size == size).ToList();
            else
                data = IconMap.Where(x => x.Name.Contains(searchterm.ToLower()) && x.Size == size && x.Variant == variant).ToList();
        }

        return data;
    }
}
