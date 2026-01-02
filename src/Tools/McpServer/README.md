# Fluent UI Blazor MCP Server

A Model Context Protocol (MCP) server that provides documentation for the [Fluent UI Blazor](https://github.com/microsoft/fluentui-blazor) component library.

## Overview

This MCP server enables AI assistants to access comprehensive documentation about Fluent UI Blazor components, including:

- **Component listing** - Browse all 142+ available components with descriptions
- **Component details** - Get complete documentation including parameters, events, and methods
- **Search** - Find components by name or description
- **Enum information** - Explore enum types and their values

## Installation

### Option 1: Install as dotnet tool based on NuGet.org (Recommended)

Install the MCP server as a global .NET tool:

```bash
dotnet tool install -g Microsoft.FluentUI.AspNetCore.McpServer
```

After installation, configure your MCP client to use the tool:

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

### Option 1: Use dnx script tool based on NuGet.org

Use [dnx](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10/sdk#the-new-dnx-tool-execution-script) to add the MCP server directly to your MCP client:

```bash
dnx Microsoft.FluentUI.AspNetCore.McpServer
```

Once added, configure your MCP client to use the tool:
```json
{
  "mcp": {
    "servers": {
      "fluentui-blazor": {
        "command": "dnx",
        "args": [
          "Microsoft.FluentUI.AspNetCore.McpServer"
        ]
      }
    }
  }
}
```

This command will download from NuGet.org the latest version. You can specify the version number using the following command :

```bash
dnx Microsoft.FluentUI.AspNetCore.McpServer@5.0.1
```


### Option 3: Build from Source

**Prerequisites:**
- .NET 9.0 SDK
- Build the FluentUI Blazor solution first

```bash
cd src/Tools/McpServer
dotnet build
```

Configure in your MCP client:

```json
{
  "mcp": {
    "servers": {
      "fluentui-blazor": {
        "command": "dotnet",
        "args": [
          "run",
          "--project",
          "path/to/src/Tools/McpServer/Microsoft.FluentUI.AspNetCore.McpServer.csproj"
        ]
      }
    }
  }
}
```

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

## Available Features

### Tools (Model-controlled)

Tools are invoked automatically by the LLM for dynamic queries.

| Tool | Description | Parameters |
|------|-------------|------------|
| `ListComponents` | Lists all available components | `category` (optional) |
| `GetComponentDetails` | Gets detailed documentation for a component | `componentName` |
| `SearchComponents` | Searches components by name or description | `searchTerm` |
| `GetEnumValues` | Gets enum type values | `enumName`, `filter` (optional) |
| `GetComponentEnums` | Lists enums used by a component | `componentName` |

### Resources (User-controlled)

Resources provide static documentation that users can attach to conversations.

| URI | Description |
|-----|-------------|
| `fluentui://components` | Complete list of all components organized by category |
| `fluentui://categories` | List of all component categories with counts |
| `fluentui://enums` | Complete list of all enum types with values |
| `fluentui://component/{name}` | Detailed documentation for a specific component |
| `fluentui://category/{name}` | List of components in a specific category |
| `fluentui://enum/{name}` | Detailed information about a specific enum type |

## Usage Examples

```
# List all button components
ListComponents(category: "Button")

# Get details about FluentDataGrid
GetComponentDetails(componentName: "FluentDataGrid")

# Search for input components
SearchComponents(searchTerm: "input")

# Get enum values for Appearance
GetEnumValues(enumName: "Appearance")


## Debugging

### Visual Studio

1. Configure the MCP server in `.vscode/mcp.json`:

```json
{
  "servers": {
    "fluentui-blazor": {
      "type": "stdio",
      "command": "dotnet",
      "args": [
        "run",
        "--project",
        "src/Tools/McpServer/Microsoft.FluentUI.AspNetCore.McpServer.csproj"
      ],
      "env": {
        "DOTNET_ENVIRONMENT": "Development"
      }
    }
  }
}
```

2. Set breakpoints in your code
3. Use **Debug > Attach to Process** and select the `dotnet` process running the MCP server

### VS Code

1. Launch the MCP Inspector with the server:

```bash
npx @modelcontextprotocol/inspector dotnet run --project src/Tools/McpServer/Microsoft.FluentUI.AspNetCore.McpServer.csproj
```

2. Open VS Code and use the **"Attach MCP"** task from the Run and Debug panel to attach the debugger to the running process

The MCP Inspector provides a web-based interface to:
- Test MCP tools interactively
- View requests/responses in real-time
- Inspect exposed resources
- Debug protocol interactions

## Development

### Regenerating Documentation

The MCP documentation JSON is automatically regenerated during build. To force regeneration:

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

## Troubleshooting

### Documentation Not Loading

If component descriptions are missing:
1. Force regeneration with `dotnet build -p:ForceGenerateMcpDocs=true`
2. Ensure the FluentUI Components project builds successfully

### Server Not Responding

Check that:
1. The solution builds successfully
2. .NET 9.0 SDK is installed
3. The path in your MCP configuration is correct

## License

This project is licensed under the MIT License - see the [LICENSE](../../../LICENSE.TXT) file for details.
