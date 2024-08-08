// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
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
