namespace Microsoft.Fast.Components.FluentUI;

internal interface INavMenuItemsOwner
{
    void Register(FluentNavMenuItem child);
    void Unregister(FluentNavMenuItem child);
    IEnumerable<FluentNavMenuItem> GetChildItems();

    string? Id { get; }
    bool Expanded { get; set; }
    bool Collapsed => !Expanded;
    bool HasChildIcons => GetChildItems().Any(x => x.HasIcon);
}
