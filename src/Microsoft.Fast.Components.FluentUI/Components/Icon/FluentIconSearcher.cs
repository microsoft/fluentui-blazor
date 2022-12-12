namespace Microsoft.Fast.Components.FluentUI;

public class FluentIconSearcher
{
    private IEnumerable<IconModel>? iconList;

    public FluentIconSearcher AsVariant(IconVariant? variant)
    {
        if (variant.HasValue)
        {
            iconList ??= FluentIcons.IconMap;

            iconList = iconList!.Where(x => x.Variant == variant);

        }
        return this;
    }


    public FluentIconSearcher WithSize(IconSize? size)
    {
        if (size.HasValue)
        {
            iconList ??= FluentIcons.IconMap;

            iconList = iconList!.Where(x => x.Size == size);

        }
        return this;
    }

    
    public FluentIconSearcher WithName(string? searchterm)
    {

        if (!string.IsNullOrWhiteSpace(searchterm) && searchterm.Length > 2)
        {
            iconList ??= FluentIcons.IconMap;

            iconList = iconList!.Where(x => x.Name.Contains(searchterm, StringComparison.OrdinalIgnoreCase) || x.Folder.Contains(searchterm.ToLower(), StringComparison.OrdinalIgnoreCase));
        }

        return this;
    }

    public List<IconModel>? ToList(int count = 50)
    {
        if (iconList is null)
            return new List<IconModel>();
        else
            return iconList?.Take(count).ToList();
    }
}