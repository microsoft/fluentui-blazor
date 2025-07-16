// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

public enum CalendarSelectMode
{
    /// <summary>
    /// Allow only one selected date.
    /// </summary>
    Single,

    /// <summary>
    /// Allow a contiguous range of selected dates.
    /// </summary>
    Range,

    /// <summary>
    /// Allow multiple selected dates.
    /// </summary>
    Multiple,
}
