// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The Option element is used to define a `string` item contained in a List component.
/// This is a convenience class so that you don't have to specify the generic type parameter or
/// to have a weird syntax in Razor like `FluentOption TValue="string" Value="@("My value")"`.
/// </summary>
public class FluentOptionString : FluentOption<string>
{
    /// <summary />
    public FluentOptionString(LibraryConfiguration configuration) : base(configuration) { }
}
