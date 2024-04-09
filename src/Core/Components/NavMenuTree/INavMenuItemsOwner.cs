namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// An interface for supporting the ownership of <see cref="FluentNavMenuItemBase"/>.
/// </summary>
public interface INavMenuItemsOwner
{
    /// <summary>
    /// <see cref="FluentComponentBase.Id"/>.
    /// </summary>
    string? Id { get; }

    /// <summary>
    /// Returns <see langword="true"/> if the implementing component
    /// is collapsed, and <see langword="false"/> if expanded.
    /// </summary>
    bool Collapsed => !Expanded;

    /// <summary>
    /// Returns <see langword="true"/> if the implementing component
    /// is expanded, and <see langword="false"/> if collapsed.
    /// </summary>
    bool Expanded { get; set; }

    /// <summary>
    /// Returns <see langword="true"/> if the result of <see cref="GetChildItems"/>
    /// contains any items, otherwise returns <see langword="false"/>.
    /// </summary>
    bool HasChildIcons => GetChildItems().Any(x => x.HasIcon);

    /// <summary>
    /// Gets all <see cref="FluentNavMenuItemBase"/> items directly
    /// parented by the implementing object.
    /// </summary>
    /// <returns></returns>
    IEnumerable<FluentNavMenuItemBase> GetChildItems();

    /// <summary>
    /// Called by a direct child to register itself with its owner
    /// when it is created.
    /// </summary>
    /// <param name="child"></param>
    void Register(FluentNavMenuItemBase child);

    /// <summary>
    /// Called by a direct child to unregister itself from its owner
    /// when it is being disposed.
    /// </summary>
    /// <param name="child"></param>
    void Unregister(FluentNavMenuItemBase child);
}
