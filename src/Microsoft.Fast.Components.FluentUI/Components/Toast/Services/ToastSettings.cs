using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// Instance specific settings for a <see cref="FluentToast"/> component
/// </summary>
public class ToastSettings : IToastData
{
    /// <summary>
    /// The intent of the toast.
    /// </summary>
    public ToastIntent Intent { get; set; }

    /// <summary>
    /// The main text of the toast.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// The type of the top Call To Action.
    /// </summary>
    public ToastTopCTAType TopCTAType { get; set; }

    /// <summary>
    /// If the top CTA is of type <see cref="ToastTopCTAType.Action"/>, this is the action to be performed.
    /// </summary>  
    public ToastAction? TopAction { get; set; }

    /// <summary>
    /// The timestamp of the toast. If the top CTA is of type <see cref="ToastTopCTAType.Timestamp"/>, this is the value displayed.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the the icon to display in the notification
    /// Use a constant from the <see cref="FluentIcons" /> class for the <c>Name</c> value
    /// The <c>Color</c> value determines the display color of the icon.
    /// It is based on either the <see cref="ToastIntent"/> or the active Accent color 
    /// The <c>Variant</c> value determines the variant of the icon.
    /// For the intents Success, Warning, Error and Information the defualt is IconVariant.Filled.
    /// For all other intents the default is IconVariant.Regular.
    /// </summary>
    public (string Name, Color Color, IconVariant Variant)? Icon { get; set; }

    /// <summary>
    /// The <c>Timeout</c> property determines the amount of time, in seconds, that the toast notification will be displayed before it is automatically closed.
    /// Defaults to the timeout of the <see cref="FluentToastContainer"/> if not specified.
    /// </summary>
    /// <remarks>
    /// By setting this property, you can control the duration of the notification and ensure that it is visible to the user for an appropriate amount of time.
    /// </remarks>
    public int? Timeout { get; set; }


    public ToastAction? PrimaryAction { get; set; }

    public ToastAction? SecondaryAction { get; set; }

    /// <summary>
    /// Callback function for the result.
    /// </summary>
    public EventCallback<ToastResult> OnToastResult { get; set; } = default!;

}
