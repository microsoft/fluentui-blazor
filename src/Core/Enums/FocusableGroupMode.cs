using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Values to define Tab behavior for the group element which contains other focusable elements.
/// </summary>
public enum FocusableGroupMode
{
    /// <summary>
    /// The group will not be focusable.
    /// </summary>
    [Description("off")]
    Off,

    /// <summary>
    /// This behaviour traps the focus inside of the group when pressing the Enter key and will only release focus when pressing the Escape key.
    /// </summary>
    [Description("no-tab")]
    NoTab,

    /// <summary>
    /// This behaviour traps the focus inside of the group when pressing the Enter key but will release focus when pressing the Tab key on the last inner element.
    /// </summary>
    [Description("tab-exit")]
    TabExit,

    /// <summary>
    /// This behaviour will cycle through all elements inside of the group when pressing the Tab key and then release focus after the last inner element.
    /// </summary>
    [Description("tab-only")]
    TabOnly
}