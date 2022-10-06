namespace Microsoft.Fast.Components.FluentUI;

public static partial class FluentIcons
{
    public static List<IconModel> GetFilteredIcons(IconSize size, bool? filled)
    {
        List<IconModel> data = new();

        if (filled == null)
            data = IconMap.Where(x => x.Size == size).ToList();
        else
            data = IconMap.Where(x => x.Size == size && x.Filled == filled).ToList();

        return data;
    }

    public static List<IconModel> GetFilteredIcons(string searchterm, IconSize size, bool? filled)
    {
        List<IconModel> data = new();
        if (!string.IsNullOrWhiteSpace(searchterm))
        {
            if (filled == null)
                data = IconMap.Where(x => x.Name.Contains(searchterm.ToLower()) && x.Size == size).ToList();
            else
                data = IconMap.Where(x => x.Name.Contains(searchterm.ToLower()) && x.Size == size && x.Filled == filled).ToList();
        }

        return data;
    }
}
