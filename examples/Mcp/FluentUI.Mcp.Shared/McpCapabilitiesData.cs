// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Mcp.Shared;

/// <summary>
/// Static data about MCP capabilities for documentation purposes.
/// This data is generated from the MCP Server assembly at build time.
/// </summary>
public static class McpCapabilitiesData
{
    /// <summary>
    /// Gets all MCP tools available in the server.
    /// </summary>
    public static IReadOnlyList<McpToolInfo> Tools { get; } =
    [
        new("GetComponentDetails", "Gets detailed documentation for a specific component including parameters, events, and methods.", "ComponentDetailTools",
        [
            new("componentName", "string", "The component name (e.g., 'FluentButton', 'FluentDataGrid')", true)
        ]),
        new("GetComponentExample", "Gets usage examples for a specific component showing basic and common usage patterns.", "ComponentDetailTools",
        [
            new("componentName", "string", "The component name", true)
        ]),
        new("GetComponentEnums", "Lists all enum types used by a specific component, showing which property/parameter uses each enum.", "EnumTools",
        [
            new("componentName", "string", "The component name to get enums for", true)
        ]),
        new("GetEnumValues", "Gets information about a specific enum type including all possible values.", "EnumTools",
        [
            new("enumName", "string", "The enum name (e.g., 'Appearance', 'Color')", true),
            new("componentFilter", "string?", "Optional: filter to enums used by a specific component", false)
        ]),
        new("GetGuide", "Gets a specific documentation guide (installation, defaultvalues, whatsnew, migration, localization, styles).", "GuideTools",
        [
            new("guideKey", "string", "The guide key", true)
        ]),
        new("ListCategories", "Lists all available component categories to help navigate the library.", "ComponentListTools", []),
        new("ListComponents", "Lists all available Fluent UI Blazor components with their names and brief descriptions.", "ComponentListTools",
        [
            new("category", "string?", "Optional: filter by category", false)
        ]),
        new("ListEnums", "Lists all enum types used in the library.", "EnumTools",
        [
            new("componentFilter", "string?", "Optional: filter to enums used by a specific component", false)
        ]),
        new("ListGuides", "Lists all available documentation guides with descriptions.", "GuideTools", []),
        new("SearchComponents", "Searches for components by name or description.", "ComponentListTools",
        [
            new("searchTerm", "string", "The term to search for", true)
        ]),
        new("SearchGuides", "Searches documentation guides for specific content or topics.", "GuideTools",
        [
            new("searchTerm", "string", "The term to search for", true)
        ])
    ];

    /// <summary>
    /// Gets all MCP prompts available in the server.
    /// </summary>
    public static IReadOnlyList<McpPromptInfo> Prompts { get; } =
    [
        new("compare_components", "Compare two or more Fluent UI Blazor components to understand their differences.", "CompareComponentsPrompt",
        [
            new("componentNames", "string", "Comma-separated list of component names (e.g., 'FluentButton,FluentAnchor')", true)
        ]),
        new("configure_localization", "Get guidance for implementing localization in Fluent UI Blazor.", "ConfigureLocalizationPrompt",
        [
            new("languages", "string", "Target languages (comma-separated, e.g., 'French,German,Spanish')", true)
        ]),
        new("configure_theming", "Get guidance for configuring theming and styles in Fluent UI Blazor.", "ConfigureThemingPrompt",
        [
            new("themeType", "string", "Theme requirements: 'dark', 'light', 'custom', or 'dynamic'", true),
            new("customizations", "string?", "Optional: specific colors or design tokens to customize", false)
        ]),
        new("create_component", "Generate code for a Fluent UI Blazor component with specified configuration.", "CreateComponentPrompt",
        [
            new("componentName", "string", "The component name (e.g., 'FluentButton', 'FluentDataGrid')", true),
            new("requirements", "string?", "Optional: specific requirements or configuration", false)
        ]),
        new("create_datagrid", "Generate a Fluent UI Blazor DataGrid with specified columns and features.", "CreateDataGridPrompt",
        [
            new("dataDescription", "string", "Describe the data and columns", true),
            new("features", "string?", "Optional: features to include (sorting, filtering, pagination)", false),
            new("itemType", "string?", "Optional: the item type name", false)
        ]),
        new("create_dialog", "Generate a Fluent UI Blazor dialog/modal component.", "CreateDialogPrompt",
        [
            new("purpose", "string", "The purpose of the dialog", true),
            new("content", "string?", "Optional: content requirements", false)
        ]),
        new("create_drawer", "Generate a Fluent UI Blazor drawer/panel component.", "CreateDrawerPrompt",
        [
            new("purpose", "string", "The purpose of the drawer", true),
            new("position", "string?", "Optional: position ('start', 'end', 'top', 'bottom')", false),
            new("content", "string?", "Optional: content requirements", false)
        ]),
        new("create_form", "Generate a complete Fluent UI Blazor form with validation.", "CreateFormPrompt",
        [
            new("formFields", "string", "Describe the form fields needed", true),
            new("modelName", "string?", "Optional: the model class name", false),
            new("validation", "string?", "Optional: validation requirements", false)
        ]),
        new("explain_component", "Get a detailed explanation of a Fluent UI Blazor component and its usage.", "ExplainComponentPrompt",
        [
            new("componentName", "string", "The component name to explain", true)
        ]),
        new("migrate_to_v5", "Get guidance for migrating Fluent UI Blazor code from v4 to v5.", "MigrateToV5Prompt",
        [
            new("existingCode", "string", "The code to migrate (paste your existing v4 code)", true),
            new("focus", "string?", "Optional: specific components or features you're migrating", false)
        ]),
        new("setup_project", "Get step-by-step guidance for setting up a new Fluent UI Blazor project.", "SetupProjectPrompt",
        [
            new("projectType", "string", "The type of Blazor project: 'server', 'wasm', 'hybrid', or 'maui'", true),
            new("features", "string?", "Optional: specific features to include", false)
        ])
    ];

    /// <summary>
    /// Gets all MCP resources available in the server.
    /// </summary>
    public static IReadOnlyList<McpResourceInfo> Resources { get; } =
    [
        // Static resources
        new("fluentui://categories", "categories", "Component Categories", "List of all component categories with component counts.", "text/markdown", false, "FluentUIResources"),
        new("fluentui://components", "components", "All Fluent UI Blazor Components", "Complete list of all Fluent UI Blazor components organized by category.", "text/markdown", false, "FluentUIResources"),
        new("fluentui://enums", "enums", "All Enum Types", "Complete list of all enum types used in the Fluent UI Blazor library.", "text/markdown", false, "FluentUIResources"),
        new("fluentui://guides", "guides", "Documentation Guides", "List of all available documentation guides.", "text/markdown", false, "GuideResources"),

        // Template resources
        new("fluentui://category/{name}", "category", "Components by Category", "List of all components in a specific category.", "text/markdown", true, "ComponentResources"),
        new("fluentui://component/{name}", "component", "Component Documentation", "Detailed documentation for a specific component.", "text/markdown", true, "ComponentResources"),
        new("fluentui://enum/{name}", "enum", "Enum Details", "Detailed information about a specific enum type.", "text/markdown", true, "ComponentResources"),
        new("fluentui://guide/{key}", "guide", "Documentation Guide", "Content of a specific documentation guide.", "text/markdown", true, "GuideResources")
    ];

    /// <summary>
    /// Gets a summary of all MCP capabilities.
    /// </summary>
    public static McpSummary GetSummary()
    {
        return new McpSummary(Tools, Prompts, Resources);
    }
}
