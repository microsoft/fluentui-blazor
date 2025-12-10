// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Shared.Models;

/// <summary>
/// Summary of all MCP primitives.
/// </summary>
/// <param name="Tools">All tools.</param>
/// <param name="Prompts">All prompts.</param>
/// <param name="Resources">All resources.</param>
public record McpSummary(
    IReadOnlyList<McpToolInfo> Tools,
    IReadOnlyList<McpPromptInfo> Prompts,
    IReadOnlyList<McpResourceInfo> Resources);
