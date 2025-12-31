// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.McpServer;

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
            Path.Combine(AppContext.BaseDirectory, "../../..", JsonFileName),
            Path.Combine(AppContext.BaseDirectory, "../../../../Tools/McpServer", JsonFileName),
            // Development paths - relative to solution
            Path.Combine(AppContext.BaseDirectory, "../../../../../src/Tools/McpServer", JsonFileName),
        };

        var foundPath = possiblePaths
            .Select(Path.GetFullPath)
            .FirstOrDefault(File.Exists);

        if (foundPath != null)
        {
            Console.Error.WriteLine($"{McpServerConstants.LogPrefix} Found JSON documentation at: {foundPath}");
            return foundPath;
        }

        // No external file found - will use embedded resource
        Console.Error.WriteLine($"{McpServerConstants.LogPrefix} No external JSON documentation file found. Using embedded resource.");
        return null;
    }
}
