namespace FluentUI.Demo.Shared;

public class FluentEmojiSearcher
{
    private IEnumerable<EmojiModel>? emojiList;

    public FluentEmojiSearcher InGroup(EmojiGroup? group)
    {
        if (group.HasValue)
        { 
            emojiList ??= FluentEmojis.EmojiMap;

            emojiList = emojiList!.Where(x => x.Group == group);
                
        }
        return this;
    }

    public FluentEmojiSearcher WithStyle(EmojiStyle? style)
    {
        if (style.HasValue)
        {
            emojiList ??= FluentEmojis.EmojiMap;

            emojiList = emojiList!.Where(x => x.Style == style);
                
        }

        return this;
    }

    public FluentEmojiSearcher WithSkintone(EmojiSkintone? skintone)
    {
        if (skintone.HasValue)
        {
            emojiList ??= FluentEmojis.EmojiMap;

            emojiList = emojiList!.Where(x => x.Skintone == skintone);
        }

        return this;
    }

    public FluentEmojiSearcher WithName(string? searchterm)
    {

        if (!string.IsNullOrWhiteSpace(searchterm) && searchterm.Length >= 2)
        {
            emojiList ??= FluentEmojis.EmojiMap;
            
            emojiList = emojiList!.Where(x => x.Name.Contains(searchterm, StringComparison.OrdinalIgnoreCase) || x.Folder.Contains(searchterm.ToLower()));
        }

        return this;
    }

    public List<EmojiModel>? ToList(int count = 50)
    {
        if (emojiList is null)
            return new List<EmojiModel>();
        else
            return emojiList?.Take(count).OrderBy(x => x.Folder).ToList();
    }

    public int ResultCount()
    {
        if (emojiList is null)
            return 0;
        else
            return emojiList.Count();
    }
}
