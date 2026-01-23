// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Prompts;

/// <summary>
/// MCP prompts for helping users set up Fluent UI Blazor projects.
/// </summary>
[McpServerPromptType]
public class SetupProjectPrompts
{
    /// <summary>
    /// Generates a prompt to help users set up a new Fluent UI Blazor project.
    /// </summary>
    /// <param name="projectType">The type of project (e.g., 'blazor-server', 'blazor-webassembly', 'blazor-webapp').</param>
    /// <param name="includeIcons">Whether to include the Icons package.</param>
    [McpServerPrompt(Name = "setup_project")]
    [Description("Generates step-by-step instructions for setting up a new Fluent UI Blazor project from scratch.")]
    public static ChatMessage SetupProject(
        [Description("The type of Blazor project: 'blazor-server', 'blazor-webassembly', or 'blazor-webapp' (default)")]
        string projectType = "blazor-webapp",
        [Description("Whether to include the Fluent UI Icons package (default: true)")]
        bool includeIcons = true)
    {
        var sb = new StringBuilder();
        AppendHeader(sb, projectType);
        AppendInstallationSteps(sb, includeIcons);
        AppendConfigurationSteps(sb, includeIcons);
        AppendTestingAndNotes(sb);

        return new ChatMessage(ChatRole.User, sb.ToString());
    }

    private static void AppendHeader(StringBuilder sb, string projectType)
    {
        sb.AppendLine("# Fluent UI Blazor Project Setup Guide");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"Help the user set up a new **{projectType}** Blazor project with Fluent UI Blazor components.");
        sb.AppendLine();
        sb.AppendLine("## Instructions to Follow");
    }

    private static void AppendInstallationSteps(StringBuilder sb, bool includeIcons)
    {
        sb.AppendLine();
        sb.AppendLine("### 1. Install the NuGet Package");
        sb.AppendLine();
        sb.AppendLine("Install `Microsoft.FluentUI.AspNetCore.Components` package (with `--prerelease` flag for v5).");

        if (includeIcons)
        {
            sb.AppendLine();
            sb.AppendLine("Also install `Microsoft.FluentUI.AspNetCore.Components.Icons` package for icon support.");
        }
    }

    private static void AppendConfigurationSteps(StringBuilder sb, bool includeIcons)
    {
        sb.AppendLine();
        sb.AppendLine("### 2. Add Imports to `_Imports.razor`");
        sb.AppendLine();
        sb.AppendLine("Add the following using statements:");
        sb.AppendLine();
        sb.AppendLine("- `@using Microsoft.FluentUI.AspNetCore.Components`");

        if (includeIcons)
        {
            sb.AppendLine("- `@using Icons = Microsoft.FluentUI.AspNetCore.Components.Icons`");
        }

        sb.AppendLine();
        sb.AppendLine("### 3. Add Stylesheet Reference");
        sb.AppendLine();
        sb.AppendLine("In `App.razor` or your main HTML file, add a link to the Fluent UI CSS bundle:");
        sb.AppendLine();
        sb.AppendLine("- Path: `_content/Microsoft.FluentUI.AspNetCore.Components/Microsoft.FluentUI.AspNetCore.Components.bundle.scp.css`");
        sb.AppendLine();
        sb.AppendLine("### 4. Register Services in `Program.cs`");
        sb.AppendLine();
        sb.AppendLine("Call `builder.Services.AddFluentUIComponents();` to register the required services.");
        sb.AppendLine();
        sb.AppendLine("### 5. Add FluentProviders to Layout");
        sb.AppendLine();
        sb.AppendLine("In `MainLayout.razor`, add `<FluentProviders />` at the end of the layout.");
        sb.AppendLine();
        sb.AppendLine("### 6. Ensure Interactive Rendering");
        sb.AppendLine();
        sb.AppendLine("Fluent UI Blazor requires interactive rendering. Make sure your app is not using static rendering.");
    }

    private static void AppendTestingAndNotes(StringBuilder sb)
    {
        sb.AppendLine();
        sb.AppendLine("### 7. Test the Installation");
        sb.AppendLine();
        sb.AppendLine("Create a simple test page with a `FluentButton` component to verify the installation works correctly.");
        sb.AppendLine();
        sb.AppendLine("## Important Notes");
        sb.AppendLine();
        sb.AppendLine("- For v5 prerelease, you may need to add a custom NuGet source.");
        sb.AppendLine("- Check the [Installation documentation](https://www.fluentui-blazor.net/installation) for the latest instructions.");
        sb.AppendLine("- Explore the [FluentLayout](https://www.fluentui-blazor.net/layout) documentation to understand layout strategies.");
        sb.AppendLine();
        sb.AppendLine("**Important:** Use the available MCP tools to retrieve component documentation and code examples for the project setup.");
    }
}
