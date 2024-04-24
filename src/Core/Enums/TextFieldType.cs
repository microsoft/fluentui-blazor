namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Describes the type of input of a <see cref="FluentTextField"/>.
/// </summary>
public enum TextFieldType
{
    /// <summary>
    /// The default type of text field
    /// </summary>
    Text,

    /// <summary>
    /// A text field that is used for email address input.
    /// </summary>
    Email,

    /// <summary>
    /// A text field that is used for password input.
    /// </summary>
    Password,

    /// <summary>
    /// A text field that is used for telephone number input.
    /// </summary>
    Tel,

    /// <summary>
    /// A text field that is used for URL input.
    /// </summary>
    Url
}
