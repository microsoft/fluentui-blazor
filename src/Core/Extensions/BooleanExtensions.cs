namespace Microsoft.FluentUI.AspNetCore.Components;

internal static class BooleanExtensions
{
    public static string ToAttributeValue(this bool value) => value ? "true" : "false";
}
