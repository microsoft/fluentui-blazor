// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Defines an interface for components with values that are sensitive to culture settings, eg : parsing to string.
/// </summary>
public interface ICultureSensitiveComponent
{
    /// <summary>
    /// Gets or sets the culture of the component.
    /// </summary>
    public CultureInfo Culture { get; set; }
}
