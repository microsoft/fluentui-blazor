namespace Microsoft.Fast.Components.FluentUI;

public class IconConfiguration
{
    public bool PublishAssets { get; init; } = true;

    private IconSize[] _sizes = new[]
    {
        IconSize.Size10,
        IconSize.Size12,
        IconSize.Size16,
        IconSize.Size20,
        IconSize.Size24,
        IconSize.Size28,
        IconSize.Size32,
        IconSize.Size48
    };

    private IconVariant[] _variants = new[]
    {
        IconVariant.Filled,
        IconVariant.Regular
    };


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

    public IconConfiguration(bool publishAssets)
    {
        PublishAssets = publishAssets;
        if (PublishAssets is false)
        {
            _sizes = Array.Empty<IconSize>();
            _variants = Array.Empty<IconVariant>();
        }
    }
}
