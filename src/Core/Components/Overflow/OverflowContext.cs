#pragma warning disable IDE0073

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Provides the complete typed overflow collection and its trigger identifier to templates.
/// </summary>
/// <typeparam name="TItem">The type of the source items.</typeparam>
public sealed class OverflowContext<TItem>
{
    internal OverflowContext(IReadOnlyList<TItem> items, IReadOnlyList<OverflowItem> itemsOverflow, int overflowCount, string idMoreButton)
    {
        Items = items;
        ItemsOverflow = itemsOverflow;
        OverflowCount = overflowCount;
        IdMoreButton = idMoreButton;
    }

    /// <summary>
    /// Gets all overflowed items in source order, including items not rendered for measurement.
    /// </summary>
    public IReadOnlyList<TItem> Items { get; }

    /// <summary>
    /// Gets the rendered overflow records, preserving direct child content template support.
    /// </summary>
    public IReadOnlyList<OverflowItem> ItemsOverflow { get; }

    /// <summary>
    /// Gets the total number of overflowed items.
    /// </summary>
    public int OverflowCount { get; }

    /// <summary>
    /// Gets the identifier of the overflow trigger, suitable for anchoring a popup.
    /// </summary>
    public string IdMoreButton { get; }
}