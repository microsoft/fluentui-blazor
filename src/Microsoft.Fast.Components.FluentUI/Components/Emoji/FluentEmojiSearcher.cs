using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class FluentEmojiSearcher
{
    [Inject]
    private EmojiService EmojiService { get; set; } = default!;

    private IEnumerable<EmojiModel>? _emojiList;



    public FluentEmojiSearcher(EmojiService emojiService)
    {
        EmojiService = emojiService;
    }

    private IEnumerable<EmojiModel>? GetFilteredEmojiList()
    {
        return FluentEmojis.GetEmojiMap(EmojiService.Configuration.Groups, EmojiService.Configuration.Styles); ;
    }

    public FluentEmojiSearcher InGroup(EmojiGroup? group)
    {
        if (group.HasValue)
        {
            _emojiList ??= GetFilteredEmojiList();

            _emojiList = _emojiList!.Where(x => x.Group == group);

        }
        return this;
    }

    public FluentEmojiSearcher WithStyle(EmojiStyle? style)
    {
        if (style.HasValue)
        {
            _emojiList ??= GetFilteredEmojiList();

            _emojiList = _emojiList!.Where(x => x.Style == style);

        }

        return this;
    }

    public FluentEmojiSearcher WithSkintone(EmojiSkintone? skintone)
    {
        if (skintone.HasValue)
        {
            _emojiList ??= GetFilteredEmojiList();

            _emojiList = _emojiList!.Where(x => x.Skintone == skintone);
        }

        return this;
    }

    public FluentEmojiSearcher WithName(string? searchterm)
    {

        if (!string.IsNullOrWhiteSpace(searchterm) && searchterm.Length >= 2)
        {
            _emojiList ??= GetFilteredEmojiList();

            _emojiList = _emojiList!.Where(x => x.Name.Contains(searchterm, StringComparison.OrdinalIgnoreCase) ||
                                                         x.Keywords.Contains(searchterm, StringComparison.OrdinalIgnoreCase) ||
                                                         x.Folder.Contains(searchterm.ToLower()));
        }

        return this;
    }

    public List<EmojiModel>? ToList(int count = 50)
    {
        if (_emojiList is null)
            return new List<EmojiModel>();
        else
            return _emojiList?.Take(count).OrderBy(x => x.Folder).ToList();
    }

    public int ResultCount()
    {
        if (_emojiList is null)
            return 0;
        else
            return _emojiList.Count();
    }
}
