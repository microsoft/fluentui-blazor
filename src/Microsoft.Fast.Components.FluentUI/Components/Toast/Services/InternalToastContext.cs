namespace Microsoft.Fast.Components.FluentUI;

internal sealed class InternalToastContext
{
    public Dictionary<string, FluentToast> References { get; set; } = new();

    public FluentToastContainer ToastContainer { get; }

    public InternalToastContext(FluentToastContainer container)
    {
        ToastContainer = container;
    }

    internal void Register(FluentToast dialog)
    {
        References.Add(dialog.Id!, dialog);
    }

    internal void Unregister(FluentToast dialog)
    {
        References.Remove(dialog.Id!);
    }

}
