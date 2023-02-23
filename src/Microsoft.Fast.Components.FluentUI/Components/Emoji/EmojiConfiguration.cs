namespace Microsoft.Fast.Components.FluentUI;

public class EmojiConfiguration
{
    public bool PublishedAssets { get; init; } = false;

    private EmojiGroup[] _groups = Array.Empty<EmojiGroup>();

    private EmojiStyle[] _styles = Array.Empty<EmojiStyle>();

    internal event Action? OnUpdate;

    public EmojiGroup[] Groups
    {
        get => _groups;
        set
        {
            _groups = value;
            OnUpdate?.Invoke();
        }
    }

    public EmojiStyle[] Styles
    {
        get => _styles;
        set
        {
            _styles = value;
            OnUpdate?.Invoke();
        }
    }

    public EmojiConfiguration()
    {

    }

    public EmojiConfiguration(bool publishedAssets)
    {
        PublishedAssets = publishedAssets;
    }
}
