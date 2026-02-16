// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.AI;
using Microsoft.FluentUI.AspNetCore.McpServer.Tools;
using ModelContextProtocol.Server;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Prompts;

/// <summary>
/// MCP prompt for guiding users through adding the correct Fluent UI Blazor
/// package references to their project.
/// </summary>
[McpServerPromptType]
public class AddPackageReferencePrompts
{
    /// <summary>
    /// Generates a prompt that guides the user through adding the correct
    /// <c>Microsoft.FluentUI.AspNetCore.Components</c> package reference,
    /// along with optional Icons and Emoji packages.
    /// </summary>
    /// <param name="projectPath">The path to the .csproj file where the package should be added.</param>
    /// <param name="includeIcons">Whether to also add the Icons package.</param>
    /// <param name="includeEmoji">Whether to also add the Emoji package.</param>
    [McpServerPrompt(Name = "add_package_reference")]
    [Description("Guides the user through adding the correct version of the Fluent UI Blazor NuGet packages " +
                 "(Components, Icons, Emoji) to their project. Ensures the package version matches the MCP server version.")]
    public static ChatMessage AddPackageReference(
        [Description("The path to the .csproj file where the packages should be added (e.g., 'src/MyApp/MyApp.csproj').")]
        string projectPath,
        [Description("Whether to include the Microsoft.FluentUI.AspNetCore.Components.Icons package (default: true).")]
        bool includeIcons = true,
        [Description("Whether to include the Microsoft.FluentUI.AspNetCore.Components.Emoji package (default: false).")]
        bool includeEmoji = false)
    {
        var version = VersionTools.GetMcpSemanticVersion();

        var sb = new StringBuilder();

        sb.AppendLine("# Add Fluent UI Blazor Package References");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"Add the Fluent UI Blazor NuGet packages to the project `{projectPath}`.");
        sb.AppendLine();
        sb.AppendLine("## Instructions to Follow");

        AppendVerifyStep(sb, projectPath, version);
        AppendInstallStep(sb, projectPath, version, includeIcons, includeEmoji);
        AppendVerifyResultStep(sb, projectPath, version, includeIcons, includeEmoji);
        AppendNotesStep(sb, version);

        return new ChatMessage(ChatRole.User, sb.ToString());
    }

    private static void AppendVerifyStep(StringBuilder sb, string projectPath, string version)
    {
        sb.AppendLine();
        sb.AppendLine("### 1. Verify the Project File");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"Open `{projectPath}` and check if a `Microsoft.FluentUI.AspNetCore.Components` PackageReference already exists.");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"- If it exists with version `{version}`, skip to step 3.");
        sb.AppendLine(CultureInfo.InvariantCulture, $"- If it exists with a **different** version, update it to `{version}`.");
        sb.AppendLine("- If it does not exist, proceed to step 2.");
    }

    private static void AppendInstallStep(StringBuilder sb, string projectPath, string version, bool includeIcons, bool includeEmoji)
    {
        sb.AppendLine();
        sb.AppendLine("### 2. Add the NuGet Packages");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"The required version is **`{version}`** (matching this MCP server).");
        sb.AppendLine();

        AppendPackageCommand(sb, "Components (required)", null, projectPath, "Microsoft.FluentUI.AspNetCore.Components", version);

        if (includeIcons)
        {
            AppendPackageCommand(sb, "Icons (recommended)", "Provides access to the full Fluent UI icon set via `FluentIcon`.", projectPath, "Microsoft.FluentUI.AspNetCore.Components.Icons", version);
        }

        if (includeEmoji)
        {
            AppendPackageCommand(sb, "Emoji (optional)", "Provides access to the Fluent UI emoji set via `FluentEmoji`.", projectPath, "Microsoft.FluentUI.AspNetCore.Components.Emoji", version);
        }
    }

    private static void AppendPackageCommand(StringBuilder sb, string heading, string? description, string projectPath, string packageId, string version)
    {
        sb.AppendLine(CultureInfo.InvariantCulture, $"#### {heading}");
        sb.AppendLine();

        if (!string.IsNullOrEmpty(description))
        {
            sb.AppendLine(description);
            sb.AppendLine();
        }

        sb.AppendLine("```bash");
        sb.AppendLine(CultureInfo.InvariantCulture, $"dotnet add \"{projectPath}\" package {packageId} --version {version}");
        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendVerifyResultStep(StringBuilder sb, string projectPath, string version, bool includeIcons, bool includeEmoji)
    {
        sb.AppendLine("### 3. Verify the Package References");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"After adding the packages, the `{projectPath}` file should contain:");
        sb.AppendLine();
        sb.AppendLine("```xml");
        sb.AppendLine(CultureInfo.InvariantCulture, $"<PackageReference Include=\"Microsoft.FluentUI.AspNetCore.Components\" Version=\"{version}\" />");

        if (includeIcons)
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"<PackageReference Include=\"Microsoft.FluentUI.AspNetCore.Components.Icons\" Version=\"{version}\" />");
        }

        if (includeEmoji)
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"<PackageReference Include=\"Microsoft.FluentUI.AspNetCore.Components.Emoji\" Version=\"{version}\" />");
        }

        sb.AppendLine("```");
    }

    private static void AppendNotesStep(StringBuilder sb, string version)
    {
        sb.AppendLine();
        sb.AppendLine("### 4. Important Notes");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"- **All packages must use the same version** (`{version}`) to avoid compatibility issues.");
        sb.AppendLine("- If this is a prerelease version, the `--prerelease` flag may also be needed when searching for packages.");
        sb.AppendLine("- After adding packages, run `dotnet restore` to ensure they are properly resolved.");
        sb.AppendLine();
        sb.AppendLine("**Important:** Use the `CheckProjectVersion` tool to confirm the project now references the correct version.");
    }
}
