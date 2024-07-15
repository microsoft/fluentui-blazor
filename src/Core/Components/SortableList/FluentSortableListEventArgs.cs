namespace Microsoft.FluentUI.AspNetCore.Components;

public class FluentSortableListEventArgs
{
    public FluentSortableListEventArgs()
    {

    }

    public FluentSortableListEventArgs(int oldIndex, int newIndex, string? fromListId, string? toListId)
    {
        OldIndex = oldIndex;
        NewIndex = newIndex;
        FromListId = fromListId;
        ToListId = toListId;
    }

    /// <summary>
    /// Gets the index of the item in the list before the update.
    /// </summary>
    public int OldIndex { get; }

    /// <summary>
    /// Gets the index of the item in the list after the update.
    /// </summary>
    public int NewIndex { get; }

    /// <summary>
    /// Gets the id of the list the item was in before the update.
    /// May be null if no Id was set for <see cref="FluentSortableList{TItem}"/>
    /// </summary>
    public string? FromListId { get; }

    /// <summary>
    /// Gets the id of the list the item is in after the update.
    /// May be null if no Id was set for <see cref="FluentSortableList{TItem}"/>
    /// </summary>
    public string? ToListId { get; }
}
