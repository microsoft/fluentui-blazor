using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// Instance specific parameters for a <see cref="FluentDialog"/> component
/// </summary>
public class DialogSettings
{
    /// <summary>
    /// Gets or sets the dialog position:
    /// left (full height), right (full height)
    /// or screen middle (using Width and Height properties).
    /// </summary>
    public HorizontalAlignment Alignment { get; set; } = HorizontalAlignment.Center;

    /// <summary>
    /// Gets or sets the dialog title.
    /// </summary>
    public string? Title { get; set; }

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

    /// <summary>
    /// Identifies the element that labels the element it is applied to.
    /// </summary>
    public string? AriaLabelledby { get; set; }

    /// <summary>
    /// Identifies the element that describes the element on which the attribute is set.
    /// </summary>
    public string? AriaDescribedby { get; set; }

    /// <summary>
    /// The value that labels an interactive element.
    /// </summary>
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
    /// Callback function for the result.
    /// </summary>
    public EventCallback<DialogResult> OnDialogResult { get; set; } = default!;

    /// <summary>
    /// Gets whether the primary button is displayed or not. Depends on PrimaryButton having a value.
    /// </summary>
    internal bool ShowPrimaryButton => !string.IsNullOrEmpty(PrimaryButton);

    /// <summary>
    /// Gets whether the secondary button is displayed or not. Depends on SecondaryButton having a value. 
    /// </summary>
    internal bool ShowSecondaryButton => !string.IsNullOrEmpty(SecondaryButton);
}
