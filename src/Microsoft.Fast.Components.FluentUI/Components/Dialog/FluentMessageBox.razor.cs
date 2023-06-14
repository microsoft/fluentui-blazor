using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public partial class FluentMessageBox : IDialogInstance
{
    [Parameter]
    public MessageBoxOptions MessageBoxOptions { get; set; } = default!;

    protected virtual Task OnConfirmAsync(MouseEventArgs e)
    {
        return OnCloseAsync(DialogResult.Ok(true));
    }

    protected virtual Task OnCancelAsync(MouseEventArgs e)
    {
        return OnCloseAsync(DialogResult.Cancel());
    }

    public virtual Task OnCloseAsync(DialogResult result)
    {
        return Task.CompletedTask;
    }
}
