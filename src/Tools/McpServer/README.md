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

**For Visual Studio Code** (`.vscode/mcp.json`):

```json
{
    "servers": {
        "fluent-ui-blazor": {
            "command": "fluentui-mcp"
        }
    }
}
```

**For Visual Studio 2026** (`.mcp.json` at solution root):

```json
{
    "servers": {
        "fluent-ui-blazor": {
            "command": "fluentui-mcp"
        }
    }
}
```

### Option 2: Use dnx script tool based on NuGet.org

Use [dnx](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10/sdk#the-new-dnx-tool-execution-script) to add the MCP server directly to your MCP client:

```bash
dnx Microsoft.FluentUI.AspNetCore.McpServer
```

Once added, configure your MCP client to use the tool:

**For Visual Studio Code** (`.vscode/mcp.json`):

```json
{
    "servers": {
        "fluent-ui-blazor": {
            "command": "dnx",
            "args": [
                "Microsoft.FluentUI.AspNetCore.McpServer"
            ]
        }
    }
}
```

**For Visual Studio 2026** (`.mcp.json`):

```json
{
    "servers": {
        "fluent-ui-blazor": {
            "command": "dnx",
            "args": [
                "Microsoft.FluentUI.AspNetCore.McpServer"
            ]
        }
    }
}
```

This command will download from NuGet.org the latest version. You can specify the version number using the following command:

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

**For Visual Studio Code** (`.vscode/mcp.json`):

```json
{
    "servers": {
        "fluent-ui-blazor": {
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

**For Visual Studio 2026** (`.mcp.json` at solution root):

```json
{
    "servers": {
        "fluent-ui-blazor": {
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
| `ListDocumentationTopics` | Lists all documentation topics | - |
| `GetDocumentationTopic` | Gets detailed documentation for a documentation topic | `topicName` |
| `SearchDocumentation` | Searches documentation by keyword | `searchTerm` |
| `GetMigrationGuide` | Gets migration guide for upgrading to v5 | - |
| `GetVersionInfo` | Gets the MCP server version and the expected component library version | - |
| `CheckProjectVersion` | Checks if a project's component library version matches the MCP server | `projectVersion` |

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
| `fluentui://documentation` | Complete list of all documentation topics |
| `fluentui://documentation/{topic}` | Documentation for a specific documentation topic (e.g., installation, localization, styles) |
| `fluentui://documentation/migration` | Complete migration guide for upgrading to v5 |

## Version Compatibility

The MCP server and the `Microsoft.FluentUI.AspNetCore.Components` NuGet package are published together with the **same version number** (e.g. `5.0.0-rc.1-26049.2`). Because the documentation served by the MCP is generated from a specific version of the library, it is important that the user's project references the matching version.

Two tools are provided to automate this check:

| Tool | Purpose |
|------|---------|
| `GetVersionInfo` | Returns the MCP server version and the exact `<PackageReference>` the project should use. |
| `CheckProjectVersion` | Accepts the version string found in the user's `.csproj` and reports **COMPATIBLE** or **INCOMPATIBLE** with upgrade instructions. |

### Recommended workflow for AI agents

1. Call `GetVersionInfo` to obtain the expected component library version.
2. Read the user's `.csproj` to find the `Microsoft.FluentUI.AspNetCore.Components` PackageReference version.
3. Call `CheckProjectVersion(projectVersion)` with that version.
4. If the result is **INCOMPATIBLE**, inform the user about the risks and suggest upgrading.

### Example

```
# Step 1 – Get the MCP server version
GetVersionInfo()
# → MCP version: 5.0.0-rc.1-26049.2
# → Expected: <PackageReference Include="Microsoft.FluentUI.AspNetCore.Components" Version="5.0.0-rc.1-26049.2" />

# Step 2 – Read the user's .csproj, find version "4.9.0"

# Step 3 – Validate
CheckProjectVersion(projectVersion: "4.9.0")
# → Result: INCOMPATIBLE – upgrade recommended
```

When versions do not match, the `CheckProjectVersion` tool returns:
- A list of **risks** (parameters, events, or methods may differ; code examples may not compile).
- The exact `<PackageReference>` XML and `dotnet add package` command to upgrade.

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
```

# List all documentation topics
ListDocumentation()

# Get the installation guide
GetDocumentationTopic(topicName: "Installation")

# Search for migration-related documentation
SearchDocumentation(searchTerm: "migrate")

# Get the complete migration guide to v5
GetMigrationGuide()
```

## Debugging


### Visual Studio Code

1. Configure the MCP server in `.vscode/mcp.json`:

```json
{
    "servers": {
        "fluent-ui-blazor": {
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

### MCP Inspector

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

## Security

The Fluent UI Blazor MCP Server is designed with security as a priority:

### Security Features

- ✅ **Read-only operations** - No file system modifications
- ✅ **No network access** - Runs entirely offline
- ✅ **Pre-generated documentation** - No runtime code execution
- ✅ **Sandboxed execution** - Runs as a child process of your IDE
- ✅ **No sensitive data** - Only serves public API documentation
- ✅ **Open source** - All code is publicly auditable

### What the MCP Server CANNOT Do

- ❌ Execute arbitrary code
- ❌ Access your source code or files
- ❌ Make network requests
- ❌ Modify system state
- ❌ Access environment variables or credentials
- ❌ Launch other processes

### For SecOps Teams

For comprehensive security information including:
- Architecture and isolation details
- Threat model analysis
- Compliance considerations
- Audit procedures
- Security best practices

See the [Security & Compliance Documentation](/Mcp/Security) in the demo site.

### Reporting Security Issues

To report security vulnerabilities:
- **Microsoft Security Response Center**: secure@microsoft.com
- **GitHub Security Advisories**: https://github.com/microsoft/fluentui-blazor/security

> [!IMPORTANT]
> Do not report security issues via public GitHub issues.

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
