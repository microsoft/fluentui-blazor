namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentEmojis
{
    public static IEnumerable<EmojiModel> GetEmojiMap(EmojiGroup[] groups, EmojiStyle[] styles)
    {
        return FullEmojiMap.Where(x => groups.Contains(x.Group) && styles.Contains(x.Style));
    }
}
