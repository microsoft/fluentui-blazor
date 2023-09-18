using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;

namespace FluentUI.Demo.Shared.Pages.Dialog.Examples;
public partial class CustomFooter : FluentComponentBase
{
    [CascadingParameter]
    private FluentDialog? Dialog { get; set; }

    /// <summary>
    /// Text to display for the primary action.
    /// </summary>
    [Parameter]
    public string? PrimaryAction { get; set; } = "Ok"; 

    /// <summary>
    /// The event callback invoked when primary button is clicked
    /// </summary>
    [Parameter]
    public EventCallback OnPrimaryAction { get; set; }


    /// <summary>
    /// Text to display for the secondary action.
    /// </summary>
    [Parameter]
    public string? SecondaryAction { get; set; } = "Cancel"; 

    /// <summary>
    /// The event callback invoked when secondary button is clicked
    /// </summary>
    [Parameter]
    public EventCallback OnSecondaryAction { get; set; }

    /// <summary>
    /// Text to display for the tertiary action.
    /// </summary>
    [Parameter]
    public string? TertiaryAction { get; set; } = "";

    /// <summary>
    /// The event callback invoked when tertiary button is clicked
    /// </summary>
    [Parameter]
    public EventCallback OnTertiaryAction { get; set; }

    protected override void OnParametersSet()
    {
        if (Dialog is null)
        {
            throw new ArgumentNullException(nameof(Dialog), "CustomFooter must be used inside FluentDialog");
        }
    }

    private async Task OnPrimaryActionButtonClickAsync()
    {
        if (OnPrimaryAction.HasDelegate)
        {
            await OnPrimaryAction.InvokeAsync();
        }
        else
        {
            await Dialog!.CloseAsync(Dialog.Instance?.Content ?? true);
        }
    }

    private async Task OnSecondaryActionButtonClickAsync()
    {
        if (OnSecondaryAction.HasDelegate)
        {
            await OnSecondaryAction.InvokeAsync();
        }
        else
        {
            await Dialog!.CancelAsync(Dialog.Instance?.Content ?? false);
        }
    }

    private async Task OnTertiaryActionButtonClickAsync()
    {
        if (OnTertiaryAction.HasDelegate)
        {
            await OnTertiaryAction.InvokeAsync();
        }
        else
        {
            await Dialog!.CancelAsync(Dialog.Instance?.Content ?? false);
        }
    }
}