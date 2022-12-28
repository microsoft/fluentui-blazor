namespace Microsoft.Fast.Components.FluentUI.Infrastructure;

public class StaticAssetServiceConfiguration
{
    private string _baseAddress = string.Empty;

    internal event Action? OnUpdate;

    public string BaseAddress
    {
        get => _baseAddress;
        set
        {
            _baseAddress = value;
            OnUpdate?.Invoke();
        }
    }
}