using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

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

    /// <summary>
    /// When true, the footer is visible.
    /// Default is True.
    /// </summary>
    [Parameter]
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    protected override void OnInitialized()
    {
        if (Dialog is null)
        {
            throw new ArgumentNullException(nameof(Dialog), $"{nameof(FluentDialogFooter)} must be used inside {nameof(FluentDialog)}");
        }

        Dialog.SetDialogFooter(this);
    }

    /// <summary />
    internal void Refresh()
    {
        StateHasChanged();
    }

    /// <summary />
    private async Task OnPrimaryActionButtonClickAsync()
    {
        if (Dialog.Instance?.Parameters?.PrimaryActionEnabled == true)
        {
            await Dialog!.CloseAsync(Dialog.Instance?.Content ?? true);
        }
    }

    /// <summary />
    private async Task OnSecondaryActionButtonClickAsync()
    {
        if (Dialog.Instance?.Parameters?.SecondaryActionEnabled == true)
        {
            await Dialog!.CancelAsync(Dialog.Instance?.Content ?? false);
        }
    }
}
