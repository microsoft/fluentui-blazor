namespace Microsoft.Fast.Components.FluentUI;

internal interface INavMenuParentElement
{
    Task SetExpandedAsync(bool expanded, bool forceChangedEvent);
    void Register(INavMenuChildElement child);
    void Unregister(INavMenuChildElement child);
    IEnumerable<INavMenuChildElement> GetChildElements();

    bool Expanded { get; set; }
    bool Collapsed => !Expanded;
    bool HasChildIcons => GetChildElements().Any(x => x.HasIcon);
}
