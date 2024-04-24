namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The type of autocoplete for a <see cref="FluentCombobox{TValue}"/> component.
/// </summary>
public enum ComboboxAutocomplete
{
    /// <summary>
    /// The combobox will autocomplete inline.
    /// </summary>
    Inline,

    /// <summary>
    /// The combobox will autocomplete on the list values.
    /// </summary>
    List,

    /// <summary>
    /// The combobox will autocomplete inline and on the list values.
    /// </summary>
    Both
}
