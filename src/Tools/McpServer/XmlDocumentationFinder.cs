// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer;

/// <summary>
/// Helper class for finding XML documentation files.
/// </summary>
internal static class XmlDocumentationFinder
{
    /// <summary>
    /// Tries to find the XML documentation file for the Fluent UI Components.
    /// </summary>
    /// <returns>The path to the XML documentation file, or null if not found.</returns>
    public static string? Find()
    {
        var possiblePaths = new[]
        {
            // Same directory as executable
            Path.Combine(AppContext.BaseDirectory, "Microsoft.FluentUI.AspNetCore.Components.xml"),
            // Development paths
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Tools", "FluentUI.Demo.DocApiGen", "Microsoft.FluentUI.AspNetCore.Components.xml"),
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "Tools", "FluentUI.Demo.DocApiGen", "Microsoft.FluentUI.AspNetCore.Components.xml"),
        };

        foreach (var path in possiblePaths)
        {
            var fullPath = Path.GetFullPath(path);
            if (File.Exists(fullPath))
            {
                Console.Error.WriteLine($"[FluentUI.Mcp.Server] Found XML documentation at: {fullPath}");
                return fullPath;
            }
        }

        Console.Error.WriteLine("[FluentUI.Mcp.Server] Warning: XML documentation file not found. Descriptions may be limited.");
        return null;
    }
}
