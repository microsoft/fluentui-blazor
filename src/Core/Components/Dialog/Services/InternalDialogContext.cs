using System.Collections.ObjectModel;

namespace Microsoft.Fast.Components.FluentUI;

internal sealed class InternalDialogContext
{
    //public Dictionary<string, FluentDialog> ComponentReferences { get; set; } = new();

    public Collection<IDialogReference> References { get; set; } = [];

    public FluentDialogContainer DialogContainer { get; }


    public InternalDialogContext(FluentDialogContainer container)
    {
        DialogContainer = container;
    }

    //internal void Register(FluentDialog dialog)
    //{
    //    ComponentReferences.Add(dialog.Id!, dialog);
    //}

    //internal void Unregister(FluentDialog dialog)
    //{
    //    ComponentReferences.Remove(dialog.Id!);
    //}
}
