

namespace Microsoft.Fast.Components.FluentUI;

public class DialogOptions
{
    /// <summary>
    /// Gets or sets the window position:
    /// left (full height), right (full height)
    /// or screen middle (using Width and Height properties).
    /// </summary>
    public HorizontalAlignment Alignment { get; set; } = HorizontalAlignment.Right;

    /// <summary>
    /// Label of the Cancel button.
    /// </summary>
    public string CancelText { get; set; } = "Cancel"; //FluentPanelResources.ButtonCancel;

    /// <summary>
    /// Window height, only if <see cref="Alignment"/> is Center.
    /// </summary>
    public string? Height { get; set; }

    /// <summary>
    /// Allow to close (cancel) this dialog when the user click outside the dialog panel.
    /// </summary>
    public bool AllowedToCancelOutsideDialog { get; set; } = true;

    /// <summary>
    /// Display or hide the Cancel button (in the footer).
    /// </summary>
    public bool ShowCancel { get; set; } = true;

    /// <summary>
    /// Display or hide the close cross button (in the header).
    /// </summary>
    public bool ShowDismiss { get; set; } = true;

    /// <summary>
    /// Display or hide the OK button (in the footer).
    /// </summary>
    public bool ShowOK { get; set; } = true;

    /// <summary>
    /// Label of the OK button.
    /// </summary>
    public string OKText { get; set; } = "Ok"; //FluentPanelResources.ButtonOK;

    /// <summary>
    /// Gets or sets the extra styles appied to the Body content.
    /// </summary>
    public string StyleBody { get; set; } = string.Empty;

    /// <summary>
    /// Window width, only if <see cref="Alignment"/> is Center.
    /// </summary>
    public string? Width { get; set; }

    /// <summary>
    /// Default configuration for a and almost full screen dialog.
    /// </summary>
    public static DialogOptions LargeScreen() => new()
    {
        Alignment = HorizontalAlignment.Center,
        Width = "80vw",
        Height = "95vh",
        AllowedToCancelOutsideDialog = false,
        ShowOK = false,
        ShowCancel = false,
    };
}
