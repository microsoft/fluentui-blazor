using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

[CascadingTypeParameter(nameof(TOption))]
public partial class FluentSelect<TOption> : ListBase<TOption>
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
            if (Items != null && Items.Any() && SelectedItem == null && InternalValue == null && Multiple == false)
            {
                if (OptionSelected is not null)
                    SelectedItem = Items.FirstOrDefault(i => OptionSelected(i));
                else if (OptionDisabled is not null)
                    SelectedItem = Items.FirstOrDefault(i => !OptionDisabled(i));
                else
                    // a Listbox always has an element selected
                    SelectedItem = Items.FirstOrDefault();
                await RaiseChangedEvents();
            }

            await base.OnAfterRenderAsync(firstRender);
        }
    }
}