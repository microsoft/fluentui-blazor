using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentRadio<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TValue> : FluentComponentBase
{
    /// <summary>
    /// Gets context for this <see cref="FluentRadio{TValue}"/>. 
    /// </summary>
    internal FluentRadioContext? Context { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the element is readonly.
    /// </summary>
    [Parameter]
    public bool ReadOnly { get; set; }

    /// <summary>
    /// Gets or sets the text displayed just above the component.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// Gets or sets the content displayed just above the component.
    /// </summary>
    [Parameter]
    public RenderFragment? LabelTemplate { get; set; }

    /// <summary>
    /// Gets or sets the text used on aria-label attribute.
    /// </summary>
    [Parameter]
    public virtual string? AriaLabel { get; set; }

    /// <summary>
    /// Gets or sets the value of the element.
    /// </summary>
    [Parameter]
    public TValue? Value { get; set; }

    /// <summary>
    /// Disables the form control, ensuring it doesn't participate in form submission
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the name of the parent fluent radio group.
    /// </summary>
    [Parameter]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the element needs to have a value.
    /// </summary>
    [Parameter]
    public bool Required { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the element is checked.
    /// </summary>
    [Parameter]
    public bool? Checked { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [CascadingParameter] private FluentRadioContext? CascadedContext { get; set; }

    public FluentRadio()
    {
        Id = Identifier.NewId();
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        Context = string.IsNullOrEmpty(Name) ? CascadedContext : CascadedContext?.FindContextInAncestors(Name);

        if (Context == null)
        {
            throw new InvalidOperationException($"{GetType()} must have an ancestor {typeof(FluentRadioGroup<TValue>)} " +
                $"with a matching 'Name' property, if specified.");
        }

        if (Checked.HasValue && Checked == true)
        {
            Context.CurrentValue = Value;
        }
    }
}
