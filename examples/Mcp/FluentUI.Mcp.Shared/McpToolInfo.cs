// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Mcp.Shared;

/// <summary>
/// Information about an MCP tool.
/// </summary>
/// <param name="Name">The tool name.</param>
/// <param name="Description">The tool description.</param>
/// <param name="ClassName">The class containing the tool.</param>
/// <param name="Parameters">The tool parameters.</param>
public record McpToolInfo(
    string Name,
    string Description,
    string ClassName,
    IReadOnlyList<McpParameterInfo> Parameters);
