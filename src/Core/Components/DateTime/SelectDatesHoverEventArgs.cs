// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Arguments for the <see cref="FluentCalendar.SelectDatesHover"/> event.
/// </summary>
public record SelectDatesHoverEventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SelectDatesHoverEventArgs"/> class.
    /// </summary>
    /// <param name="value"></param>
    internal SelectDatesHoverEventArgs(DateTime value)
    {
        Start = value;
        End = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectDatesHoverEventArgs"/> class.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    internal SelectDatesHoverEventArgs(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    /// <summary>
    /// Gets or sets the start date of the selection.
    /// </summary>
    public DateTime Start { get; set; }

    /// <summary>
    /// Gets or sets the end date of the selection.
    /// </summary>
    public DateTime End { get; set; }
}
