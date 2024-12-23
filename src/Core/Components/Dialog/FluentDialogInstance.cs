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
    /// 
    /// </summary>
    [CascadingParameter]
    public required IDialogInstance Dialog { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="header"></param>
    /// <param name="footer"></param>
    protected virtual void OnInitializeDialog(DialogOptionsHeader header, DialogOptionsFooter footer)
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override Task OnInitializedAsync()
    {
        Dialog.Options.Footer.PrimaryAction.Label = "OK";
        Dialog.Options.Footer.PrimaryAction.OnClickAsync = (e) => OnActionClickedAsync(primary: true);
        Dialog.Options.Footer.SecondaryAction.Label = "Cancel";
        Dialog.Options.Footer.SecondaryAction.OnClickAsync = (e) => OnActionClickedAsync(primary: false);

        OnInitializeDialog(Dialog.Options.Header, Dialog.Options.Footer);

        return base.OnInitializedAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected abstract Task OnActionClickedAsync(bool primary);
}
