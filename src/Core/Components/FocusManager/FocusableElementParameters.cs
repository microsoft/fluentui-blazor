namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// General focus-related parameters targeting individual elements. Can be applied to element using <see cref="IFocusManager"/>.
/// </summary>
public class FocusableElementParameters
{
    /// <summary>
    /// Marks element as default for focus.
    /// </summary>
    public bool Default { get; set; }

    /// <summary>
    /// Indicates that element's focusability should not be determined based on aria-disabled.
    /// </summary>
    public bool IgnoreAriaDisabled { get; set; }

    /// <summary>
    /// Excludes element (and all sub-elements) from arrow key navigation.
    /// </summary>
    public bool ExcludeFromArrowKeyNavigation { get; set; }

    /// <summary>
    /// Events to be ignored by <see cref="IFocusManager"/>
    /// </summary>
    public FocusManagerIgnoredKeydownEvents? IgnoredKeydownEvents { get; set; }
}
