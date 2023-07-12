namespace Microsoft.Fast.Components.FluentUI;

internal interface INavMenuParentElement
{
    void Register(INavMenuChildElement child);
    void Unregister(INavMenuChildElement child);
    IEnumerable<INavMenuChildElement> GetChildElements();

    string? Id { get; }
    bool Expanded { get; set; }
    bool Collapsed => !Expanded;
    bool HasChildIcons => GetChildElements().Any(x => x.HasIcon);
}
