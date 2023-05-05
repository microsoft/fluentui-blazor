namespace Microsoft.Fast.Components.FluentUI;
public class ToastSettings
{

    /// <summary>
    /// Gets or sets the name of the icon to display with the link
    /// Use a constant value from the <see cref="FluentIcons" /> class 
    /// </summary>
    public string? Icon { get; set; } = string.Empty;

    /// <summary>
    /// The <c>IconColor</c> property determines the color of the icon.
    /// It is based on either the <see cref="ToastIntent"/> or the active Accent color
    /// </summary>
    public Color IconColor { get; set; } = Color.Accent;


    /// <summary>
    /// The <c>IconVariant</c> property determines the variant of the icon.
    /// For the Intent related icons it is IconVariant.Filled by default.
    /// For the custom icons it is IconVariant.Regular by default.
    /// </summary>
    public IconVariant IconVariant { get; set; } = IconVariant.Regular;
    
    /// <summary>
    /// The ShowCloseButton property determines whether or not the close button is displayed on the toast notification.
    /// </summary>
    public bool ShowCloseButton { get; set; }

    /// <summary>
    /// The <c>OnClick</c> property is an optional action that is triggered when the user clicks on the toast notification.
    /// </summary>
    /// <remarks>
    /// This property allows you to define a custom action that will be executed when the user interacts with the notification, such as opening a new window or performing some other action.
    /// </remarks>
    public Action? OnClick { get; set; }

    /// <summary>
    /// The <c>Timeout</c> property determines the amount of time, in seconds, that the toast notification will be displayed before it is automatically closed.
    /// </summary>
    /// <remarks>
    /// By setting this property, you can control the duration of the notification and ensure that it is visible to the user for an appropriate amount of time.
    /// </remarks>
    public int Timeout { get; set; }

      

    public ToastSettings(
        string? icon,
        Color iconColor,
        IconVariant iconVariant,
        bool showCloseButton,
        Action? onClick,
        int timeout)
    {
        Icon = icon;
        IconColor = iconColor;
        IconVariant = iconVariant;
        ShowCloseButton = showCloseButton;
        OnClick = onClick;
        Timeout = timeout;
    }

    internal ToastSettings() { }
}
