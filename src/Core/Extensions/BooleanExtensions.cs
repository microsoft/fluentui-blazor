// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------
namespace Microsoft.FluentUI.AspNetCore.Components.Extensions;

internal static class BooleanExtensions
{
    public static string ToAttributeValue(this bool value) => value ? "true" : "false";
}
