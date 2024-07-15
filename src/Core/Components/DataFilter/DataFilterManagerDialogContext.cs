using System.ComponentModel.DataAnnotations;

namespace Microsoft.FluentUI.AspNetCore.Components;

public class DataFilterManagerDialogContext<TItem>
{
    [Required]
    public string Name { get; set; } = default!;

    public bool AllowDelete { get; set; }

    public bool IsDeleted { get; set; }

    public FluentDataFilterManager<TItem> FilterManager { get; set; } = default!;
}
