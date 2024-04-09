namespace Microsoft.FluentUI.AspNetCore.Components;

public class FluentSortableListEventArgs
{
    public FluentSortableListEventArgs()
    {

    }

    public FluentSortableListEventArgs(int oldIndex, int newIndex)
    {
        OldIndex = oldIndex;
        NewIndex = newIndex;
    }

    /// <summary>
    /// Gets the index of the item in the list before the update.
    /// </summary>
    public int OldIndex { get; }

    /// <summary>
    /// Gets the index of the item in the list after the update.
    /// </summary>
    public int NewIndex { get; }
}
