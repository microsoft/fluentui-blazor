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

## Documentation Formats

The tool supports different JSON formats depending on the generation mode:

1. **Summary Mode - Compact Format** (default): Optimized for documentation display
   - Uses `__Generated__` for metadata
   - Simple component names as keys
   - Direct member properties
   - Best for quick lookups and UI display

2. **All Mode - Structured Format**: Complete documentation with all members
   - Uses `metadata` and `components` sections
   - CamelCase property names
   - Full type names with namespaces
   - Includes properties, methods, events, and enums

See [GENERATION_MODES.md](GENERATION_MODES.md) for detailed documentation about generation modes and formats.
