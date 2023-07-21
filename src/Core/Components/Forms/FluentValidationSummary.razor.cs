using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentValidationSummary
{
    private IEnumerable<string>? _validationMessages;

    [CascadingParameter]
    public EditContext FluentEditContext { get; set; } = default!;

    protected override void OnInitialized()
    {

        _validationMessages = Model is null
            ? FluentEditContext.GetValidationMessages()
            : FluentEditContext.GetValidationMessages(new FieldIdentifier(Model, string.Empty));
    }
}