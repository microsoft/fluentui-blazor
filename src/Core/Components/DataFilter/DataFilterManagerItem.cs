namespace Microsoft.FluentUI.AspNetCore.Components;

public class DataFilterManagerItem<TItem>
{
    public string Title { get; set; } = default!;
    public DataFilterCriteria<TItem> Criteria { get; set; } = default!;
    public bool AllowEdit { get; set; }
}
