using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentOption<TOption> : FluentComponentBase, IDisposable where TOption : notnull
{

    [CascadingParameter(Name = "ListContext")]
    internal InternalListContext<TOption> _internalListContext { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether the element is disabled.
    /// </summary>
    [Parameter]
    public bool? Disabled { get; set; }

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
        _internalListContext.Register(this);

        return base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && Selected &&
            _internalListContext != null &&
            _internalListContext.ValueChanged.HasDelegate &&
            _internalListContext.ListComponent.Multiple)
        {
            await _internalListContext.ValueChanged.InvokeAsync(Value);
        }
    }

    /// <summary />
    public async Task OnClickHandlerAsync()
    {
        if (Disabled == true)
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
            if (_internalListContext != null && _internalListContext.ListComponent.Items is null)
            {
                if (_internalListContext.ValueChanged.HasDelegate)
                {
                    await _internalListContext.ValueChanged.InvokeAsync(Value);
                }
                if (_internalListContext.SelectedOptionChanged.HasDelegate)
                {
                    await _internalListContext.SelectedOptionChanged.InvokeAsync();
                }
            }
        }
    }

    public FluentOption()
    {
        Id = Identifier.NewId();
    }

    public void Dispose() => _internalListContext.Unregister(this);
}