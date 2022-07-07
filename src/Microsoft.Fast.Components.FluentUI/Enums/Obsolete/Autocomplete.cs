namespace Microsoft.Fast.Components.FluentUI;

[Obsolete("Use ComboboxAutoComplete instead", true)]
public enum Autocomplete
{
    Inline,
    List,
    Both
}

[Obsolete("Use ComboboxAutoCompleteExtensions instead", true)]
internal static class AutocompleteExtensions
{
    private static readonly Dictionary<Autocomplete, string> _autocompleteValues =
        Enum.GetValues<Autocomplete>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this Autocomplete? value) => value == null ? null : _autocompleteValues[value.Value];
}