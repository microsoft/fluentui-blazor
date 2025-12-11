# FluentUI.Mcp.Server

A Model Context Protocol (MCP) server that provides documentation for the Fluent UI Blazor component library.

## Overview

This MCP server enables AI assistants to access comprehensive documentation about Fluent UI Blazor components, including:

- **Component listing** - Browse all available components with descriptions
- **Component details** - Get complete documentation including parameters, events, and methods
- **Search** - Find components by name or description
- **Enum information** - Explore enum types and their values
- **Usage examples** - Get code examples for components

## Architecture

The MCP server uses **pre-generated JSON documentation** to provide fast, dependency-free access to component information:

```
┌─────────────────────────────────────────────────┐
│ FluentUI.Demo.DocApiGen (Build Time)            │
│ ├── Uses LoxSmoke.DocXml                        │
│ └── Generates FluentUIComponentsDocumentation.json
└─────────────────────────────────────────────────┘
                    │
                    ▼ (PreBuild / EmbeddedResource)
┌─────────────────────────────────────────────────┐
│ McpServer (Runtime)                             │
│ └── Reads JSON from embedded resource           │
└─────────────────────────────────────────────────┘
```

This architecture provides:
- **Faster startup** - No XML parsing at runtime
- **Consistent documentation** - Generated once at build time

## Tools vs Resources

This server implements both **Tools** and **Resources** following MCP best practices:

- **Tools** (Model-controlled): Invoked automatically by the LLM for dynamic queries like search or getting specific details
- **Resources** (User-controlled): Selected by the user/application to provide static context to the LLM

## Available Tools

### `ListComponents`
Lists all available Fluent UI Blazor components with their names and brief descriptions.

**Parameters:**
- `category` (optional): Filter by category (e.g., "Button", "Input", "Dialog")

### `GetComponentDetails`
Gets detailed documentation for a specific component including parameters, properties, events, and methods.

**Parameters:**
- `componentName`: The name of the component (e.g., "FluentButton", "FluentDataGrid")

### `SearchComponents`
Searches for components by name or description.

**Parameters:**
- `searchTerm`: The term to search for

### `ListCategories`
Lists all available component categories with component counts.

### `GetEnumValues`
Gets information about a specific enum type including all possible values.

**Parameters:**
- `enumName`: The name of the enum (e.g., "Appearance", "Color", "Size")
- `filter` (optional): Filter to show only values matching this search term

### `GetComponentEnums`
Lists all enum types used by a specific component, showing which property/parameter uses each enum.

**Parameters:**
- `componentName`: The name of the component (e.g., "FluentButton", "FluentDataGrid")

### `ListEnums`
Lists all enum types used in the library.

**Parameters:**
- `filter` (optional): Filter enums by name (e.g., "Color", "Size", "Appearance")

### `GetComponentExample`
Gets usage examples for a specific component.

**Parameters:**
- `componentName`: The name of the component

### `GetGuide`
Gets a specific documentation guide by name.

**Parameters:**
- `guideName`: The guide name: 'installation', 'defaultvalues', 'whatsnew', 'migration', 'localization', or 'styles'

### `SearchGuides`
Searches documentation guides for specific content or topics.

**Parameters:**
- `searchTerm`: The search term to find in documentation guides

### `ListGuides`
Lists all available documentation guides with descriptions.

## Available Resources

Resources provide static documentation that users can attach to conversations for context.

### Component Resources

| URI | Description |
|-----|-------------|
| `fluentui://components` | Complete list of all Fluent UI Blazor components organized by category |
| `fluentui://categories` | List of all component categories with component counts |
| `fluentui://enums` | Complete list of all enum types with their values |

### Documentation Guides

| URI | Description |
|-----|-------------|
| `fluentui://guides` | List of all available documentation guides |
| `fluentui://guide/installation` | Installation and setup guide |
| `fluentui://guide/defaultvalues` | Configure default component values globally |
| `fluentui://guide/whatsnew` | Latest release notes and changes |
| `fluentui://guide/migration` | Complete migration guide from v4 to v5 |
| `fluentui://guide/localization` | Translate and localize component texts |
| `fluentui://guide/styles` | CSS styling, design tokens, and theming |

### Resource Templates

| URI Template | Description |
|--------------|-------------|
| `fluentui://component/{name}` | Detailed documentation for a specific component |
| `fluentui://category/{name}` | List of all components in a specific category |
| `fluentui://enum/{name}` | Detailed information about a specific enum type |

## Available Prompts

Prompts are pre-defined templates that help generate common code patterns and configurations.

### Component Prompts

| Prompt | Description |
|--------|-------------|
| `create_component` | Generate code for a Fluent UI Blazor component with specified configuration |
| `explain_component` | Get a detailed explanation of a component and its usage |
| `compare_components` | Compare two or more components to understand their differences |

### Form & Data Prompts

| Prompt | Description |
|--------|-------------|
| `create_form` | Generate a complete form with validation |
| `create_datagrid` | Generate a DataGrid with specified columns and features |
| `create_dialog` | Generate a dialog/modal component |
| `create_drawer` | Generate a drawer/panel component for side content |

### Migration & Setup Prompts

| Prompt | Description |
|--------|-------------|
| `migrate_to_v5` | Get guidance for migrating code from v4 to v5 |
| `setup_project` | Get step-by-step guidance for setting up a new project |
| `configure_theming` | Get guidance for configuring theming and styles |
| `configure_localization` | Get guidance for implementing localization |

