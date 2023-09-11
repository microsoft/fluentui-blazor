using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentDialogFooter : FluentComponentBase
{
    [CascadingParameter]
    private FluentDialog? Dialog { get; set; }

    /// <summary>
    /// Gets or sets the dialog position:
    /// left (full height), right (full height)
    /// or screen middle (using Width and Height properties).
    /// </summary>
    [Parameter]
    public virtual HorizontalAlignment Alignment { get; set; } = HorizontalAlignment.Center;

    /// <summary>
    /// Text to display for the primary action.
    /// </summary>
    [Parameter]
    public string? PrimaryAction { get; set; } = "Ok"; //DialogResources.ButtonPrimary;

    /// <summary>
    /// When true, primary action's button is enabled.
    /// </summary>
    [Parameter]
    public bool PrimaryActionEnabled { get; set; } = true;

    /// <summary>
    /// The event callback invoked when primary button is clicked
    /// </summary>
    [Parameter]
    public EventCallback OnPrimaryAction { get; set; }

    /// <summary>
    /// Text to display for the secondary action.
    /// </summary>
    [Parameter]
    public string? SecondaryAction { get; set; } = "Cancel"; //DialogResources.ButtonSecondary;

    /// <summary>
    /// When true, secondary action's button is enabled.
    /// </summary>
    [Parameter]
    public bool SecondaryActionEnabled { get; set; } = true;

    /// <summary>
    /// The event callback invoked when secondary button is clicked
    /// </summary>
    [Parameter]
    public EventCallback OnSecondaryAction { get; set; }

    /// <summary>
    /// Gets whether the primary button is displayed or not. Depends on PrimaryAction having a value.
    /// </summary>
    private bool ShowPrimaryAction => !string.IsNullOrEmpty(PrimaryAction);

    /// <summary>
    /// Gets whether the secondary button is displayed or not. Depends on SecondaryAction having a value. 
    /// </summary>
    private bool ShowSecondaryAction => !string.IsNullOrEmpty(SecondaryAction);

    protected override void OnParametersSet()
    {
        if (Dialog is null)
        {
            throw new ArgumentNullException(nameof(Dialog), "FluentDialogFooter must be used inside FluentDialog");
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
}