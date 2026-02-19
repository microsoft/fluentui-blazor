// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using ModelContextProtocol.Server;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tools;

/// <summary>
/// MCP tools for checking version information and ensuring consistency
/// between the MCP server and the referenced Fluent UI Blazor component library.
/// </summary>
[McpServerToolType]
public class VersionTools
{
    /// <summary>
    /// Gets version information for the MCP server and the recommended
    /// <c>Microsoft.FluentUI.AspNetCore.Components</c> package version.
    /// </summary>
    /// <returns>
    /// A string containing the MCP server version, the recommended component library version,
    /// and the expected PackageReference to use.
    /// </returns>
    [McpServerTool]
    [Description("Gets version information for the Fluent UI Blazor MCP server and returns the recommended " +
                 "Microsoft.FluentUI.AspNetCore.Components package version to use. " +
                 "Call this tool first, then read the user's .csproj to verify the PackageReference matches.")]
    public static string GetVersionInfo()
    {
        var mcpVersion = GetMcpSemanticVersion();

        var sb = new StringBuilder();

        sb.AppendLine("# Fluent UI Blazor - Version Information");
        sb.AppendLine();
        sb.AppendLine("## MCP Server");
        sb.AppendLine(CultureInfo.InvariantCulture, $"- **Package**: Microsoft.FluentUI.AspNetCore.McpServer");
        sb.AppendLine(CultureInfo.InvariantCulture, $"- **Version**: {mcpVersion}");
        sb.AppendLine();
        sb.AppendLine("## Required Component Library Version");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"This MCP server provides documentation for **version `{mcpVersion}`** of the component library.");
        sb.AppendLine("The user's project **must** reference the same version to ensure accurate documentation.");
        sb.AppendLine();
        sb.AppendLine("### Expected PackageReference");
        sb.AppendLine();
        sb.AppendLine("```xml");
        sb.AppendLine(CultureInfo.InvariantCulture, $"<PackageReference Include=\"Microsoft.FluentUI.AspNetCore.Components\" Version=\"{mcpVersion}\" />");
        sb.AppendLine("```");
        sb.AppendLine();
        sb.AppendLine("### Next Step");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"Read the user's `.csproj` file and call `CheckProjectVersion` with the version found in their " +
                      $"`Microsoft.FluentUI.AspNetCore.Components` PackageReference to validate compatibility.");

        return sb.ToString();
    }

    /// <summary>
    /// Checks whether the user's project references the correct version of
    /// <c>Microsoft.FluentUI.AspNetCore.Components</c> for this MCP server.
    /// </summary>
    /// <param name="projectVersion">The version found in the user's .csproj PackageReference
    /// for Microsoft.FluentUI.AspNetCore.Components (e.g., '5.0.0-rc.1-26049.2').</param>
    /// <returns>
    /// A string indicating whether the versions match, or a warning with upgrade instructions.
    /// </returns>
    [McpServerTool]
    [Description("Checks whether the user's project references the correct version of Microsoft.FluentUI.AspNetCore.Components " +
                 "for this MCP server. Pass the version string found in the user's .csproj PackageReference. " +
                 "If versions do not match, the documentation provided by this MCP server may be inaccurate.")]
    public static string CheckProjectVersion(
        [Description("The version of Microsoft.FluentUI.AspNetCore.Components found in the user's .csproj PackageReference (e.g., '5.0.0-rc.1-26049.2').")]
        string projectVersion)
    {
        if (string.IsNullOrWhiteSpace(projectVersion))
        {
            return "Please provide the version of Microsoft.FluentUI.AspNetCore.Components from the user's .csproj PackageReference (e.g., '5.0.0-rc.1-26049.2').";
        }

        var mcpVersion = GetMcpSemanticVersion();
        var normalizedProjectVersion = ExtractSemanticVersion(projectVersion.Trim());

        var sb = new StringBuilder();

        sb.AppendLine("# Version Compatibility Check");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"- **MCP Server version**: `{mcpVersion}`");
        sb.AppendLine(CultureInfo.InvariantCulture, $"- **Project version**: `{normalizedProjectVersion}`");
        sb.AppendLine();

        if (string.Equals(mcpVersion, normalizedProjectVersion, StringComparison.OrdinalIgnoreCase))
        {
            sb.AppendLine("## Result: COMPATIBLE");
            sb.AppendLine();
            sb.AppendLine("The project references the correct version of `Microsoft.FluentUI.AspNetCore.Components`. " +
                          "All documentation, parameters, events, and code examples from this MCP server are accurate for this version.");
        }
        else
        {
            sb.AppendLine("## Result: INCOMPATIBLE");
            sb.AppendLine();
            sb.AppendLine(CultureInfo.InvariantCulture, $"**WARNING**: The project references version `{normalizedProjectVersion}` " +
                          $"but this MCP server provides documentation for version `{mcpVersion}`.");
            sb.AppendLine();
            sb.AppendLine("### Risks");
            sb.AppendLine();
            sb.AppendLine("- Component parameters, events, or methods may have changed between versions.");
            sb.AppendLine("- Code examples may not compile or behave differently.");
            sb.AppendLine("- New components may be missing or deprecated components may still be documented.");
            sb.AppendLine();
            sb.AppendLine("### Recommended Action");
            sb.AppendLine();
            sb.AppendLine(CultureInfo.InvariantCulture, $"Update the project's PackageReference to match the MCP server version:");
            sb.AppendLine();
            sb.AppendLine("```xml");
            sb.AppendLine(CultureInfo.InvariantCulture, $"<PackageReference Include=\"Microsoft.FluentUI.AspNetCore.Components\" Version=\"{mcpVersion}\" />");
            sb.AppendLine("```");
            sb.AppendLine();
            sb.AppendLine(CultureInfo.InvariantCulture, $"Or install via the command line:");
            sb.AppendLine();
            sb.AppendLine("```bash");
            sb.AppendLine(CultureInfo.InvariantCulture, $"dotnet add package Microsoft.FluentUI.AspNetCore.Components --version {mcpVersion}");
            sb.AppendLine("```");
            sb.AppendLine();
            sb.AppendLine("If you cannot update the package version, be aware that the documentation provided by this MCP server " +
                          "may not be fully accurate for your project.");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Gets the semantic version of the MCP server (without build metadata).
    /// </summary>
    internal static string GetMcpSemanticVersion()
    {
        var mcpAssembly = typeof(VersionTools).Assembly;
        var informationalVersion = GetInformationalVersion(mcpAssembly);
        return ExtractSemanticVersion(informationalVersion);
    }

    /// <summary>
    /// Gets the informational version (full semver including prerelease and build metadata)
    /// from the given assembly.
    /// </summary>
    private static string GetInformationalVersion(Assembly assembly)
    {
        var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        return attribute?.InformationalVersion ?? assembly.GetName().Version?.ToString() ?? "unknown";
    }

    /// <summary>
    /// Extracts the semantic version portion (e.g., "5.0.0-rc.1-26049.2") from an informational version string
    /// that may include build metadata (e.g., "5.0.0-rc.1-26049.2+abc123").
    /// </summary>
    private static string ExtractSemanticVersion(string version)
    {
        if (string.IsNullOrEmpty(version))
        {
            return string.Empty;
        }

        var plusIndex = version.IndexOf('+', StringComparison.Ordinal);
        return plusIndex >= 0 ? version[..plusIndex] : version;
    }
}