## Installation & Setup

### Option 1: Install from NuGet (Recommended)

Install the MCP server as a global .NET tool:

```bash
dotnet tool install -g Microsoft.FluentUI.AspNetCore.Components.McpServer --prerelease
```

Or use DNX to add to your MCP client directly:

```bash
dnx add Microsoft.FluentUI.AspNetCore.Components.McpServer
```

### Option 2: Build from Source

**Prerequisites:**
- .NET 9.0 SDK
- Build the FluentUI Blazor solution first

```bash
cd src/Tools/McpServer
dotnet build
```

The build process will automatically:
1. Build the FluentUI Components project
2. Run DocApiGen to generate the MCP documentation JSON
3. Embed the JSON as a resource in the McpServer assembly

### Configure in VS Code

If installed as a global tool:

```json
{
  "mcp": {
    "servers": {
      "fluentui-blazor": {
        "command": "fluentui-mcp"
      }
    }
  }
}
```

If running from source:

```json
{
  "mcp": {
    "servers": {
      "fluentui-blazor": {
        "command": "dotnet",
        "args": [
          "run",
          "--project",
          "path/to/src/Tools/McpServer/Microsoft.FluentUI.AspNetCore.Components.McpServer.csproj"
        ]
      }
    }
  }
}
```

Or using the built executable:

```json
{
  "mcp": {
    "servers": {
      "fluentui-blazor": {
        "command": "path/to/Microsoft.FluentUI.AspNetCore.Components.McpServer.exe"
      }
    }
  }
}
```

## Usage Examples

### List all button components

```
ListComponents(category: "Button")
```

### Get details about FluentDataGrid

```
GetComponentDetails(componentName: "FluentDataGrid")
```

### Search for input components

```
SearchComponents(searchTerm: "input")
```

### Get enum values for Appearance

```
GetEnumValues(enumName: "Appearance")
```

### Get migration guide

```
GetGuide(guideName: "migration")
```

### Search for content in guides

```
SearchGuides(searchTerm: "FluentButton")
```

## Development

### Project Structure

```
McpServer/
├── Program.cs                 # Entry point
├── FluentUIComponentsDocumentation.json  # Pre-generated documentation (auto-generated)
├── Extensions/
│   └── ServiceCollectionExtensions.cs  # DI configuration
├── Models/                    # Data models (7 files)
│   ├── ComponentInfo.cs
│   ├── ComponentDetails.cs
│   ├── PropertyInfo.cs
│   ├── EventInfo.cs
│   ├── MethodInfo.cs
│   ├── EnumInfo.cs
│   └── EnumValueInfo.cs
├── Services/                  # Documentation services
│   ├── JsonBasedDocumentationService.cs  # Main service (uses JSON)
│   ├── JsonDocumentationReader.cs        # JSON reader
│   ├── DocumentationGuideService.cs
│   ├── TypeHelper.cs
│   └── ComponentCategoryHelper.cs
├── Tools/                     # MCP Tools (model-controlled)
│   ├── ToolOutputHelper.cs
│   ├── ComponentListTools.cs
│   ├── ComponentDetailTools.cs
│   ├── EnumTools.cs
│   └── GuideTools.cs
├── Resources/                 # MCP Resources (user-controlled)
│   ├── FluentUIResources.cs
│   ├── ComponentResources.cs
│   └── GuideResources.cs
└── Prompts/                   # MCP Prompts (user-controlled templates)
    ├── CreateComponentPrompt.cs
    ├── ExplainComponentPrompt.cs
    └── ...
```

### Regenerating Documentation

The MCP documentation JSON is automatically regenerated during build if:
- The file doesn't exist
- You set `ForceGenerateMcpDocs=true`

To force regeneration:

```bash
dotnet build -p:ForceGenerateMcpDocs=true
```

### Adding New Tools

1. Add a new class in the `Tools/` folder with `[McpServerToolType]` attribute
2. Add methods with `[McpServerTool]` and `[Description]` attributes
3. Use `[Description]` on parameters for better AI understanding

### Adding New Resources

1. Add a new class in the `Resources/` folder with `[McpServerResourceType]` attribute
2. Add methods with `[McpServerResource]` attribute specifying UriTemplate, Name, and MimeType
3. Register in `ServiceCollectionExtensions.cs` with `.WithResources<YourResourceClass>()`

### Adding New Prompts

1. Add a new class in the `Prompts/` folder with `[McpServerPromptType]` attribute
2. Add methods with `[McpServerPrompt]` and `[Description]` attributes
3. Return `ChatMessage` with the prompt content
4. Register in `ServiceCollectionExtensions.cs` with `.WithPrompts<YourPromptClass>()`

### Testing

Run the server in debug mode:

```bash
dotnet run --project Microsoft.FluentUI.AspNetCore.Components.McpServer.csproj
```

The server communicates via stdin/stdout using JSON-RPC.

## Troubleshooting

### Documentation Not Loading

If component descriptions are missing:
1. Check if `FluentUIComponentsDocumentation.json` exists in the project directory
2. Force regeneration with `dotnet build -p:ForceGenerateMcpDocs=true`
3. Ensure the FluentUI Components project builds successfully

### Server Not Responding

Check that:
1. The solution builds successfully
2. .NET 9.0 SDK is installed
3. The path in your MCP configuration is correct

## License

This project is licensed under the MIT License - see the [LICENSE](../../../LICENSE.TXT) file for details.
