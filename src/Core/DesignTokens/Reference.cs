using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.DesignTokens;

public class Reference : Reference<ElementReference>
{
}

public class Reference<T>
{
    private T _current = default!;

    public T Current
    {
        get => _current;
        set => Set(value);
    }

    public void Set(T value)
    {
        _current = value;
    }
}
