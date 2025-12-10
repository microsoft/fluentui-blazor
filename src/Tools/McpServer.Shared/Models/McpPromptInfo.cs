// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Shared.Models;

/// <summary>
/// Information about an MCP prompt.
/// </summary>
/// <param name="Name">The prompt name.</param>
/// <param name="Description">The prompt description.</param>
/// <param name="ClassName">The class containing the prompt.</param>
/// <param name="Parameters">The prompt parameters.</param>
public record McpPromptInfo(
    string Name,
    string Description,
    string ClassName,
    IReadOnlyList<McpParameterInfo> Parameters);
