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
/// </remarks>
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, WriteIndented = true)]
[JsonSerializable(typeof(Dictionary<string, object?>))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(double))]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(KeyPress))]
[JsonSerializable(typeof(KeyPress[]))]
[ExcludeFromCodeCoverage(Justification = "This class is used for source-generated JSON serialization and does not contain any logic to be tested.")]
[Experimental("FLUENTUI0001")]
public sealed partial class FluentUIJsonSerializerContext : JsonSerializerContext
{
}