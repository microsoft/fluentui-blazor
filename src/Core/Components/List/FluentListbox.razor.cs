// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A FluentListbox allows for selecting one or more options from a list of options.
/// </summary>
[CascadingTypeParameter(nameof(TValue))]
public partial class FluentListbox<TOption, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TValue> : FluentListBase<TOption, TValue>
{
    /// <summary />
    public FluentListbox(LibraryConfiguration configuration) : base(configuration)
    {
        ConfigureRequiredValidation();
    }

    /// <inheritdoc />
    protected override void NotifyValidationFieldChanged()
    {
        if (ValidationFieldFor is not null)
        {
            EditContext?.NotifyFieldChanged(FluentField.CreateFieldIdentifier(ValidationFieldFor));
            return;
        }

        base.NotifyValidationFieldChanged();
    }

    /// <inheritdoc />
    protected override LambdaExpression? ValidationFieldExpression => Multiple
        ? GetRequiredSelectionFieldExpression()
        : base.ValidationFieldExpression;

    /// <summary />
    protected virtual string? ListStyle => new StyleBuilder()
        .AddStyle("height", Height)
        .Build();

    /// <inheritdoc />
    protected internal override string? AutofocusBySelector => $"fluent-listbox fluent-option[tabindex='0']";

    /// <summary>
    /// Focuses the active option after the listbox has initialized.
    /// </summary>
    /// <param name="preventScroll">Whether to prevent scrolling the option into view.</param>
    public override async Task FocusAsync(bool preventScroll = false)
    {
        // Wait for the web components and FAST updates so the active option has tabindex="0" before focusing it.
        await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.ListBoxContainer.ensureReady", Id);
        await base.FocusAsync(preventScroll);
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.ListBoxContainer.initialize", Id);
        }

        await base.OnAfterRenderAsync(firstRender);
    }
}
