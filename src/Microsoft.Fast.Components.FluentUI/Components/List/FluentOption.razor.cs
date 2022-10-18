using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentOption<TOption> : FluentComponentBase
{
    internal string OptionId { get; } = Identifier.NewId();

    [CascadingParameter(Name = "ListContext")]
    internal InternalListContext<TOption> ListContext { get; set; } = default!;

    /// <summary>
    /// Gets or sets if the element is disabled
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the value of this option.
    /// </summary>
    [Parameter]
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets if the element is selected
    /// </summary>
    [Parameter]
    public bool Selected { get; set; }


    /// <summary>
    /// Called whenever the selection changed.
    /// </summary>
    [Parameter]
    public EventCallback<bool> SelectedChanged { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Called whenever the selection changed.
    /// </summary>
    [Parameter]
    public EventCallback<string> OnSelect { get; set; }

    /// <summary />
    protected async Task OnSelectHandler()
    {
        if (Disabled)
            return;

        if (ListContext == null)
        {
            Selected = !Selected;
        }

        if (SelectedChanged.HasDelegate)
        {
            await SelectedChanged.InvokeAsync(Selected);
        }

        if (OnSelect.HasDelegate)
        {
            await OnSelect.InvokeAsync(Value);
        }
        else
        {
            if (ListContext != null &&
                ListContext.ValueChanged.HasDelegate)
            {
                await ListContext.ValueChanged.InvokeAsync(Value);
            }
        }
    }

}