// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Provides source-generated JSON serialization metadata for Fluent UI components.
/// </summary>
/// <remarks>
/// This API is intended for use with .NET 11 Blazor AOT and may be removed in a future release.
/// Add <see cref="Default"/> to <c>CircuitOptions.JsonTypeInfoResolvers</c> when publishing an
/// interactive server application with Native AOT.
/// </remarks>
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, WriteIndented = true)]
[JsonSerializable(typeof(Dictionary<string, object?>))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(double))]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(KeyPress))]
[JsonSerializable(typeof(KeyPress[]))]
[JsonSerializable(typeof(AccordionItemEventArgs))]
[JsonSerializable(typeof(DialogToggleEventArgs))]
[JsonSerializable(typeof(DropdownEventArgs))]
[JsonSerializable(typeof(MenuItemEventArgs))]
[JsonSerializable(typeof(OverflowChangedEventArgs))]
[JsonSerializable(typeof(OverflowChangedItem))]
[JsonSerializable(typeof(RadioEventArgs))]
[JsonSerializable(typeof(TabChangeEventArgs))]
[JsonSerializable(typeof(TreeItemChangedEventArgs))]
[JsonSerializable(typeof(TreeItemToggleEventArgs))]
[ExcludeFromCodeCoverage(Justification = "This class is used for source-generated JSON serialization and does not contain any logic to be tested.")]
[Experimental("FLUENTUI0001")]
public sealed partial class FluentUIJsonSerializerContext : JsonSerializerContext
{
}