namespace Microsoft.FluentUI.AspNetCore.Components;

internal sealed class InternalToastContext
{
    public Dictionary<string, FluentToast> References { get; set; } = new();

    public FluentToastProvider ToastProvider { get; }

    public InternalToastContext(FluentToastProvider provider)
    {
        ToastProvider = provider;
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
