# AGENTS.md

This file provides context and instructions for AI coding agents working on the **Microsoft Fluent UI Blazor** components library.

## Project Overview

This is a .NET Blazor component library that provides Fluent UI design system components for Blazor applications. The library wraps Microsoft's official **Fluent UI Web Components** and provides additional components leveraging the Fluent Design System.

- **Main Package**: `Microsoft.FluentUI.AspNetCore.Components`
- **Target Framework**: .NET 8+
- **License**: MIT
- **Language**: C# with Razor components
- **Documentation**: https://www.fluentui-blazor.net

## Repository Structure

```
src/
├── Core/                           # Main component library
├── Core.Scripts/                   # TypeScript/JavaScript for web components
└── Extensions/                     # Additional packages (EF adapter, OData, etc.)

tests/
├── Core/                           # Unit tests (bUnit)
└── Integration/                    # Integration tests

examples/
├── Demo/                           # Demo application
└── Samples/                        # Sample projects
```

## Setup Commands

### Prerequisites
- .NET 8 SDK or later
- Node.js 22.x (for Core.Scripts project)
- Visual Studio 2026 (recommended on Windows)

### Build Commands

```bash
# Build the entire solution
dotnet build Microsoft.FluentUI-v5.slnx

# Build only the core component library
dotnet build src/Core/Microsoft.FluentUI.AspNetCore.Components.csproj

# Build the demo application
dotnet build examples/Demo/FluentUI.Demo/FluentUI.Demo.csproj

# Clean the solution
dotnet clean Microsoft.FluentUI-v5.slnx

# Restore packages
dotnet restore Microsoft.FluentUI-v5.slnx
```

### Run the Demo

```bash
# Run demo application
dotnet run --project examples/Demo/FluentUI.Demo/FluentUI.Demo.csproj

# Watch mode (hot reload)
dotnet watch run --project examples/Demo/FluentUI.Demo/FluentUI.Demo.csproj
```

## Testing Instructions

### Running Tests

```bash
# Run all unit tests
dotnet test tests/Core/Components.Tests.csproj

# Run specific test
dotnet test tests/Core/Components.Tests.csproj --filter "FullyQualifiedName~TestClassName"
```

### Unit Test Guidelines

- Tests use **bUnit** for Blazor component testing
- Tests use **Verify** for snapshot testing
- Test file naming: `{ComponentName}Tests.cs`
- Test method naming: `{MethodName}_{Scenario}_{ExpectedBehavior}`
- Always add or update tests when modifying components
- Run all tests before submitting a PR

### Test File Location

Unit tests are in `tests/Core/Components/` mirroring the component structure in `src/Core/Components/`.

## Code Style Guidelines

### C# Conventions

- **Nullable**: Enabled (`<Nullable>enable</Nullable>`)
- **Warnings as Errors**: Enabled (`<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`)
- **Code Style**: Enforced in build (`<EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>`)
- Use C# latest language features
- Follow Microsoft C# coding conventions

### Component Development

- Components are in `src/Core/Components/`
- Each component should have its own folder
- Include: `.razor` file, `.razor.cs` code-behind (if needed), `.razor.css` styles (if needed)
- Expose design tokens for customization
- Ensure accessibility compliance

### Razor Component Pattern

```csharp
// Example component structure
public partial class FluentComponentName : FluentComponentBase
{
    [Parameter]
    public string? Property { get; set; }

    [Parameter]
    public EventCallback<T> OnEvent { get; set; }
}
```

## PR Guidelines

### Before Submitting

1. **Rebase** your branch from the target branch (do NOT use `git merge`)
2. Run all unit tests: `dotnet test tests/Core/Components.Tests.csproj`
3. Ensure the build passes: `dotnet build Microsoft.FluentUI-v5.sln`
4. Update documentation if needed

### Commit Messages

Follow [Conventional Commits](https://www.conventionalcommits.org/):

- `feat:` - New feature
- `fix:` - Bug fix
- `docs:` - Documentation changes
- `chore:` - Maintenance tasks
- `refactor:` - Code refactoring
- `test:` - Adding or updating tests

### PR Checklist

- [ ] Tests added/updated for changes
- [ ] Changes have been tested locally
- [ ] Documentation updated if needed
- [ ] Follows project coding standards

## NPM/JavaScript Setup (Core.Scripts)

If you encounter NPM authentication issues (E401):

```bash
cd src/Core.Scripts

# Install vsts-npm-auth
npm install -g vsts-npm-auth --registry https://registry.npmjs.com --always-auth false

# Get authentication token
vsts-npm-auth -config .npmrc -force

# Install packages
npm install
```

## Important Files

- `Directory.Build.props` - Central versioning and build configuration
- `Directory.Packages.props` - Central package version management
- `.github/CODEOWNERS` - Code ownership (@vnbaaij @dvoituron)
- `docs/contributing.md` - Contribution guidelines
- `docs/unit-tests.md` - Detailed unit testing documentation

## Security Considerations

- Do not commit sensitive data or credentials
- Review the `SECURITY.md` file for security policies
- Report security vulnerabilities through proper channels (not public issues)

## Contributing Workflow

This repository uses a fork-based contribution model:

1. **Fork** the repository from [microsoft/fluentui-blazor](https://github.com/microsoft/fluentui-blazor)
2. **Clone** your fork locally
3. **Create a feature branch** from the target branch (`dev-v5`)
4. **Make changes** and commit following conventional commit guidelines
5. **Push** to your fork
6. **Create a Pull Request** targeting the upstream repository `microsoft/fluentui-blazor`

### Remote Configuration

```bash
# Add upstream remote (if not already configured)
git remote add upstream https://github.com/microsoft/fluentui-blazor.git

# Fetch upstream changes
git fetch upstream

# Rebase your branch on upstream
git rebase upstream/dev-v5
```

## Additional Resources

- [Demo Site](https://www.fluentui-blazor.net)
- [Discord Community](https://discord.gg/M5cBTfp6J2)
- [GitHub Issues](https://github.com/microsoft/fluentui-blazor/issues)
- [Contributing Guide](docs/contributing.md)
- [Unit Tests Guide](docs/unit-tests.md)
