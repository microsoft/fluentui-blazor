// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Text;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;
using ModelContextProtocol.Server;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Resources;

/// <summary>
/// MCP Resources for documentation guides (Installation, Migration, Styles, etc.).
/// </summary>
[McpServerResourceType]
public class GuideResources
{
    private readonly DocumentationGuideService _guideService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GuideResources"/> class.
    /// </summary>
    public GuideResources(DocumentationGuideService guideService)
    {
        _guideService = guideService;
    }

    /// <summary>
    /// Gets the list of all available documentation guides.
    /// </summary>
    [McpServerResource(
        UriTemplate = "fluentui://guides",
        Name = "guides",
        Title = "Documentation Guides",
        MimeType = "text/markdown")]
    [Description("List of all available documentation guides: Installation, Default Values, Migration, Localization, and Styles.")]
    public string GetAllGuides()
    {
        var guides = _guideService.GetAllGuides();

        var sb = new StringBuilder();
        sb.AppendLine("# Fluent UI Blazor Documentation Guides");
        sb.AppendLine();

        if (guides.Count == 0)
        {
            sb.AppendLine("⚠️ No guides found. Documentation files may not be available.");
            sb.AppendLine();
            sb.AppendLine("When running from source, ensure the Demo project documentation is accessible.");
            return sb.ToString();
        }

        sb.AppendLine("| Guide | Description |");
        sb.AppendLine("|-------|-------------|");
        sb.AppendLine("| [Installation](fluentui://guide/installation) | How to install and configure Fluent UI Blazor |");
        sb.AppendLine("| [Default Values](fluentui://guide/defaultvalues) | Configure default component values globally |");
        sb.AppendLine("| [What's New](fluentui://guide/whatsnew) | Latest release notes and changes |");
        sb.AppendLine("| [Migration to v5](fluentui://guide/migration) | Complete guide for migrating from v4 to v5 |");
        sb.AppendLine("| [Localization](fluentui://guide/localization) | Translate and localize component texts |");
        sb.AppendLine("| [Styles](fluentui://guide/styles) | CSS styling, design tokens, and theming |");
        sb.AppendLine();

        sb.AppendLine("## Quick Links");
        sb.AppendLine();
        sb.AppendLine("Use these resource URIs to access specific guides:");
        sb.AppendLine();
        sb.AppendLine("```");
        sb.AppendLine("fluentui://guide/installation");
        sb.AppendLine("fluentui://guide/defaultvalues");
        sb.AppendLine("fluentui://guide/whatsnew");
        sb.AppendLine("fluentui://guide/migration");
        sb.AppendLine("fluentui://guide/localization");
        sb.AppendLine("fluentui://guide/styles");
        sb.AppendLine("```");

        return sb.ToString();
    }

    /// <summary>
    /// Gets the installation guide.
    /// </summary>
    [McpServerResource(
        UriTemplate = "fluentui://guide/installation",
        Name = "guide-installation",
        Title = "Installation Guide",
        MimeType = "text/markdown")]
    [Description("Complete installation guide for Fluent UI Blazor including NuGet packages, configuration, and setup.")]
    public string GetInstallationGuide()
    {
        return _guideService.GetGuideContent("installation")
            ?? "# Installation Guide\n\nGuide not available. Please check the documentation at https://www.fluentui-blazor.net/installation";
    }

    /// <summary>
    /// Gets the default values guide.
    /// </summary>
    [McpServerResource(
        UriTemplate = "fluentui://guide/defaultvalues",
        Name = "guide-defaultvalues",
        Title = "Default Values Guide",
        MimeType = "text/markdown")]
    [Description("Guide for configuring default values for Fluent UI Blazor components globally.")]
    public string GetDefaultValuesGuide()
    {
        return _guideService.GetGuideContent("defaultvalues")
            ?? "# Default Values\n\nGuide not available. Please check the documentation at https://www.fluentui-blazor.net/default-values";
    }

    /// <summary>
    /// Gets the What's New / Release Notes.
    /// </summary>
    [McpServerResource(
        UriTemplate = "fluentui://guide/whatsnew",
        Name = "guide-whatsnew",
        Title = "What's New",
        MimeType = "text/markdown")]
    [Description("Latest release notes and changes in Fluent UI Blazor.")]
    public string GetWhatsNewGuide()
    {
        return _guideService.GetGuideContent("whatsnew")
            ?? "# What's New\n\nGuide not available. Please check the documentation at https://www.fluentui-blazor.net/WhatsNew";
    }

    /// <summary>
    /// Gets the complete migration guide for v5.
    /// </summary>
    [McpServerResource(
        UriTemplate = "fluentui://guide/migration",
        Name = "guide-migration",
        Title = "Migration to v5 Guide",
        MimeType = "text/markdown")]
    [Description("Complete migration guide from v4 to v5, including all breaking changes and component updates.")]
    public string GetMigrationGuide()
    {
        return _guideService.GetFullMigrationGuide();
    }

    /// <summary>
    /// Gets the localization guide.
    /// </summary>
    [McpServerResource(
        UriTemplate = "fluentui://guide/localization",
        Name = "guide-localization",
        Title = "Localization Guide",
        MimeType = "text/markdown")]
    [Description("Guide for translating and localizing Fluent UI Blazor component texts.")]
    public string GetLocalizationGuide()
    {
        return _guideService.GetGuideContent("localization")
            ?? "# Localization\n\nGuide not available. Please check the documentation at https://www.fluentui-blazor.net/localization";
    }

    /// <summary>
    /// Gets the styles guide.
    /// </summary>
    [McpServerResource(
        UriTemplate = "fluentui://guide/styles",
        Name = "guide-styles",
        Title = "Styles Guide",
        MimeType = "text/markdown")]
    [Description("Complete guide for CSS styling, design tokens, and theming in Fluent UI Blazor.")]
    public string GetStylesGuide()
    {
        return _guideService.GetGuideContent("styles")
            ?? "# Styles\n\nGuide not available. Please check the documentation at https://www.fluentui-blazor.net/Styles";
    }
}
