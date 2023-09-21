using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentDialogFooter : FluentComponentBase
{
    internal const string DefaultDialogFooterIdentifier = "__DefaultDialogFooter";

    /// <summary />
    [CascadingParameter]
    private FluentDialog Dialog { get; set; } = default!;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-dialog-footer")
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .Build();

    ///// <summary>
    ///// Text to display for the primary action.
    ///// </summary>
    //[Parameter]
    //public string? PrimaryAction { get; set; } = "Ok"; //DialogResources.ButtonPrimary;

    ///// <summary>
    ///// When true, primary action's button is enabled.
    ///// </summary>
    //[Parameter]
    //public bool PrimaryActionEnabled { get; set; } = true;

    ///// <summary>
    ///// The event callback invoked when primary button is clicked
    ///// </summary>
    //[Parameter]
    //public EventCallback OnPrimaryAction { get; set; }

    ///// <summary>
    ///// Text to display for the secondary action.
    ///// </summary>
    //[Parameter]
    //public string? SecondaryAction { get; set; } = "Cancel"; //DialogResources.ButtonSecondary;

    ///// <summary>
    ///// When true, secondary action's button is enabled.
    ///// </summary>
    //[Parameter]
    //public bool SecondaryActionEnabled { get; set; } = true;

    ///// <summary>
    ///// The event callback invoked when secondary button is clicked
    ///// </summary>
    //[Parameter]
    //public EventCallback OnSecondaryAction { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets whether the primary button is displayed or not. Depends on PrimaryAction having a value.
    /// </summary>
    // private bool ShowPrimaryAction => !string.IsNullOrEmpty(PrimaryAction);

    /// <summary>
    /// Gets whether the secondary button is displayed or not. Depends on SecondaryAction having a value. 
    /// </summary>
    // private bool ShowSecondaryAction => !string.IsNullOrEmpty(SecondaryAction);


    /// <summary />
    protected override void OnInitialized()
    {
        if (Dialog is null)
        {
            throw new ArgumentNullException(nameof(Dialog), $"{nameof(FluentDialogFooter)} must be used inside {nameof(FluentDialog)}");
        }

        Dialog.SetDialogFooter(this);
    }

    private async Task OnPrimaryActionButtonClickAsync()
    {
        if (Dialog.Instance?.Parameters?.PrimaryActionEnabled == true)
        {
            await Dialog!.CloseAsync(Dialog.Instance?.Content ?? true);
        }
    }

    private async Task OnSecondaryActionButtonClickAsync()
    {
        if (Dialog.Instance?.Parameters?.SecondaryActionEnabled == true)
        {
            await Dialog!.CancelAsync(Dialog.Instance?.Content ?? false);
        }
    }
}