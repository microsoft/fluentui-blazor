namespace Microsoft.Fast.Components.FluentUI;

public enum MenuItemRole
{
    MenuItem,
    MenuItemCheckbox,
    MenuItemRadio
}

public static class MenuItemRoleExtensions
{
    private static readonly Dictionary<MenuItemRole, string> _mirValues =
        Enum.GetValues<MenuItemRole>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this MenuItemRole? value) => value == null ? null : _mirValues[value.Value];
}