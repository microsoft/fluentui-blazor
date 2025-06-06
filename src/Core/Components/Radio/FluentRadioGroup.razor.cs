// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A Fluent Radio Group component.
/// Groups <see cref="FluentRadio{TValue}"/> components together.
/// </summary>
/// <typeparam name="TValue">The type of the value</typeparam>
[CascadingTypeParameter(nameof(TValue))]
public partial class FluentRadioGroup<TValue> : FluentInputBase<TValue>, IFluentRadioValueProvider
{
    private readonly string _defaultGroupName = Identifier.NewId();
    internal FluentRadioContext? Context { get; private set; }

    object? IFluentRadioValueProvider.CurrentValue => CurrentValue;

    /// <summary />
    public FluentRadioGroup(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the orientation of the group. See <see cref="AspNetCore.Components.Orientation"/>
    /// </summary>
    [Parameter]
    public Orientation? Orientation { get; set; }

    /// <summary>
    /// Gets or sets the child content to be rendering inside the <see cref="FluentRadioGroup{TValue}"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [CascadingParameter]
    private FluentRadioContext? CascadedContext { get; set; }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage(Justification = "As part of the code cannot be executed in any known usage pattern, it can't be tested.")]
    protected override void OnParametersSet()
    {
        // On the first render, we can instantiate the FluentRadioContext
        if (Context is null)
        {
            var changeEventCallback = EventCallback.Factory.CreateBinder<string?>(this, __value => CurrentValueAsString = __value, CurrentValueAsString);
            Context = new FluentRadioContext(this, CascadedContext, changeEventCallback, Orientation);
        }
        else if (Context.ParentContext != CascadedContext)
        {
            // This should never be possible in any known usage pattern, but if it happens, we want to know
            throw new InvalidOperationException("A FluentRadioGroup cannot change context after creation");
        }

        // Mutate the FluentRadioContext instance in place. Since this is a non-fixed cascading parameter, the descendant
        // FluentRadio/FluentRadioGroup components will get notified to re-render and will see the new values.
        Context.GroupName = !string.IsNullOrEmpty(Name) ? Name : _defaultGroupName;

        Context.FieldClass = EditContext?.FieldCssClass(FieldIdentifier);
    }

    /// <inheritdoc />
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    => this.TryParseSelectableValueFromString(value, out result, out validationErrorMessage);

    /// <summary>
    /// Handler for the OnFocus event.
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    protected virtual Task FocusOutHandlerAsync(FocusEventArgs e)
    {
        FocusLost = true;
        return Task.CompletedTask;
    }
}
