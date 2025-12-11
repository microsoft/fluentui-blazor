// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Text;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Prompts;

/// <summary>
/// MCP Prompt for checking version compatibility between NuGet package and MCP Server.
/// </summary>
[McpServerPromptType]
public class CheckVersionCompatibilityPrompt
{
    private readonly FluentUIDocumentationService _documentationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckVersionCompatibilityPrompt"/> class.
    /// </summary>
    public CheckVersionCompatibilityPrompt(FluentUIDocumentationService documentationService)
    {
        _documentationService = documentationService;
    }

    [McpServerPrompt(Name = "check_version_compatibility")]
    [Description("Check if the installed Fluent UI Blazor NuGet package version is compatible with this MCP Server.")]
    public ChatMessage CheckVersionCompatibility(
        [Description("The version of Microsoft.FluentUI.AspNetCore.Components currently installed in the project (e.g., '5.0.0', '4.10.3')")] string installedVersion)
    {
        var expectedVersion = _documentationService.ComponentsVersion;
        var mcpServerVersion = FluentUIDocumentationService.McpServerVersion;
        var generatedDate = _documentationService.DocumentationGeneratedDate;

        var sb = new StringBuilder();
        sb.AppendLine("# Fluent UI Blazor Version Compatibility Check");
        sb.AppendLine();

        sb.AppendLine("## Version Information");
        sb.AppendLine();
        sb.AppendLine($"| Component | Version |");
        sb.AppendLine($"|-----------|---------|");
        sb.AppendLine($"| **MCP Server** | {mcpServerVersion} |");
        sb.AppendLine($"| **Expected Components Version** | {expectedVersion} |");
        sb.AppendLine($"| **Your Installed Version** | {installedVersion} |");
        sb.AppendLine($"| **Documentation Generated** | {generatedDate} |");
        sb.AppendLine();

        // Parse versions for comparison
        _ = CheckVersionCompatibilityInternal(installedVersion, expectedVersion, out var compatibilityLevel);

        switch (compatibilityLevel)
        {
            case VersionCompatibility.Exact:
                sb.AppendLine("## ✅ Perfect Match");
                sb.AppendLine();
                sb.AppendLine("Your installed version matches exactly with this MCP Server's documentation.");
                sb.AppendLine("All component information, parameters, and examples will be accurate.");
                break;

            case VersionCompatibility.MinorDifference:
                sb.AppendLine("## ⚠️ Minor Version Difference");
                sb.AppendLine();
                sb.AppendLine("Your installed version has a different minor or patch version.");
                sb.AppendLine("Most documentation should be accurate, but some new features or changes may not be reflected.");
                sb.AppendLine();
                sb.AppendLine("### Recommendations:");
                sb.AppendLine($"- Consider upgrading to version **{expectedVersion}** for full compatibility");
                sb.AppendLine("- Or update the MCP Server to match your installed version");
                break;

            case VersionCompatibility.MajorDifference:
                sb.AppendLine("## ❌ Major Version Mismatch");
                sb.AppendLine();
                sb.AppendLine("**Warning**: Your installed version has a different major version.");
                sb.AppendLine("There may be significant breaking changes, removed components, or new APIs not covered by this documentation.");
                sb.AppendLine();
                sb.AppendLine("### Required Actions:");
                sb.AppendLine($"1. **Upgrade your NuGet package** to version **{expectedVersion}**:");
                sb.AppendLine("   ```shell");
                sb.AppendLine($"   dotnet add package Microsoft.FluentUI.AspNetCore.Components --version {expectedVersion}");
                sb.AppendLine("   ```");
                sb.AppendLine();
                sb.AppendLine("2. **Or update the MCP Server** to match your installed version:");
                sb.AppendLine("   ```shell");
                sb.AppendLine($"   dotnet tool update --global Microsoft.FluentUI.AspNetCore.Components.McpServer --version {installedVersion}");
                sb.AppendLine("   ```");
                break;

            default:
                sb.AppendLine("## ⚠️ Unable to Determine Compatibility");
                sb.AppendLine();
                sb.AppendLine("Could not parse the version numbers for comparison.");
                sb.AppendLine($"Please verify you're using version **{expectedVersion}** of Microsoft.FluentUI.AspNetCore.Components.");
                break;
        }

        sb.AppendLine();
        sb.AppendLine("## Task");
        sb.AppendLine();
        sb.AppendLine("Based on the version compatibility check above:");
        sb.AppendLine($"1. Explain any potential issues with using version {installedVersion} when the MCP documentation is for version {expectedVersion}");
        sb.AppendLine("2. If there are breaking changes between these versions, list them");
        sb.AppendLine("3. Provide migration guidance if an upgrade is recommended");
        sb.AppendLine("4. Suggest the best course of action for the user");

        return new ChatMessage(ChatRole.User, sb.ToString());
    }

    /// <summary>
    /// Checks version compatibility and returns the level of compatibility.
    /// </summary>
    private static bool CheckVersionCompatibilityInternal(string installedVersion, string expectedVersion, out VersionCompatibility level)
    {
        level = VersionCompatibility.Unknown;

        // Clean versions (remove any prerelease suffixes for comparison)
        var installedClean = CleanVersion(installedVersion);
        var expectedClean = CleanVersion(expectedVersion);

        if (!Version.TryParse(installedClean, out var installed) ||
            !Version.TryParse(expectedClean, out var expected))
        {
            return false;
        }

        // Exact match
        if (installed.Major == expected.Major &&
            installed.Minor == expected.Minor &&
            installed.Build == expected.Build)
        {
            level = VersionCompatibility.Exact;
            return true;
        }

        // Same major version
        if (installed.Major == expected.Major)
        {
            level = VersionCompatibility.MinorDifference;
            return true;
        }

        // Different major version
        level = VersionCompatibility.MajorDifference;
        return false;
    }

    /// <summary>
    /// Cleans a version string by removing prerelease suffixes.
    /// </summary>
    private static string CleanVersion(string version)
    {
        if (string.IsNullOrEmpty(version))
        {
            return "0.0.0";
        }

        // Remove prerelease suffix (e.g., "-preview.1", "-rc.2", "-beta")
        var dashIndex = version.IndexOf('-');
        if (dashIndex > 0)
        {
            version = version[..dashIndex];
        }

        // Ensure we have at least 3 parts
        var parts = version.Split('.');
        if (parts.Length < 3)
        {
            version = string.Join(".", parts.Concat(Enumerable.Repeat("0", 3 - parts.Length)));
        }

        return version;
    }

    private enum VersionCompatibility
    {
        Unknown,
        Exact,
        MinorDifference,
        MajorDifference
    }
}
