 ---
title: Get Started
order: 0005.02
category: 10|Get Started
route: /Mcp/GetStarted
icon: Rocket
---

# Get Started with MCP Server

This guide will help you configure the Fluent UI Blazor MCP Server in your development environment.

## Prerequisites

Before you begin, ensure you have:

- **.NET 9.0 SDK** or later installed
- **Visual Studio Code** with GitHub Copilot extension, or **Visual Studio 2026** with GitHub Copilot
- A valid **GitHub Copilot subscription**

## Quick Setup

### Step 1: Install the MCP Server



The easiest way to install is via the .NET tool:

```bash
dotnet tool install -g Microsoft.FluentUI.AspNetCore.McpServer
```

> [!NOTE]
During the development phase, use this NuGet Preview feed by adding the argument
`--add-source https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet9/nuget/v3/index.json`

See [How to Install](/Mcp/Installation) for alternative installation methods.

### Step 2: Create the Configuration File

Create the appropriate configuration file for your IDE.

## Configuration for Visual Studio Code

Create a `.vscode/mcp.json` file in your workspace root:

```json
{
    "servers": {
        "fluent-ui-blazor": {
            "command": "fluentui-mcp"
        }
    }
}
```

### Enable Agent Mode

1. Open the **GitHub Copilot Chat** panel (`Ctrl+Shift+I`)
2. Switch to **Agent Mode** by clicking on the mode selector
3. The MCP Server tools will now be available to Copilot

### Verify the Connection

Ask Copilot a question about Fluent UI Blazor:

```
List all available Fluent UI Blazor components
```

If configured correctly, Copilot will use the MCP tools to provide accurate component information.

## Configuration for Visual Studio 2026

Create a `.mcp.json` file in your solution root directory:

```json
{
    "servers": {
        "fluent-ui-blazor": {
            "command": "fluentui-mcp"
        }
    }
}
```

### Enable MCP in Visual Studio

1. Open **Tools** > **Options**
2. Navigate to **GitHub** > **Copilot**
3. Ensure **Enable MCP Servers** is checked
4. Restart Visual Studio

### Verify the Connection

Open the Copilot Chat window and ask:

```
List all Fluent UI Blazor button components
```

## Alternative: Using dnx

If you prefer not to install the tool globally, use [dnx](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-10/sdk#the-new-dnx-tool-execution-script):

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

## Alternative: Build from Source

For development purposes, you can run from source:

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
                "path/to/src/Tools/McpServer/Microsoft.FluentUI.AspNetCore.McpServer.csproj"
            ],
            "env": {
                "DOTNET_ENVIRONMENT": "Development"
            }
        }
    }
}
```

## Troubleshooting

### Server Not Starting

1. Verify the tool is installed: `dotnet tool list -g`
2. Ensure .NET 9.0 SDK is installed: `dotnet --version`
3. Check that the command `fluentui-mcp` is available in your PATH

### Tools Not Available

1. Restart your IDE
2. Check the Output panel for MCP-related errors
3. Verify GitHub Copilot is active and authenticated

## Next Steps

- Learn about [available MCP Tools](/Mcp/Tools)
- Explore [MCP Resources](/Mcp/Resources)
- See [Usage Examples](/Mcp/Examples)

## References

- [VS Code MCP Configuration](https://code.visualstudio.com/docs/copilot/chat/mcp-servers)
- [Visual Studio MCP Support](https://learn.microsoft.com/visualstudio/ide/mcp-servers)
- [MCP Inspector](https://modelcontextprotocol.io/docs/tools/inspector)
