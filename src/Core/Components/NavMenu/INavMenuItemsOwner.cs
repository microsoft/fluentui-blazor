namespace Microsoft.Fast.Components.FluentUI;

internal interface INavMenuItemsOwner
{
    void Register(INavMenuChildElement child);
    void Unregister(INavMenuChildElement child);
    IEnumerable<INavMenuChildElement> GetChildItems();

    string? Id { get; }
    bool Expanded { get; set; }
    bool Collapsed => !Expanded;
    bool HasChildIcons => GetChildItems().Any(x => x.HasIcon);
}
