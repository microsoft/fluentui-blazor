namespace Microsoft.Fast.Components.FluentUI;

public enum ComboboxAutocomplete
{
    Inline,
    List,
    Both
}

public static class ComboboxAutocompleteExtensions
{
    private static readonly Dictionary<ComboboxAutocomplete, string> _autocompleteValues =
        Enum.GetValues<ComboboxAutocomplete>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this ComboboxAutocomplete? value) => value == null ? null : _autocompleteValues[value.Value];
}