// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentAppBarItem : FluentAppBarItemBase
{
    internal string? ClassValue => new CssBuilder("fluent-appbar-item")
        .AddClass("fluent-appbar-item-local", when: string.IsNullOrEmpty(Href))
        .AddClass(Class)
        .Build();

    protected async Task OnClickHandlerAsync(MouseEventArgs ev)
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync(this);
        }
    }
}
