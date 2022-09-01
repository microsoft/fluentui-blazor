namespace Microsoft.Fast.Components.FluentUI;

public enum DividerRole
{
    Separator,
    Presentation
}

public static class DividerRoleExtensions
{
    private static readonly Dictionary<DividerRole, string> _dividerRoleValues =
        Enum.GetValues<DividerRole>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this DividerRole? value) => value == null ? null : _dividerRoleValues[value.Value];
}