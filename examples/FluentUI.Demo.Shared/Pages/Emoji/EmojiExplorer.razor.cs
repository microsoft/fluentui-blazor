using Microsoft.Fast.Components.FluentUI;

namespace FluentUI.Demo.Shared.Pages.Emoji;

public partial class EmojiExplorer
{
    private const int MAX_ICONS = 200;
    private bool SearchInProgress = false;
    private SearchCriteria Criteria = new();
    private IEnumerable<EmojiInfo> EmojisFound = Array.Empty<EmojiInfo>();
    private int EmojisCount = 0;

    private async Task HandleSearchField()
    {
        SearchInProgress = true;
        await Task.Delay(1);

        var emojis = Emojis.AllEmojis
                           .Where(i => i.Name.Contains(Criteria.SearchTerm, StringComparison.InvariantCultureIgnoreCase)
                                    && (Criteria.Group != null ? i.Group == Criteria.Group : true)
                                    && (Criteria.Style != null ? i.Style == Criteria.Style : true));

        EmojisCount = emojis.Count();
        EmojisFound = emojis.Take(MAX_ICONS).ToArray();

        SearchInProgress = false;
        await Task.Delay(1);
    }

    private IEnumerable<TEnum?> GetListWithNullable<TEnum>()
        where TEnum : struct, Enum
    {
        var values = Enum.GetValues<TEnum>().Cast<TEnum?>().ToList();
        // values.Insert(0, null); // TODO: Uncomment when "ListComponentBase => item!.GetType()" will be fixed.
        return values;
    }

    private class SearchCriteria
    {
        public string SearchTerm { get; set; } = string.Empty;
        public EmojiGroup? Group { get; set; } = EmojiGroup.Objects;
        public EmojiStyle? Style { get; set; } = EmojiStyle.Color;
    }
}
