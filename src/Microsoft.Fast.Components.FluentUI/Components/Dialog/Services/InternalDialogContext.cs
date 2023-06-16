namespace Microsoft.Fast.Components.FluentUI;

public sealed class InternalDialogContext
{
    public Dictionary<string, FluentDialog> References { get; set; } = new();

    public FluentDialogContainer DialogContainer { get; }


    public InternalDialogContext(FluentDialogContainer container)
    {
        DialogContainer = container;
    }

    internal void Register(FluentDialog dialog)
    {
        References.Add(dialog.Id!, dialog);
    }

    internal void Unregister(FluentDialog dialog)
    {
        References.Remove(dialog.Id!);
    }
}
