# csharp-naming-conventions

C# and .NET naming conventions and best practices for AI coding agents, following the [Agent Skills](https://agentskills.io/) specification.

## Overview

This skill contains 22 rules across 6 categories, ordered by impact:

| Category | Impact | Description |
|----------|--------|-------------|
| Naming & Casing | CRITICAL | Pascal/Camel case, file organization, identifier naming |
| Code Layout & Formatting | HIGH | Indentation, braces, line length, member ordering |
| Code Patterns | HIGH | Variables, comments, namespaces, if patterns, strings, exceptions, operators, conversions, DateTime, static |
| Unit Testing | HIGH | Best practices, code coverage, Blazor testing with bUnit |
| Blazor Conventions | MEDIUM-HIGH | Components, structure, JS interop, CSS isolation, performance |

## Installation

### Manual Installation

Copy the skill into your `.copilot/skills/` directory:

```
cp -r skills/csharp-naming-conventions ~/.copilot/skills/
```

### Claude Code

```
cp -r skills/csharp-naming-conventions ~/.claude/skills/
```

## File Structure

```
skills/csharp-naming-conventions/
├── SKILL.md              # Skill definition (triggers agent activation)
├── metadata.json         # Version and metadata
├── README.md             # This file
└── rules/
    ├── _sections.md      # Section definitions
    ├── naming-*.md       # Naming and casing rules
    ├── layout-*.md       # Code layout rules
    ├── code-*.md         # Code pattern rules
    ├── testing-*.md      # Unit testing rules
    └── blazor-*.md       # Blazor convention rules
```

## How It Works

When you're working on C# / .NET / Blazor code, AI coding agents that support Agent Skills will automatically:

1. Detect the skill based on `SKILL.md` triggers
2. Load `SKILL.md` as the lightweight index
3. Follow linked rule files in `rules/` as needed
4. Apply best practices while generating or reviewing code

## Compatibility

This skill follows the [Agent Skills](https://agentskills.io) open standard and is compatible with:

- Claude Code
- VS Code (GitHub Copilot)
- Gemini CLI
- OpenCode
- Other Agent Skills-compatible tools

## References

- [CSharp Coding Guidelines](https://csharpcodingguidelines.com/)
- [Microsoft Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- [Unit Testing Best Practices](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)
- [Blazor Components](https://docs.microsoft.com/en-us/aspnet/core/blazor/components)

## License

MIT
