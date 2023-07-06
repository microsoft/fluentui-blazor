namespace Microsoft.Fast.Components.FluentUI;

internal interface INavMenuItemsHolder
{
    void AddItem(INavMenuItem item);
    void RemoveItem(INavMenuItem item);
    IEnumerable<INavMenuItem> GetItems();

    bool HasChildIcons => GetItems().Any(x => x.HasIcon);

    INavMenuItem? GetItemById(string? id) =>
        GetItems().FirstOrDefault(x => x.Id == id)
        ?? GetItems().OfType<INavMenuItemsHolder>().Select(x => x.GetItemById(id)).FirstOrDefault(x => x is not null);
}
