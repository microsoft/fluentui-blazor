// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Shared.Models;

/// <summary>
/// Information about an MCP parameter.
/// </summary>
/// <param name="Name">The parameter name.</param>
/// <param name="Type">The parameter type.</param>
/// <param name="Description">The parameter description.</param>
/// <param name="Required">Whether the parameter is required.</param>
public record McpParameterInfo(
    string Name,
    string Type,
    string Description,
    bool Required);
