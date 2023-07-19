using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentValidationSummary
{
    [CascadingParameter]
    public EditContext FluentEditContext { get; set; } = default!;
}