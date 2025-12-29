# DocApiGen

If API documentation is not included in your project,
you can generate it using this project `FluentUI.Demo.DocApiGen`.

Simply run the project `FluentUI.Demo.DocApiGen` to generate the file.

## From Visual Studio

1. Right-click on the project `FluentUI.Demo.DocApiGen`.
1. Select the menu `Debug` > `Start without debugging`.
1. Re-run the project `FluentUI.Demo` to see the changes.

## From command line

The tool uses the **compact format by default** for Summary mode, which is optimized for documentation display.

### Generate Summary mode (default, compact format)

```bash
dotnet run --xml "./Microsoft.FluentUI.AspNetCore.Components.xml" --dll "../../../src/Core/bin/Debug/net9.0/Microsoft.FluentUI.AspNetCore.Components.dll" --output "../../../examples/Demo/FluentUI.Demo.Client/wwwroot/api-comments.json" --format json
```

This will generate a JSON file with the compact structure optimized for Summary mode:
```json
{
  "__Generated__": {
    "AssemblyVersion": "5.0.0-alpha.1+07c679a2",
    "DateUtc": "2025-12-13 21:33"
  },
  "FluentButton": {
    "__summary__": "The FluentButton component...",
    "Appearance": "Gets or sets the visual appearance...",
    ...
  }
}
```

### Generate All mode (complete documentation)

```bash
dotnet run --xml "./Microsoft.FluentUI.AspNetCore.Components.xml" --dll "../../../src/Core/bin/Debug/net9.0/Microsoft.FluentUI.AspNetCore.Components.dll" --output "../../../examples/Demo/FluentUI.Demo.Client/wwwroot/api-comments-all.json" --format json --mode all
```

### Generate MCP mode (MCP Server documentation)

Generate documentation for MCP Server tools, resources, and prompts:

```bash
dotnet run --xml "../../../src/Tools/McpServer/bin/Debug/net9.0/Microsoft.FluentUI.AspNetCore.McpServer.xml" --dll "../../../src/Tools/McpServer/bin/Debug/net9.0/Microsoft.FluentUI.AspNetCore.McpServer.dll" --output "../../../examples/Demo/FluentUI.Demo.Client/wwwroot/mcp-documentation.json" --format json --mode mcp
```

This will generate a JSON file with the MCP documentation structure:
```json
{
  "metadata": {
    "assemblyVersion": "5.0.0+abc12345",
    "generatedDateUtc": "2025-06-15 12:30",
    "toolCount": 5,
    "resourceCount": 6,
    "promptCount": 0
  },
  "tools": [
    {
      "name": "ListComponents",
      "description": "Lists all available Fluent UI Blazor components...",
      "summary": "XML documentation summary...",
      "className": "ComponentListTools",
      "returnType": "string",
      "parameters": [
        {
          "name": "category",
          "type": "string?",
          "description": "Filter by category",
          "isRequired": false,
          "defaultValue": "null"
        }
      ]
    }
  ],
  "resources": [
    {
      "name": "components",
      "uriTemplate": "fluentui://components",
      "title": "All Fluent UI Blazor Components",
      "mimeType": "text/markdown",
      "description": "Complete list of all components",
      "className": "FluentUIResources",
      "methodName": "GetAllComponents"
    }
  ],
  "prompts": []
}
```

The MCP mode extracts information from:
- `[McpServerToolType]` and `[McpServerTool]` attributes for tools
- `[McpServerResourceType]` and `[McpServerResource]` attributes for resources
- `[McpServerPromptType]` and `[McpServerPrompt]` attributes for prompts
- `[Description]` attributes for parameter and method descriptions
- XML documentation comments (`<summary>`) via LoxSmoke.DocXml

## Documentation Formats

The tool supports different JSON formats depending on the generation mode:

| Mode | Format | Description | Use Case |
|------|--------|-------------|----------|
| **Summary** | Compact | Optimized for documentation display | Quick lookups and UI display |
| **All** | Structured | Complete documentation with all members | Full API reference |
| **MCP** | Structured | MCP Server tools, resources, prompts | Demo site MCP documentation |

### Summary Mode - Compact Format (default)
- Uses `__Generated__` for metadata
- Simple component names as keys
- Direct member properties
- Best for quick lookups and UI display

### All Mode - Structured Format
- Uses `metadata` and `components` sections
- CamelCase property names
- Full type names with namespaces
- Includes properties, methods, events, and enums

### MCP Mode - Structured Format
- Uses `metadata`, `tools`, `resources`, `prompts` sections
- Extracts from MCP attributes and XML documentation
- Includes parameter information with types and defaults
- Only supports JSON format

## Command Line Reference

```
Usage:
  DocApiGen --xml <xml_file> --dll <dll_file> [options]

Required Arguments:
  --xml <file>      Path to the XML documentation file
  --dll <file>      Path to the assembly DLL file

Optional Arguments:
  --output <file>   Path to the output file (default: stdout)
  --format <name>   Output format (default: json)
  --mode <name>     Generation mode (default: summary)

Formats:
  json     - Generate JSON documentation
  csharp   - Generate C# code with documentation dictionary

Modes:
  summary  - Generate documentation with only [Parameter] properties
             Supports: json, csharp
  all      - Generate complete documentation (properties, methods, events)
             Supports: json only
  mcp      - Generate MCP server documentation (tools, resources, prompts)
             Supports: json only
