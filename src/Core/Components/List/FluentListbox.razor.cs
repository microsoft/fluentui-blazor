// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A FluentSelect allows for selecting one or more options from a list of options.
/// </summary>
[CascadingTypeParameter(nameof(TValue))]
public partial class FluentListbox<TOption, TValue> : FluentListBase<TOption, TValue>
{
    /// <summary />
    public FluentListbox(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary />
    protected virtual string? ListStyle => new StyleBuilder()
        .AddStyle("width", Width)
        .AddStyle("height", Height)
        .Build();

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Import the JavaScript module
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.ListBoxContainer.Init", Id);
        }

        await base.OnAfterRenderAsync(firstRender);
    }
}
