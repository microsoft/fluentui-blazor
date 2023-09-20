using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentDialogHeader : FluentComponentBase
{
    internal const string DefaultDialogHeaderIdentifier = "__DefaultDialogHeader";

    [CascadingParameter]
    private FluentDialog Dialog { get; set; } = default!;

    protected string? ClassValue => new CssBuilder(Class)
      .Build();

    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("grid-area", "dialog-header")
        .AddStyle("width", "100%")
        .AddStyle("padding", "10px")
        .Build();

    /// <summary>
    /// Title of the dialog.
    /// If defined, this value will replace the one defined in the <see cref="DialogParameters"/>.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// When true, shows the title in the header.
    /// If defined, this value will replace the one defined in the <see cref="DialogParameters"/>.
    /// </summary>
    [Parameter]
    public bool? ShowTitle { get; set; }

    /// <summary>
    /// When true, shows the dismiss button in the header.
    /// If defined, this value will replace the one defined in the <see cref="DialogParameters"/>.
    /// </summary>
    [Parameter]
    public bool? ShowDismiss { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected override void OnInitialized()
    {
        if (Dialog is null)
        {
            throw new ArgumentNullException(nameof(Dialog), $"{nameof(FluentDialogHeader)} must be used inside {nameof(FluentDialog)}");
        }

        Dialog.SetContainsHeader(this);

        if (Dialog.Instance is not null)
        {
            ShowDismiss ??= Dialog.Instance.Parameters.ShowDismiss;
            Title ??= Dialog.Instance.Parameters.Title;
            ShowTitle ??= Dialog.Instance.Parameters.ShowTitle;
        }
    }
}