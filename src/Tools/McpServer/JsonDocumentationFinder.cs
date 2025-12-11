// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer;

/// <summary>
/// Helper class for finding JSON documentation files.
/// </summary>
internal static class JsonDocumentationFinder
{
    private const string JsonFileName = "FluentUIComponentsDocumentation.json";

    /// <summary>
    /// Tries to find the JSON documentation file for the Fluent UI Components.
    /// </summary>
    /// <returns>The path to the JSON documentation file, or null if not found (will use embedded resource).</returns>
    public static string? Find()
    {
        var possiblePaths = new[]
        {
            // Same directory as executable
            Path.Combine(AppContext.BaseDirectory, JsonFileName),
            // Development paths - relative to McpServer project
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", JsonFileName),
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Tools", "McpServer", JsonFileName),
            // Development paths - relative to solution
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "src", "Tools", "McpServer", JsonFileName),
        };

        foreach (var path in possiblePaths)
        {
            var fullPath = Path.GetFullPath(path);
            if (File.Exists(fullPath))
            {
                Console.Error.WriteLine($"[FluentUI.Mcp.Server] Found JSON documentation at: {fullPath}");
                return fullPath;
            }
        }

        // No external file found - will use embedded resource
        Console.Error.WriteLine("[FluentUI.Mcp.Server] No external JSON documentation file found. Using embedded resource.");
        return null;
    }
}
