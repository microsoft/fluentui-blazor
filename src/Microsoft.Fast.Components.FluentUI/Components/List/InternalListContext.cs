using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

// The list cascades this so that descendant options can talk back to it.
// It's an internal type so it doesn't show up in unrelated components by mistake.
internal class InternalListContext<TOption>
{

    public Dictionary<string, FluentOption<TOption>> Options { get; set; } = new();

    public ListBase<TOption> ListComponent { get; }

    public InternalListContext(ListBase<TOption> listComponent)
    {
        ListComponent = listComponent;
    }

    internal void Register(FluentOption<TOption> option)
    {
        Options.Add(option.OptionId, option);
    }

    internal void Unregister(FluentOption<TOption> option)
    {
        Options.Remove(option.OptionId);
    }

    /// <summary>
    /// Gets the event callback to be invoked when the selected value is changed.
    /// </summary>
    public EventCallback<string?> ValueChanged { get; set; }
}



