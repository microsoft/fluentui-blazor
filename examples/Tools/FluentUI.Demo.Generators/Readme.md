# Code Comments Generators

This project contains a set of code generators that can be used to generate code comments for C# code files.

## Usage

To use the code generators, you need to add the following package reference to your project file:

```xml
  <ItemGroup>
    <ProjectReference Include="FluentUI.Demo.Generators.csproj" OutputItemType="Analyzer" ReferenceOutputAssembly="false" />
  </ItemGroup>
```

The first time, you need to restart Visual Studio to make the code generators available.

## Engine

The engine is responsible for generating the code comments based on the
`Microsoft.FluentUI.AspNetCore.Components.xml` code file (in FluentUI.Demo.Client project).

The file generated is under the `Dependencies / Analyzers / FluentUI.Demo.Generators` folder in the project.
