// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A Fluent Radio button component.
/// </summary>
/// <typeparam name="TValue">The type for the value of the radio button</typeparam>
public partial class FluentRadio<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TValue> : FluentComponentBase, IFluentField
{
    internal FluentRadioContext? Context { get; private set; }

    /// <summary />
    protected virtual string? ClassValue => DefaultClassBuilder.Build();

    /// <summary />
    protected virtual string? StyleValue => DefaultStyleBuilder.Build();

    /// <summary />
    public FluentRadio()
    {
        Id = Identifier.NewId();
    }

    [CascadingParameter]
    private FluentRadioContext? CascadedContext { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the element is checked.
    /// </summary>
    [Parameter]
    public bool? Checked { get; set; }

    /// <summary>
    /// Gets or sets the name of the parent fluent radio group.
    /// </summary>
    [Parameter]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the value of the element.
    /// </summary>
    [Parameter]
    public TValue? Value { get; set; }

    /// <summary />
    [Parameter]
    public EventCallback<TValue?> ValueChanged { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    #region IFluentField

    /// <inheritdoc cref="IFluentField.FocusLost" />
    public virtual bool FocusLost { get; protected set; }

    /// <inheritdoc cref="IFluentField.Disabled" />
    [Parameter]
    public virtual bool? Disabled { get; set; }

    /// <inheritdoc cref="IFluentField.Label" />
    [Parameter]
    public virtual string? Label { get; set; }

    /// <inheritdoc cref="IFluentField.LabelTemplate" />
    [Parameter]
    public virtual RenderFragment? LabelTemplate { get; set; }

    /// <inheritdoc cref="IFluentField.LabelPosition" />
    [Parameter]
    public virtual LabelPosition? LabelPosition { get; set; }

    /// <inheritdoc cref="IFluentField.LabelWidth" />
    [Parameter]
    public virtual string? LabelWidth { get; set; }

    /// <inheritdoc cref="IFluentField.Required" />
    [Parameter]
    public virtual bool? Required { get; set; }

    /// <inheritdoc cref="IFluentField.Message" />
    [Parameter]
    public virtual string? Message { get; set; }

    /// <inheritdoc cref="IFluentField.MessageIcon" />
    [Parameter]
    public virtual Icon? MessageIcon { get; set; }

    /// <inheritdoc cref="IFluentField.MessageTemplate" />
    [Parameter]
    public virtual RenderFragment? MessageTemplate { get; set; }

    /// <inheritdoc cref="IFluentField.MessageCondition" />
    [Parameter]
    public virtual Func<IFluentField, bool>? MessageCondition { get; set; }

    /// <inheritdoc cref="IFluentField.MessageState" />
    [Parameter]
    public virtual MessageState? MessageState { get; set; }

    #endregion

    /// <inheritdoc />
    protected override void OnParametersSet()
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
