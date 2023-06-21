namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// _instance specific settings for a <see cref="FluentDialog"/> component
/// </summary>
public class DialogSettings
{
    /// <summary>
    /// Gets or sets the dialog position:
    /// left (full height), right (full height)
    /// or screen middle (using Width and Height properties).
    /// </summary>
    public HorizontalAlignment Alignment { get; set; } = HorizontalAlignment.Right;

    /// <summary>
    /// Window width, only if <see cref="Alignment"/> is Center.
    /// </summary>
    public string? Width { get; set; } = "500px";

    /// <summary>
    /// Window height, only if <see cref="Alignment"/> is Center.
    /// </summary>
    public string? Height { get; set; } = "300px";

    /// <summary>
    /// Indicates the element is modal. When modal, user mouse interaction will be limited to the contents of the element by a modal
    /// overlay. Clicking on the overlay will cause the dialog to be dismissed.
    /// </summary>
    public bool? Modal { get; set; } = true;

    /// <summary>
    /// Indicates that the dialog should trap focus.
    /// </summary>
    public bool? TrapFocus { get; set; } = true;

    /// <summary>
    /// Determines whether the dialog title part shown or hidden.
    /// </summary>
    public bool ShowTitle { get; set; } = true;

    /// <summary>
    /// Display or hide the dismiss icon button (in the header).
    /// </summary>
    public bool ShowDismiss { get; set; } = true;

    /// <summary>
    /// Label of the primary (OK) button. If <value>null</value> button will not be shown.
    /// </summary>
    public string? PrimaryButton { get; set; } = "Ok"; //FluentPanelResources.ButtonOK;

    /// <summary>
    /// Label of the secondary (Cancel) button. If <value>null</value> button will not be shown.
    /// </summary>
    public string? SecondaryButton { get; set; } = "Cancel"; //FluentPanelResources.ButtonCancel;

    /// <summary>
    /// Gets or sets the extra styles appied to the Body content.
    /// </summary>
    public string DialogBodyStyle { get; set; } = string.Empty;

    public string? AriaLabelledby { get; set; }

    public string? AriaDescribedby { get; set; }

    public string? AriaLabel { get; set; }


    /// <summary>
    /// Default configuration for a large, almost full screen dialog.
    /// </summary>
    public static DialogSettings LargeScreen() => new()
    {
        Alignment = HorizontalAlignment.Center,
        Width = "80vw",
        Height = "95vh",
        Modal = false,
        PrimaryButton = null,
        SecondaryButton = null
    };

    /// <summary>
    /// Gets whether the primary button is displayed or not. Depends on PrimaryButton having a value.
    /// </summary>
    public bool ShowPrimaryButton => !string.IsNullOrEmpty(PrimaryButton);

    /// <summary>
    /// Gets whether the secondary button is displayed or not. Depends on SecondaryButton having a value. 
    /// </summary>
    public bool ShowSecondaryButton => !string.IsNullOrEmpty(SecondaryButton);
}
