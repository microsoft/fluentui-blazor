namespace Microsoft.Fast.Components.FluentUI;

internal sealed class InternalMessageContext
{
    public Dictionary<string, FluentMessageBar> References { get; set; } = new();

    public FluentMessageBarContainer MessageBarContainer { get; }

    public InternalMessageContext(FluentMessageBarContainer container)
    {
        MessageBarContainer = container;
    }

    internal void Register(FluentMessageBar messageBar)
    {
        References.Add(messageBar.Id!, messageBar);
    }

    internal void Unregister(FluentMessageBar messageBar)
    {
        References.Remove(messageBar.Id!);
    }

}
