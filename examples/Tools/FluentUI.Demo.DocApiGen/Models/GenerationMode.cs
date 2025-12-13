// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.DocApiGen.Models;

/// <summary>
/// Defines the generation modes for documentation.
/// </summary>
public enum GenerationMode
{
    /// <summary>
    /// Generate summary documentation with only [Parameter] properties.
    /// This is the default mode used by ApiClassGenerator.
    /// </summary>
    Summary,

    /// <summary>
    /// Generate complete documentation including all properties, methods, and events.
    /// This mode is used by McpDocumentationGenerator.
    /// </summary>
    All
}
