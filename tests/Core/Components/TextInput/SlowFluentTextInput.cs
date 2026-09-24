// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.TextInput;

/// <summary>
/// Test double that artificially delays <see cref="ChangeHandlerAsync"/> so tests can simulate
/// two immediate keystrokes whose server-side processing overlaps/finishes out of order.
/// </summary>
public sealed class SlowFluentTextInput : FluentTextInput
{
    public SlowFluentTextInput(LibraryConfiguration configuration) : base(configuration) { }

    [Parameter]
    public int ProcessingDelayMs { get; set; }

    protected override async Task ChangeHandlerAsync(ChangeEventArgs e)
    {
        if (ProcessingDelayMs > 0)
        {
            await Task.Delay(ProcessingDelayMs);
        }

        await base.ChangeHandlerAsync(e);
    }
}
