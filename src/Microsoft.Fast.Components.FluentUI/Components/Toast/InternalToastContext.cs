namespace Microsoft.Fast.Components.FluentUI;

internal sealed class InternalToastContext
{
    public FluentToastContainer ToastsContainer { get; }


    public InternalToastContext(FluentToastContainer container)
    {
        ToastsContainer = container;
    }

}
