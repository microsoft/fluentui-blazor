// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

internal sealed class InternalDialogContext
{
    public Collection<IDialogReference> References { get; set; } = [];

    public FluentDialogProvider DialogContainer { get; }

    public InternalDialogContext(FluentDialogProvider container)
    {
        DialogContainer = container;
    }
}
