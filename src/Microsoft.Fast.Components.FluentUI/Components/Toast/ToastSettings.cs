namespace Microsoft.Fast.Components.FluentUI;
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
    /// Gets or sets the primary call to action for the notification
    /// </summary>
    public ToastAction? PrimaryCallToAction { get; set; }

    /// <summary>
    /// Gets or sets the secondary call to action for the notification
    /// </summary>
    public ToastAction? SecondaryCallToAction { get; set; }

    /// <summary>
    /// Gets or sets the subtitle of the notification.
    /// </summary>
    public string? Subtitle { get; set; }

    /// <summary>
    /// Gets or sets the details of the notification.
    /// </summary>
    public string? Details { get; set; }

    /// <summary>
    /// Gets or sets the percentage completed for a progress notification
    /// </summary>
    public int? PercentageComplete { get; set; }

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
    public int Timeout { get; set; } = 7;

      

    public ToastSettings(
        (string Name, Color Color, IconVariant Variant)? icon = null,
        ToastAction? primaryCallToAction = null,
        ToastAction? secondaryCallToAction = null, 
        string? subtitle = null,
        string? details = null,
        int? percentageComplete = null,
        Action? onClick = null,
        int timeout = 7)
    {
        Icon = icon;
        PrimaryCallToAction = primaryCallToAction;
        SecondaryCallToAction = secondaryCallToAction;
        Subtitle = subtitle;
        Details = details;
        PercentageComplete = percentageComplete;
        OnClick = onClick;
        Timeout = timeout;
    }

    internal ToastSettings() { }
}
