---
title: How to Install
order: 0005.03
category: 10|Get Started
route: /Mcp/Installation
icon: ArrowDownload
---

# How to Install the MCP Server

The Fluent UI Blazor MCP Server can be installed in several ways depending on your workflow.

## Option 1: Install as .NET Tool (Recommended)

Install the MCP server as a global .NET tool from NuGet.org:

```bash
dotnet tool install -g Microsoft.FluentUI.AspNetCore.McpServer
```

After installation, configure your MCP client:

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

### Updating the Tool

To update to the latest version:

```bash
dotnet tool update -g Microsoft.FluentUI.AspNetCore.McpServer
```

### Uninstalling the Tool

```bash
dotnet tool uninstall -g Microsoft.FluentUI.AspNetCore.McpServer
```

## Option 2: Using dnx Script Tool

Use [dnx](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-10/sdk#the-new-dnx-tool-execution-script) to run the MCP server directly from NuGet.org without installing it globally:

```bash
dnx Microsoft.FluentUI.AspNetCore.McpServer
```

Configure your MCP client:

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

### Specifying a Version

You can specify a specific version:

```bash
dnx Microsoft.FluentUI.AspNetCore.McpServer@5.0.1
```

Or in the configuration:

```json
{
    "servers": {
        "fluent-ui-blazor": {
            "command": "dnx",
            "args": [
                "Microsoft.FluentUI.AspNetCore.McpServer@5.0.1"
            ]
        }
    }
}
```

## Requirements

### System Requirements

| Requirement | Minimum Version |
|-------------|-----------------|
| .NET SDK | 9.0 |
| Visual Studio | 2026 18.1+ |
| VS Code | 1.85+ |
| GitHub Copilot | Latest |

## Verification

After installation, verify the server is working:

### IDE Test

1. Open your project in VS Code or Visual Studio
2. Open GitHub Copilot Chat
3. Ask: *"What Fluent UI components are available?"*
4. The response should include component information from the MCP Server

## Troubleshooting

### Tool Not Found

If the `fluentui-mcp` command is not found after installation:

1. Ensure the .NET tools directory is in your PATH
2. Try restarting your terminal
3. Run `dotnet tool list -g` to verify installation

### Documentation Not Loading

If component descriptions are missing:

1. Force regeneration with `dotnet build -p:ForceGenerateMcpDocs=true` (source build only)
2. Ensure the Fluent UI Components project builds successfully

### Server Not Responding

Check that:
1. .NET 9.0 SDK is installed: `dotnet --version`
2. The path in your MCP configuration is correct
3. No firewall is blocking the process

## Next Steps

- [Get Started](/Mcp/GetStarted) - Configure your IDE
- [MCP Tools](/Mcp/Tools) - Explore available tools
- [Usage Examples](/Mcp/Examples) - See real-world examples
- [Security & Compliance](/Mcp/Security) - Security information for SecOps teams
