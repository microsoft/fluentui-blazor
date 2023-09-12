namespace Microsoft.Fast.Components.FluentUI;

internal sealed class InternalToastContext
{
    public Dictionary<string, FluentToast> References { get; set; } = new();

    public FluentToastContainer ToastContainer { get; }

    public InternalToastContext(FluentToastContainer container)
    {
        ToastContainer = container;
    }

    internal void Register(FluentToast toast)
    {
        References.Add(toast.Id!, toast);
    }

    internal void Unregister(FluentToast toast)
    {
        References.Remove(toast.Id!);
    }

}
