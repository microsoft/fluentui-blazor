namespace Microsoft.Fast.Components.FluentUI;

internal static class BooleanExtensions
{
    public static string ToAttributeValue(this bool value) => value ? "true" : "false";
}
