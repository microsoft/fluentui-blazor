// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.DocApiGen.Models.AllMode;

/// <summary>
/// Represents a method of a component.
/// </summary>
public class MethodInfo
{
    /// <summary>
    /// Gets or sets the name of the method.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the return type of the method.
    /// </summary>
    public string ReturnType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the method.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the parameters of the method.
    /// </summary>
    public string[] Parameters { get; set; } = [];

    /// <summary>
    /// Gets or sets whether this method is inherited.
    /// </summary>
    public bool IsInherited { get; set; }
}
