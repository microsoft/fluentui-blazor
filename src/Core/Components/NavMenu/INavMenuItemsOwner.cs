namespace Microsoft.Fast.Components.FluentUI;

internal interface INavMenuItemsOwner
{
    void Register(FluentNavMenuItemBase child);
    void Unregister(FluentNavMenuItemBase child);
    IEnumerable<FluentNavMenuItemBase> GetChildItems();

    string? Id { get; }
    bool Expanded { get; set; }
    bool Collapsed => !Expanded;
    bool HasChildIcons => GetChildItems().Any(x => x.HasIcon);
}
