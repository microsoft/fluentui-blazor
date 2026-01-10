// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The component cascades this so that descendant options can talk back to it.
/// It's an internal type so it doesn't show up in unrelated components by mistake
/// </summary>
internal class InternalListContext<TValue>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InternalListContext{TValue}"/> class.
    /// </summary>
    /// <param name="component"></param>
    public InternalListContext(IInternalListBase<TValue> component)
    {
        ListComponent = component;
    }

    /// <summary>
    /// Gets the list component.
    /// </summary>
    public IInternalListBase<TValue> ListComponent { get; }

    /// <summary>
    /// Call the list component to add an option.
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    public string? AddOption(FluentOption option)
    {
        return ListComponent.AddOption(option);
    }

    /// <summary>
    /// Call the list component to remove an option.
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    public string? RemoveOption(FluentOption option)
    {
        return ListComponent.RemoveOption(option);
    }
}
