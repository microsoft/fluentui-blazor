// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Prompts;
using Microsoft.FluentUI.AspNetCore.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Prompts;

/// <summary>
/// Tests for the <see cref="IconPrompts"/> class.
/// </summary>
public class IconPromptsTests
{
    private static IconPrompts CreatePrompts()
    {
        var synonymService = new IconSynonymService();
        var iconService = new IconService(synonymService);
        return new IconPrompts(iconService);
    }

    #region FindIcon Prompt

    [Fact]
    public void FindIcon_WithDescription_ContainsDescription()
    {
        var prompts = CreatePrompts();
        var message = prompts.FindIcon("delete a record");

        Assert.Contains("delete a record", message.Text, StringComparison.Ordinal);
        Assert.Contains("# Find the Best Fluent UI Icon", message.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void FindIcon_WithContext_ContainsContext()
    {
        var prompts = CreatePrompts();
        var message = prompts.FindIcon("settings", "toolbar button");

        Assert.Contains("toolbar button", message.Text, StringComparison.Ordinal);
        Assert.Contains("Where it will be used", message.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void FindIcon_WithoutContext_OmitsContext()
    {
        var prompts = CreatePrompts();
        var message = prompts.FindIcon("settings");

        Assert.DoesNotContain("Where it will be used", message.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void FindIcon_ContainsInstructions()
    {
        var prompts = CreatePrompts();
        var message = prompts.FindIcon("settings");

        Assert.Contains("## Instructions", message.Text, StringComparison.Ordinal);
        Assert.Contains("SearchIcons", message.Text, StringComparison.Ordinal);
        Assert.Contains("GetIconDetails", message.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void FindIcon_ContainsSizeGuidance()
    {
        var prompts = CreatePrompts();
        var message = prompts.FindIcon("settings");

        Assert.Contains("## Size Guidance", message.Text, StringComparison.Ordinal);
        Assert.Contains("Size", message.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void FindIcon_ContainsOutputFormat()
    {
        var prompts = CreatePrompts();
        var message = prompts.FindIcon("settings");

        Assert.Contains("## Output Format", message.Text, StringComparison.Ordinal);
        Assert.Contains("FluentIcon", message.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void FindIcon_ButtonContext_ContainsButtonGuidance()
    {
        var prompts = CreatePrompts();
        var message = prompts.FindIcon("settings", "button");

        Assert.Contains("buttons and toolbars", message.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void FindIcon_NavigationContext_ContainsNavGuidance()
    {
        var prompts = CreatePrompts();
        var message = prompts.FindIcon("home", "navigation menu");

        Assert.Contains("navigation", message.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void FindIcon_HeaderContext_ContainsHeaderGuidance()
    {
        var prompts = CreatePrompts();
        var message = prompts.FindIcon("star", "hero section");

        Assert.Contains("headers", message.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void FindIcon_ReturnsUserRole()
    {
        var prompts = CreatePrompts();
        var message = prompts.FindIcon("settings");

        Assert.Equal(Microsoft.Extensions.AI.ChatRole.User, message.Role);
    }

    #endregion

    #region UseIcon Prompt

    [Fact]
    public void UseIcon_ExistingIcon_ContainsVariants()
    {
        var prompts = CreatePrompts();
        var message = prompts.UseIcon("Bookmark");

        Assert.Contains("# Use Icon: Bookmark", message.Text, StringComparison.Ordinal);
        Assert.Contains("Available variants", message.Text, StringComparison.Ordinal);
        Assert.Contains("Recommended default", message.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void UseIcon_NonExistentIcon_ContainsWarning()
    {
        var prompts = CreatePrompts();
        var message = prompts.UseIcon("NonExistentXyz");

        Assert.Contains("was not found", message.Text, StringComparison.Ordinal);
        Assert.Contains("SearchIcons", message.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void UseIcon_WithUseCase_ContainsUseCase()
    {
        var prompts = CreatePrompts();
        var message = prompts.UseIcon("Bookmark", "button");

        Assert.Contains("**Use case:** button", message.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void UseIcon_ContainsInstructions()
    {
        var prompts = CreatePrompts();
        var message = prompts.UseIcon("Bookmark");

        Assert.Contains("## Instructions", message.Text, StringComparison.Ordinal);
        Assert.Contains("GetIconUsage", message.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void UseIcon_ContainsAccessibilitySection()
    {
        var prompts = CreatePrompts();
        var message = prompts.UseIcon("Bookmark");

        Assert.Contains("## Accessibility Requirements", message.Text, StringComparison.Ordinal);
        Assert.Contains("Decorative icons", message.Text, StringComparison.Ordinal);
        Assert.Contains("Focusable", message.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void UseIcon_ExistingIcon_ContainsCodeExample()
    {
        var prompts = CreatePrompts();
        var message = prompts.UseIcon("Bookmark");

        Assert.Contains("```razor", message.Text, StringComparison.Ordinal);
        Assert.Contains("Icons.", message.Text, StringComparison.Ordinal);
        Assert.Contains("Bookmark()", message.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void UseIcon_ButtonUseCase_ContainsButtonGuidance()
    {
        var prompts = CreatePrompts();
        var message = prompts.UseIcon("Bookmark", "button");

        Assert.Contains("Button Usage", message.Text, StringComparison.Ordinal);
        Assert.Contains("Slot", message.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void UseIcon_MenuUseCase_ContainsMenuGuidance()
    {
        var prompts = CreatePrompts();
        var message = prompts.UseIcon("Bookmark", "menu");

        Assert.Contains("Menu / Navigation Usage", message.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void UseIcon_ToolbarUseCase_ContainsToolbarGuidance()
    {
        var prompts = CreatePrompts();
        var message = prompts.UseIcon("Bookmark", "toolbar");

        Assert.Contains("Toolbar Usage", message.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void UseIcon_StatusUseCase_ContainsStatusGuidance()
    {
        var prompts = CreatePrompts();
        var message = prompts.UseIcon("Bookmark", "status indicator");

        Assert.Contains("Status Indicator Usage", message.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void UseIcon_HeaderUseCase_ContainsHeaderGuidance()
    {
        var prompts = CreatePrompts();
        var message = prompts.UseIcon("Bookmark", "header");

        Assert.Contains("Header / Hero Usage", message.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void UseIcon_FormUseCase_ContainsFormGuidance()
    {
        var prompts = CreatePrompts();
        var message = prompts.UseIcon("Bookmark", "form input");

        Assert.Contains("Form / Input Usage", message.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void UseIcon_TableUseCase_ContainsTableGuidance()
    {
        var prompts = CreatePrompts();
        var message = prompts.UseIcon("Bookmark", "data grid");

        Assert.Contains("Table / DataGrid Usage", message.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void UseIcon_NoUseCase_ContainsDefaultGuidance()
    {
        var prompts = CreatePrompts();
        var message = prompts.UseIcon("Bookmark");

        Assert.Contains("variant", message.Text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("accessibility", message.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void UseIcon_ReturnsUserRole()
    {
        var prompts = CreatePrompts();
        var message = prompts.UseIcon("Bookmark");

        Assert.Equal(Microsoft.Extensions.AI.ChatRole.User, message.Role);
    }

    #endregion
}
