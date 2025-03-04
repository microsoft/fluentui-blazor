// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The Option element is used to define an item contained in a List component.
/// </summary>
public partial class FluentOption<TOption>
{
    /// <summary>
    /// Gets or sets the context of the list.
    /// </summary>
    [CascadingParameter(Name = "ListContext")]
    internal InternalListContext<TOption>? InternalListContext { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the element is disabled.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the value of this option.
    /// </summary>
    [Parameter]
    public TOption? Value { get; set; }

    /// <summary>
    /// Gets or sets the value indicating whether the element is selected.
    /// This should be used with two-way binding.
    /// </summary>
    /// <example>
    /// @bind-Value="model.PropertyName"
    /// </example>
    [Parameter]
    public bool Selected { get; set; }

    /// <summary>
    /// Gets or sets a callback that updates the bound value.
    /// </summary>
    [Parameter]
    public EventCallback<bool> SelectedChanged { get; set; }

    /// <summary>
    /// Gets or sets the content to display in the description slot.
    /// </summary>
    [Parameter]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    private async Task OnSelectHandlerAsync()
    {
        if (Disabled)
        {
            return;
        }

        Selected = !Selected;

        if (SelectedChanged.HasDelegate)
        {
            await SelectedChanged.InvokeAsync(Selected);
        }
    }
}
