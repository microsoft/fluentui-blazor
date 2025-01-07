namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentDialogHeader : FluentComponentBase
{
    internal const string DefaultDialogHeaderIdentifier = "__DefaultDialogHeader";

    /// <summary />
    [CascadingParameter]
    private FluentDialog Dialog { get; set; } = default!;

    /// <summary />
    private int TabIndex { get; set; } = 0;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-dialog-header")
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .Build();

    /// <summary>
    /// When true, the header is visible.
    /// Default is True.
    /// </summary>
    [Parameter]
    public bool Visible { get; set; } = true;

    /// <summary>
    /// When true, shows the dismiss button in the header.
    /// If defined, this value will replace the one defined in the <see cref="DialogParameters"/>.
    /// </summary>
    [Parameter]
    public bool? ShowDismiss { get; set; }

    /// <summary>
    /// When false, uses the default tabindex of 0 in the header.
    /// If defined, the header will have a tabindex of -1. 
    /// </summary>
    [Parameter]
    public bool ExcludeFromTabFocus { get; set; } = false;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    internal void Refresh()
    {
        StateHasChanged();
    }

    /// <summary />
    protected override void OnInitialized()
    {
        if (Dialog is null)
        {
            throw new ArgumentNullException(nameof(Dialog), $"{nameof(FluentDialogHeader)} must be used inside {nameof(FluentDialog)}");
        }

        Dialog.SetDialogHeader(this);

        if (Dialog.Instance is not null)
        {
            ShowDismiss ??= Dialog.Instance.Parameters.ShowDismiss;

            if (ExcludeFromTabFocus)
            {
                TabIndex = -1;
            }
        }
    }
}
