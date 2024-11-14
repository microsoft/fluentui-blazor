// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------
namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Base interface for Fluent UI Blazor components.
/// </summary>
public interface IFluentComponentBase
{
    /// <summary>
    /// Gets or sets a collection of additional attributes that will be applied to the created element.
    /// </summary>
    IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Gets or sets the CSS class names. If given, these will be included in the class attribute of the component.
    /// </summary>
    string? Class { get; set; }

    /// <summary>
    /// Gets or sets custom data. Used to attach any custom data object to the component.
    /// </summary>
    object? Data { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier.
    /// The value will be used as the HTML <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/id">global id attribute</see>.
    /// </summary>
    string? Id { get; set; }

    /// <summary>
    /// Gets or sets the in-line styles. If given, these will be included in the style attribute of the component.
    /// </summary>
    string? Style { get; set; }
}
