// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Explorers.Components.Layout;

public partial class ClipboardFeature
{
    [Inject]
    public required IJSRuntime JSRuntime { get; set; }

    private async Task ItemToCopyAsync(string name)
    {
        await JSRuntime.InvokeVoidAsync("itemToClipboard", name);
    }
}
