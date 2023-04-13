using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public partial class FluentOverlay
{
    [Parameter]
    public EventCallback<MouseEventArgs> OnClose { get; set; }

    [Parameter]
    public bool Transparent { get; set; } = true;

    [Parameter]
    public double Opacity { get; set; } = 0.4;

    protected Task OnCloseHandlerAsync(MouseEventArgs e)
    {
        if (OnClose.HasDelegate)
        {
            return OnClose.InvokeAsync(e);
        }

        return Task.CompletedTask;
    }
}
