// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The dialog component is a window overlaid on either the primary window or another dialog window.
/// Windows under a modal dialog are inert. 
/// </summary>
public partial class FluentDialog : FluentComponentBase
{
    /// <summary />
    public FluentDialog()
    {
    }

    /// <summary />
    internal FluentDialog(IDialogService? dialogService, DialogInstance? instance)
    {
        DialogService = dialogService;
        Instance = instance;
    }

    /// <summary />
    [Inject]
    private IDialogService? DialogService { get; set; }

    /// <summary>
    /// Gets or sets the instance used by the <see cref="DialogService" />.
    /// </summary>
    [Parameter]
    public DialogInstance? Instance { get; set; }

    /// <summary>
    /// Used when not calling the <see cref="DialogService" /> to show a dialog.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    ///// <summary>
    ///// Initializes the component.
    ///// </summary>
    ///// <exception cref="FluentServiceProviderException{FluentDialogProvider}"></exception>
    //protected override void OnInitialized()
    //{
    //    if (DialogService != null)
    //    {
    //        if (DialogService.ProviderNotAvailable())
    //        {
    //            throw new FluentServiceProviderException<FluentDialogProvider>();
    //        }

    //        DialogService.Add(this);
    //    }

    //    base.OnInitialized();
    //}
}
