// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The aria-live attribute in HTML is used to inform assistive technologies (like screen readers) about updates to dynamic content.
/// It helps ensure that users with disabilities are notified when content changes on the page without requiring them to manually check.
/// </summary>
public enum AriaLive
{
    /// <summary>
    /// Updates to the region will not be announced by screen readers.
    /// Use this when the content is not important for accessibility.
    /// </summary>
    [Description("off")]
    Off,

    /// <summary>
    /// Updates are announced at the next available opportunity, without interrupting the current speech.
    /// Ideal for non-urgent updates like status messages or background changes.
    /// </summary>
    [Description("polite")]
    Polite,

    /// <summary>
    /// Updates are announced immediately, interrupting whatever the screen reader is currently reading.
    /// Use this only for critical or time-sensitive information, such as error messages or alerts.
    /// </summary>
    [Description("assertive")]
    Assertive,
}
