namespace Microsoft.Fast.Components.FluentUI;

internal interface INavMenuParentElement
{
    void Register(INavMenuChildElement child);
    void Unregister(INavMenuChildElement child);
    IEnumerable<INavMenuChildElement> GetChildElements();

    bool HasChildIcons => GetChildElements().Any(x => x.HasIcon);

    INavMenuChildElement? FindElementById(string? id) =>
        GetChildElements().FirstOrDefault(x => x.Id == id)
        ?? GetChildElements().OfType<INavMenuParentElement>().Select(x => x.FindElementById(id)).FirstOrDefault(x => x is not null);
}
