using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

public interface IDesignToken<T>
{
    string? Name { get; init; }

    ValueTask DeleteValueFor(ElementReference element);
    ValueTask<T> GetValueFor(ElementReference element);
    ValueTask SetValueFor(ElementReference element);
    ValueTask SetValueFor(ElementReference element, T value);
    DesignToken<T> WithDefault(T value);
}