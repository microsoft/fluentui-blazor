namespace Microsoft.Fast.Components.FluentUI;

internal interface INavMenuParentElement
{
    Task CollapseAsync();
    Task ExpandAsync();
    void Register(INavMenuChildElement child);
    void Unregister(INavMenuChildElement child);
    IEnumerable<INavMenuChildElement> GetChildElements();

    bool Expanded { get; set; }
    bool Collapsed => !Expanded;
    bool HasChildIcons => GetChildElements().Any(x => x.HasIcon);

    INavMenuChildElement? FindElementById(string? id) =>
        GetChildElements().FirstOrDefault(x => x.Id == id)
        ?? GetChildElements().OfType<INavMenuParentElement>().Select(x => x.FindElementById(id)).FirstOrDefault(x => x is not null);
}
