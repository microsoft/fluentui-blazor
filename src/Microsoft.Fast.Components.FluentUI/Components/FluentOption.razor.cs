namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentOption<TValue> : FluentComponentBase
{
    internal FluentOptionContext? Context { get; private set; }

    [Parameter]
    public bool? Disabled { get; set; }

    /// <summary>
    /// Gets or sets the value of this option.
    /// </summary>
    [Parameter]
    public TValue? Value { get; set; }


    /// <summary>
    /// Gets or sets the name of the parent container component.
    /// </summary>
    [Parameter] public string? Name { get; set; }

    [Parameter]
    public bool? Selected { get; set; }

    [CascadingParameter] private FluentOptionContext? CascadedContext { get; set; }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        Context = string.IsNullOrEmpty(Name) ? CascadedContext : CascadedContext?.FindContextInAncestors(Name);

        if (Context == null)
        {
            throw new InvalidOperationException($"{GetType()} must have an ancestor of type FluentSelect, FluentListbox or FluentCombobox " +

                $"with a matching 'Name' property, if specified.");
        }
    }
}