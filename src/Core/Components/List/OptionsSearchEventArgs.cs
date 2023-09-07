﻿namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// <see cref="FluentAutocomplete{TOption}"/> uses this event to return the list of items to display.
/// </summary>
/// <typeparam name="T"></typeparam>
public class OptionsSearchEventArgs<T>
{
    /// <summary>
    /// Text to search.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// List of items to display.
    /// </summary>
    public IEnumerable<T>? Items { get; set; }
}
