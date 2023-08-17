using System.Collections.ObjectModel;

namespace Microsoft.Fast.Components.FluentUI;

internal sealed class InternalDialogContext
{
    public Collection<IDialogReference> References { get; set; } = new();

    public FluentDialogProvider DialogContainer { get; }


    public InternalDialogContext(FluentDialogProvider container)
    {
        DialogContainer = container;
    }
}
