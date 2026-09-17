// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Non-generic base class for InternalListContext to support type validation.
/// </summary>
internal abstract class InternalListContext
{
    /// <summary>
    /// Gets the Type of the TValue generic parameter for the list component.
    /// </summary>
    public abstract Type ValueType { get; }
}

/// <summary>
/// The component cascades this so that descendant options can talk back to it.
/// It's an internal type so it doesn't show up in unrelated components by mistake
/// </summary>
internal class InternalListContext<TValue> : InternalListContext
{
    /// <summary>
    /// Initializes a new instance of the InternalListContext class.
    /// </summary>
    /// <param name="component"></param>
    public InternalListContext(IInternalListBase<TValue> component)
    {
        ListComponent = component;
    }

    /// <inheritdoc />
    public override Type ValueType => typeof(TValue);

    /// <summary>
    /// Gets the list component.
    /// </summary>
    public IInternalListBase<TValue> ListComponent { get; }

    /// <summary>
    /// Call the list component to add an option.
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    public string? AddOption(FluentOption<TValue> option)
    {
        return ListComponent.AddOption(option);
    }

    /// <summary>
    /// Call the list component to remove an option.
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    public string? RemoveOption(FluentOption<TValue> option)
    {
        return ListComponent.RemoveOption(option);
    }
}
