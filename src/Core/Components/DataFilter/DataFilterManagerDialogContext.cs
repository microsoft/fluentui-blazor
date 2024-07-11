using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.FluentUI.AspNetCore.Components;

public class DataFilterManagerDialogContext<TItem>
{
    [Required]
    public string Title { get; set; } = default!;

    public DataFilterCriteria<TItem> Criteria { get; set; } = default!;

    public RenderFragment<DataFilterCriteria<TItem>> DataFilterTemplate { get; set; } = default!;

    public bool AllowDelete { get; set; }

    public bool IsDeleted { get; set; }

    public string TextSave { get; set; } = "Save";

    public string TextDelete { get; set; } = "Delete";

    public string TextCancel { get; set; } = "Cancel";

    public string TextTitle { get; set; } = "Title";
}
