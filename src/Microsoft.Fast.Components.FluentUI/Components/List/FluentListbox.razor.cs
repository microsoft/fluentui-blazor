using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

[CascadingTypeParameter(nameof(TOption))]
public partial class FluentListbox<TOption> : ListBase<TOption>
{
    /// <summary>
    /// Width style
    /// </summary>

    [Parameter]
    public string Width { get; set; } = "250px";

    /// <summary>
    /// Height style
    /// </summary>
    [Parameter]
    public string Height { get; set; } = "350px";

    /// <summary>
    /// The maximum number of options that should be visible in the listbox scroll area.
    /// </summary>
    [Parameter]
    public int Size { get; set; }


    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // a Listbox always has an element selected
            if (Items != null && Items.Any() && SelectedItem == null && Value == null)
            {
                SelectedItem = Items.FirstOrDefault();
            }
            await RaiseChangedEvents();
        }

        await base.OnAfterRenderAsync(firstRender);
    }
}