namespace Microsoft.Fast.Components.FluentUI;

public class IconConfiguration
{
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

}
