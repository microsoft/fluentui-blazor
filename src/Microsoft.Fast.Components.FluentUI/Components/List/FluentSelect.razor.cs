using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

[CascadingTypeParameter(nameof(TOption))]
public partial class FluentSelect<TOption> : ListComponentBase<TOption>
{
    /// <summary>
    /// The open attribute.
    /// </summary>
    [Parameter]
    public bool? Open { get; set; }

    /// <summary>
    /// Reflects the placement for the listbox when the select is open.
    /// See <see cref="FluentUI.SelectPosition"/>
    /// </summary>
    [Parameter]
    public SelectPosition? Position { get; set; }

    /// <summary>
    /// Gets or sets the visual appearance. See <seealso cref="FluentUI.Appearance"/>
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // an item may have been selected through the data
            if (Items != null && Items.Any() && SelectedOption == null && InternalValue == null)
            {
                if (Multiple)
                {
                    //if (OptionSelected is not null)
                    //    SelectedOptions = Items.Where(i => OptionSelected.Invoke(i));
                    //else if (OptionDisabled is not null)
                    //    SelectedOptions = Items.Where(i => !OptionDisabled.Invoke(i));
                }
                else
                {
                    if (OptionSelected is not null)
                        SelectedOption = Items.FirstOrDefault(i => OptionSelected(i));
                    else if (OptionDisabled is not null)
                        SelectedOption = Items.FirstOrDefault(i => !OptionDisabled(i));
                    await RaiseChangedEvents();
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }
    }
}