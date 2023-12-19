using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentOption<TOption> : FluentComponentBase, IDisposable where TOption : notnull
{
    internal string OptionId { get; } = Identifier.NewId();

    [CascadingParameter(Name = "ListContext")]
    internal InternalListContext<TOption> InternalListContext { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether the element is disabled.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the value of this option.
    /// </summary>
    [Parameter]
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the element is selected.
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

    protected override Task OnInitializedAsync()
    {
        InternalListContext.Register(this);

        return base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && Selected && 
            InternalListContext != null && 
            InternalListContext.ValueChanged.HasDelegate &&
            InternalListContext.ListComponent.Multiple)
        {
            await InternalListContext.ValueChanged.InvokeAsync(Value);
        }
    }

    /// <summary />
    protected async Task OnSelectHandler()
    {
        if (Disabled)
            return;

        Selected = !Selected;


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
            if (InternalListContext != null &&
                InternalListContext.ValueChanged.HasDelegate && 
                InternalListContext.ListComponent.Items is null)
            {
                await InternalListContext.ValueChanged.InvokeAsync(Value);
            }
        }
    }

    public void Dispose() => InternalListContext.Unregister(this);
}