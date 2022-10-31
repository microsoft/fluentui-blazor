using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

[CascadingTypeParameter(nameof(TOption))]
public partial class FluentListbox<TOption> : ListComponentBase<TOption>
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
            // an item may have been selected through the data
            if (Items != null && Items.Any() && SelectedOption == null && InternalValue == null && !Multiple)
            {
                if (OptionSelected is not null)
                    SelectedOption = Items.FirstOrDefault(i => OptionSelected(i));
                else if (OptionDisabled is not null)
                    SelectedOption = Items.FirstOrDefault(i => !OptionDisabled(i));
                else
                    // a Listbox always has an element selected
                    SelectedOption = Items.FirstOrDefault();
            }
            await RaiseChangedEvents();
        }

        await base.OnAfterRenderAsync(firstRender);
    }
}