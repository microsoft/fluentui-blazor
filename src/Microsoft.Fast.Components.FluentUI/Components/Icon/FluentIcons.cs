namespace Microsoft.Fast.Components.FluentUI;

public static partial class FluentIcons
{
    public static List<IconModel> GetFilteredIcons(IconSize size, IconVariant? variant)
    {
        List<IconModel> data = new();

        if (!variant.HasValue)
            data = IconMap.Where(x => x.Size == size)
                .OrderBy(i => i.Name)
                .ToList();
        else
            data = IconMap.Where(x => x.Size == size && x.Variant == variant)
                .OrderBy(i => i.Name)
                .ToList();

        return data;
    }

    public static List<IconModel> GetFilteredIcons(string searchterm, IconSize size, IconVariant? variant)
    {
        List<IconModel> data = new();
        if (!string.IsNullOrWhiteSpace(searchterm))
        {
            if (!variant.HasValue)
                data = IconMap.Where(x => (x.Name.Contains(searchterm.ToLower()) || x.Folder.Contains(searchterm.ToLower(), StringComparison.InvariantCultureIgnoreCase)) && x.Size == size)
                    .OrderBy(i => i.Name)
                    .ToList();
            else
                data = IconMap.Where(x => (x.Name.Contains(searchterm.ToLower()) || x.Folder.Contains(searchterm.ToLower(), StringComparison.InvariantCultureIgnoreCase)) && x.Size == size && x.Variant == variant)
                    .OrderBy(i => i.Name)
                    .ToList();
        }

        return data;
    }
}
