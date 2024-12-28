// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// 
/// </summary>
public abstract class FluentDialogInstance : ComponentBase
{
    /// <summary>
    /// Gets or sets the dialog instance.
    /// </summary>
    [CascadingParameter]
    public required IDialogInstance DialogInstance { get; set; }

    /// <summary>
    /// Method invoked when the component is ready to start, having received its
    /// initial parameters from its parent in the render tree.
    ///
    /// Override this method if you will perform an asynchronous operation and
    /// want the component to refresh when that operation is completed.
    /// </summary>
    /// <param name="header"></param>
    /// <param name="footer"></param>
    protected virtual void OnInitializeDialog(DialogOptionsHeader header, DialogOptionsFooter footer)
    {
    }

    /// <summary>
    /// Configures the dialog header and footer.
    /// </summary>
    /// <returns></returns>
    protected override Task OnInitializedAsync()
    {
        var footer = DialogInstance.Options.Footer;
        footer.PrimaryAction.Label ??= "OK";
        footer.PrimaryAction.OnClickAsync ??= (e) => OnActionClickedAsync(primary: true);
        footer.SecondaryAction.Label ??= "Cancel";
        footer.SecondaryAction.OnClickAsync ??= (e) => OnActionClickedAsync(primary: false);

        OnInitializeDialog(DialogInstance.Options.Header, DialogInstance.Options.Footer);

        return base.OnInitializedAsync();
    }

    /// <summary>
    /// Method invoked when an action is clicked.
    ///
    /// Override this method if you will perform an asynchronous operation
    /// when the user clicks an action button.
    /// </summary>
    /// <returns></returns>
    protected abstract Task OnActionClickedAsync(bool primary);
}
