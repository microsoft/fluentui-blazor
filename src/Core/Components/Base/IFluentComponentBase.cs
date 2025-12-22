// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Base interface for FluentUI Blazor components.
/// </summary>
public interface IFluentComponentBase
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// The value will be used as the HTML <see href="https://developer.mozilla.org/docs/Web/HTML/Global_attributes/id">global id attribute</see>.
    /// </summary>
    string? Id { get; set; }

    /// <summary>
    /// Gets or sets the CSS class names. If given, these will be included in the class attribute of the component.
    /// </summary>
    string? Class { get; set; }

    /// <summary>
    /// Gets or sets the in-line styles. If given, these will be included in the style attribute of the component.
    /// </summary>
    string? Style { get; set; }

    /// <summary>
    /// Gets or sets the component <see href="https://developer.mozilla.org/docs/Web/CSS/margin">CSS margin</see> property.
    /// </summary>
    string? Margin { get; set; }

    /// <summary>
    /// Gets or sets the component <see href="https://developer.mozilla.org/docs/Web/CSS/padding">CSS padding</see> property.
    /// </summary>
    string? Padding { get; set; }

    /// <summary>
    /// Gets or sets custom data, to attach any user data object to the component.
    /// </summary>
    object? Data { get; set; }

    /// <summary>
    /// Gets or sets a collection of additional attributes that will be applied to the created element.
    /// </summary>
    IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
}
