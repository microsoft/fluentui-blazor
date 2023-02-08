namespace Microsoft.Fast.Components.FluentUI;

public class IconConfiguration
{
    public bool PublishedAssets { get; init; } = false;

    private IconSize[] _sizes = Array.Empty<IconSize>();
    private IconVariant[] _variants = Array.Empty<IconVariant>();

    internal event Action? OnUpdate;

    public IconSize[] Sizes
    {
        get => _sizes;
        set
        {
            _sizes = value;
            OnUpdate?.Invoke();
        }
    }

    public IconVariant[] Variants
    {
        get => _variants;
        set
        {
            _variants = value;
            OnUpdate?.Invoke();
        }
    }

    public IconConfiguration()
    {

    }

    public IconConfiguration(bool publishedAssets)
    {
        PublishedAssets = publishedAssets;
    }
}
