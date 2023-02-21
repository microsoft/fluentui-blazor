using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class FluentIconSearcher
{
    [Inject]
    private IconService IconService { get; set; } = default!;

    private IEnumerable<IconModel>? iconList;


    public FluentIconSearcher(IconService iconService)
    {
        IconService = iconService;
    }

    private IEnumerable<IconModel>? GetFilteredIconList()
    {
        return FluentIcons.GetIconMap(IconService.Configuration.Sizes, IconService.Configuration.Variants); ;
    }


    public FluentIconSearcher AsVariant(IconVariant? variant)
    {
        if (variant.HasValue)
        {
            iconList ??= GetFilteredIconList();

            iconList = iconList!.Where(x => x.Variant == variant);

        }
        return this;
    }


    public FluentIconSearcher WithSize(IconSize? size)
    {
        if (size.HasValue)
        {
            iconList ??= GetFilteredIconList();

            iconList = iconList!.Where(x => x.Size == size);

        }
        return this;
    }


    public FluentIconSearcher WithName(string? searchterm)
    {

        if (!string.IsNullOrWhiteSpace(searchterm) && searchterm.Length >= 2)
        {
            iconList ??= GetFilteredIconList();

            iconList = iconList!.Where(x => x.Name.Contains(searchterm, StringComparison.OrdinalIgnoreCase));
        }

        return this;
    }

    public List<IconModel>? ToList(int count = 50)
    {
        if (iconList is null)
            return new();
        else
            return iconList?.Take(count).OrderBy(x => x.Name).ToList();
    }

    public int ResultCount()
    {
        if (iconList is null)
            return 0;
        else
            return iconList.Count();
    }
}