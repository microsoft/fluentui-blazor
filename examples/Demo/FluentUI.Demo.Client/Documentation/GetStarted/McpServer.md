---
title: MCP Server
order: 0012
category: 10|Get Started
route: /mcp-server
---

# MCP Server

**Model Context Protocol (MCP)** is an open protocol that enables seamless integration between LLM applications and external data sources and tools.
The Fluent UI Blazor library provides an MCP server that gives AI assistants access to component documentation, enabling them to generate accurate, up-to-date Fluent UI Blazor code.

## What is MCP?

MCP provides a standardized way for AI assistants (like GitHub Copilot, Claude, and others) to access external context.
Instead of relying solely on training data, AI assistants can query real-time documentation, search for specific components, and get accurate parameter information.

Learn more: [Model Context Protocol](https://modelcontextprotocol.io/)

## Installation

### As a .NET Global Tool

```bash
dotnet tool install -g Microsoft.FluentUI.AspNetCore.Components.McpServer --prerelease
```

### From Source

```bash
cd examples/Mcp/FluentUI.Mcp.Server
dotnet build
```

## Configuration

### VS Code / GitHub Copilot

Add to your `.vscode/mcp.json` or user settings:

```json
{
  "servers": {
    "fluentui-blazor": {
      "command": "fluentui-mcp",
      "args": []
    }
  }
}
```

### Claude Desktop

Add to your `claude_desktop_config.json`:

```json
{
  "mcpServers": {
    "fluentui-blazor": {
      "command": "fluentui-mcp",
      "args": []
    }
  }
}
```

### Running from Source

If running from source, use the full path:

```json
{
  "servers": {
    "fluentui-blazor": {
      "command": "dotnet",
      "args": ["run", "--project", "path/to/FluentUI.Mcp.Server"]
    }
  }
}
```

## Available Capabilities

The MCP server exposes three types of primitives:

- **Tools**: Model-controlled functions for dynamic queries (search, get details)
- **Resources**: User-selected static content (component lists, guides)
- **Prompts**: Pre-defined templates for common tasks (create component, migrate code)

{{ McpCapabilities }}

## Usage Examples

### Ask for Component Help

> "How do I use FluentDataGrid with pagination?"

The AI will use the `GetComponentDetails` tool to fetch accurate parameter information.

### Generate Code

> "Create a form with name, email, and a submit button using Fluent UI Blazor"

The AI will use the `create_form` prompt combined with component documentation.

### Migration Assistance

> "Migrate this v4 code to v5"

The AI will use the `migrate_to_v5` prompt and the migration guide resource.

## Architecture

```
┌─────────────────────────────────────────────────────────┐
│                    AI Assistant                          │
│              (Copilot, Claude, etc.)                     │
└─────────────────────────────────────────────────────────┘
                           │
                           │ MCP Protocol (stdio)
                           │
┌─────────────────────────────────────────────────────────┐
│                  FluentUI MCP Server                     │
├─────────────────────────────────────────────────────────┤
│  Tools          │  Resources       │  Prompts           │
│  ───────────    │  ────────────    │  ────────────      │
│  ListComponents │  components      │  create_component  │
│  GetDetails     │  categories      │  create_form       │
│  SearchEnums    │  guides/*        │  migrate_to_v5     │
│  ...            │  ...             │  ...               │
├─────────────────────────────────────────────────────────┤
│              FluentUIDocumentationService                │
│                (Reflection + XML Docs)                   │
└─────────────────────────────────────────────────────────┘
                           │
                           │ Assembly Reflection
                           │
┌─────────────────────────────────────────────────────────┐
│      Microsoft.FluentUI.AspNetCore.Components            │
│                    (Component Library)                   │
└─────────────────────────────────────────────────────────┘
```

## Source Code

The MCP server source code is available in the repository:

[examples/Mcp/FluentUI.Mcp.Server](https://github.com/microsoft/fluentui-blazor/tree/dev/examples/Mcp/FluentUI.Mcp.Server)
