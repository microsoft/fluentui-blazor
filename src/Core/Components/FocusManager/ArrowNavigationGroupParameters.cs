namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Parameters of arrow navigation inside group that can be applied to the group element using <see cref="IFocusManager"/>.
/// </summary>
public sealed class ArrowNavigationGroupParameters
{
    /// <summary>
    /// Defines what arrow keys move focus inside the group of elements. See <see cref="ArrowNavigationGroupMode"/>.
    /// </summary>
    /// <remarks>
    /// Home, End, PageUp and PageDown keys can be used as well.
    /// </remarks>
    public ArrowNavigationGroupMode Mode { get; set; }

    /// <summary>
    /// Defines minimum visibility of element that can receive focus when focus enters the group.
    /// </summary>
    /// <remarks>
    /// <see cref="FocusableElementVisibility.Invisible"/> value might make the list scroll and cause the virtual list to load more items.
    /// </remarks>
    public FocusableElementVisibility FirstFocusedMinimumVisibility { get; set; }

    /// <summary>
    /// Defines behavior when focus re-enters the group.
    /// If set to <see langword="true" />, focus will go to the previously focused element (if available).
    /// If set to <see langword="false" />, focus will go to the first/last element inside the group.
    /// </summary>
    public bool RememberLastFocused { get; set; }

    /// <summary>
    /// Defines whether Tab can be used to navigate elements inside the group.
    /// By default when Tab is pressed, focus leaves the group.
    /// </summary>
    public bool NavigationWithTab { get; set; }

    /// <summary>
    /// Defines behavior when first/last element is focused and arrow key is pressed.
    /// If set to <see langword="true" />
    /// <list type="bullet">
    /// <listheader>
    /// <description>if first element is focused and Up button is pressed, focus will go to the last element in the group.</description>
    /// </listheader>
    /// <item>
    /// <description>if last element is focused and Down button is pressed, focus will go to the first element in the group.</description>
    /// </item>
    /// </list>
    /// By default nothing happens if Up button is pressed when the first element is selected or Down button is pressed when the last element is select.
    /// </summary>
    public bool Circular { get; set; }
}
