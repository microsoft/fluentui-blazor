// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Text;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;
using ModelContextProtocol.Server;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tools;

/// <summary>
/// MCP tools for version information and compatibility checking.
/// </summary>
[McpServerToolType]
public class VersionTools
{
    private readonly FluentUIDocumentationService _documentationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionTools"/> class.
    /// </summary>
    /// <param name="documentationService">The documentation service.</param>
    public VersionTools(FluentUIDocumentationService documentationService)
    {
        _documentationService = documentationService;
    }

    /// <summary>
    /// Gets version information about the MCP Server and the documented components.
    /// </summary>
    [McpServerTool(Name = "GetVersionInfo")]
    [Description("Get version information about the MCP Server and the Fluent UI Blazor components it documents.")]
    public string GetVersionInfo()
    {
        var sb = new StringBuilder();

        sb.AppendLine("# Fluent UI Blazor MCP Server - Version Information");
        sb.AppendLine();
        sb.AppendLine("| Property | Value |");
        sb.AppendLine("|----------|-------|");
        sb.AppendLine($"| MCP Server Version | {FluentUIDocumentationService.McpServerVersion} |");
        sb.AppendLine($"| Components Version | {_documentationService.ComponentsVersion} |");
        sb.AppendLine($"| Documentation Generated | {_documentationService.DocumentationGeneratedDate} |");
        sb.AppendLine($"| Documentation Available | {(_documentationService.IsAvailable ? "Yes" : "No")} |");
        sb.AppendLine();

        if (_documentationService.IsAvailable)
        {
            var components = _documentationService.GetAllComponents();
            var enums = _documentationService.GetAllEnums();
            var categories = _documentationService.GetCategories();

            sb.AppendLine("## Documentation Statistics");
            sb.AppendLine();
            sb.AppendLine($"- **Components**: {components.Count}");
            sb.AppendLine($"- **Enums**: {enums.Count}");
            sb.AppendLine($"- **Categories**: {categories.Count}");
        }

        sb.AppendLine();
        sb.AppendLine("## Compatibility");
        sb.AppendLine();
        sb.AppendLine($"This MCP Server provides documentation for **Microsoft.FluentUI.AspNetCore.Components** version **{_documentationService.ComponentsVersion}**.");
        sb.AppendLine();
        sb.AppendLine("For best results, ensure your project uses the same version of the NuGet package:");
        sb.AppendLine();
        sb.AppendLine("```shell");
        sb.AppendLine($"dotnet add package Microsoft.FluentUI.AspNetCore.Components --version {_documentationService.ComponentsVersion}");
        sb.AppendLine("```");

        return sb.ToString();
    }

    /// <summary>
    /// Checks if a specific version is compatible with this MCP Server's documentation.
    /// </summary>
    [McpServerTool(Name = "CheckVersionCompatibility")]
    [Description("Check if a specific NuGet package version is compatible with this MCP Server's documentation.")]
    public string CheckVersionCompatibility(
        [Description("The version to check (e.g., '5.0.0', '4.10.3')")] string version)
    {
        var expectedVersion = _documentationService.ComponentsVersion;
        var sb = new StringBuilder();

        sb.AppendLine("# Version Compatibility Check");
        sb.AppendLine();
        sb.AppendLine($"- **Your Version**: {version}");
        sb.AppendLine($"- **MCP Documentation Version**: {expectedVersion}");
        sb.AppendLine();

        var (isCompatible, message) = CompareVersions(version, expectedVersion);

        if (isCompatible)
        {
            sb.AppendLine("## ✅ Compatible");
            sb.AppendLine();
            sb.AppendLine(message);
        }
        else
        {
            sb.AppendLine("## ⚠️ Compatibility Warning");
            sb.AppendLine();
            sb.AppendLine(message);
            sb.AppendLine();
            sb.AppendLine("### Recommended Actions");
            sb.AppendLine();
            sb.AppendLine("**Option 1**: Update your NuGet package to match the MCP Server:");
            sb.AppendLine("```shell");
            sb.AppendLine($"dotnet add package Microsoft.FluentUI.AspNetCore.Components --version {expectedVersion}");
            sb.AppendLine("```");
            sb.AppendLine();
            sb.AppendLine("**Option 2**: Update the MCP Server to match your package version:");
            sb.AppendLine("```shell");
            sb.AppendLine($"dotnet tool update --global Microsoft.FluentUI.AspNetCore.Components.McpServer --version {version}");
            sb.AppendLine("```");
        }

        return sb.ToString();
    }

    private static (bool IsCompatible, string Message) CompareVersions(string userVersion, string expectedVersion)
    {
        // Clean versions
        var userClean = CleanVersion(userVersion);
        var expectedClean = CleanVersion(expectedVersion);

        if (!Version.TryParse(userClean, out var user) ||
            !Version.TryParse(expectedClean, out var expected))
        {
            return (false, $"Unable to parse version '{userVersion}'. Please use semantic versioning (e.g., '5.0.0').");
        }

        // Exact match
        if (user.Major == expected.Major && user.Minor == expected.Minor)
        {
            if (user.Build == expected.Build)
            {
                return (true, "Perfect match! Your version exactly matches the MCP Server documentation.");
            }

            return (true, $"Minor patch difference. Your version ({userVersion}) is close to the documented version ({expectedVersion}). Most features should work correctly.");
        }

        // Same major version
        if (user.Major == expected.Major)
        {
            return (false, $"Minor version mismatch. Your version ({userVersion}) may have different features than documented ({expectedVersion}).");
        }

        // Different major version
        return (false, $"Major version mismatch! Your version ({userVersion}) is significantly different from the documented version ({expectedVersion}). Breaking changes are likely.");
    }

    private static string CleanVersion(string version)
    {
        if (string.IsNullOrEmpty(version))
        {
            return "0.0.0";
        }

        // Remove prerelease suffix
        var dashIndex = version.IndexOf('-');
        if (dashIndex > 0)
        {
            version = version[..dashIndex];
        }

        // Ensure 3 parts
        var parts = version.Split('.');
        if (parts.Length < 3)
        {
            version = string.Join(".", parts.Concat(Enumerable.Repeat("0", 3 - parts.Length)));
        }

        return version;
    }
}
