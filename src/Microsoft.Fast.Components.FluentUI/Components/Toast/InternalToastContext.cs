namespace Microsoft.Fast.Components.FluentUI;

internal sealed class InternalToastContext
{
    public FluentToasts ToastsContainer { get; }


    public InternalToastContext(FluentToasts container)
    {
        ToastsContainer = container;
    }

}
