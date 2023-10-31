using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.DesignTokens;

public interface IDesignToken<T>
{
    string? Name { get; init; }

    ValueTask DeleteValueFor(ElementReference element);
    ValueTask<T> GetValueFor(ElementReference element);

    ValueTask SetValueFor(ElementReference element, T value);
    ValueTask<DesignToken<T>> WithDefault(T value);

    ValueTask<DesignToken<T>> Create(string name);
}