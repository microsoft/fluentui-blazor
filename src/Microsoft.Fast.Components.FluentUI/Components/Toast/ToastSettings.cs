namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// _instance specific settings for a <see cref="FluentToast"/> component
/// </summary>
public class ToastSettings
{
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
    /// </summary>
    /// <remarks>
    /// By setting this property, you can control the duration of the notification and ensure that it is visible to the user for an appropriate amount of time.
    /// </remarks>
    public int Timeout { get; set; }

    public ToastSettings(int timeout = 7, (string Name, Color Color, IconVariant Variant)? icon = null)
    {
        Timeout = timeout;
        Icon = icon;
    }

    internal ToastSettings() { }
}
