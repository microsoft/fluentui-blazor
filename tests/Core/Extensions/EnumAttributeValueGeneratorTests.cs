#pragma warning disable IDE0073
using System.Collections.Immutable;
using System.ComponentModel;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.FluentUI.AspNetCore.Components.Generators;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Extensions;

public class EnumAttributeValueGeneratorTests
{
    [Fact]
    public void Generate_ExplicitRegistrations_EmitsOnlyRequestedConcreteOverloads()
    {
        var output = Generate("""
            using Microsoft.FluentUI.AspNetCore.Components.Generators;
            namespace Example;
            public enum Selected { First, Second }
            public enum Ignored { Value }
            [EnumAttributeValues(typeof(Selected))]
            [EnumAttributeValues(typeof(Selected))]
            [EnumAttributeValues(typeof(System.DayOfWeek))]
            internal static partial class Lookup { }
            """);

        var methods = output.GetTypeByMetadataName("Example.Lookup")!.GetMembers("ToAttributeValue").OfType<IMethodSymbol>().ToArray();

        Assert.Equal(8, methods.Length);
        Assert.All(methods, method => Assert.False(method.IsGenericMethod));
        Assert.DoesNotContain(methods, method => method.Parameters[0].Type.Name == "Ignored");
    }

    [Fact]
    public void Generate_NoRegistration_DoesNotEmitOverloads()
    {
        var output = Generate("namespace Example; public enum Ignored { Value }");

        Assert.Single(output.SyntaxTrees.Skip(1));
        Assert.DoesNotContain(output.SyntaxTrees, tree => tree.GetText(TestContext.Current.CancellationToken).ToString().Contains("ToAttributeValue"));
    }

    [Theory]
    [InlineData("[System.ComponentModel.Description] Value", "")]
    [InlineData("[System.ComponentModel.Description(null)] Value", "value")]
    [InlineData("[System.ComponentModel.Description(\"\")] Value", "")]
    [InlineData("[System.ComponentModel.Description(\"quoted\\\"\\nvalue\")] Value", "quoted\"\nvalue")]
    [InlineData("NoAttribute", "noattribute")]
    public void Generate_DescriptionAttribute_EmitsMatchingStringLiteral(string field, string expected)
    {
        var output = Generate($$"""
            using Microsoft.FluentUI.AspNetCore.Components.Generators;
            namespace Example;
            public enum Selected { {{field}} }
            [EnumAttributeValues(typeof(Selected))]
            internal static partial class Lookup { }
            """);

        var arm = output.SyntaxTrees.SelectMany(tree => tree.GetRoot(TestContext.Current.CancellationToken).DescendantNodes())
            .OfType<SwitchExpressionArmSyntax>().First();

        Assert.Equal(expected, Assert.IsType<LiteralExpressionSyntax>(arm.Expression).Token.ValueText);
    }

    [Theory]
    [InlineData("sbyte", "Minimum = sbyte.MinValue, Maximum = sbyte.MaxValue")]
    [InlineData("long", "Minimum = long.MinValue, Maximum = long.MaxValue")]
    [InlineData("ulong", "Minimum = 0, Maximum = ulong.MaxValue")]
    [InlineData("int", "First = 1, Alias = 1, Second = 2, Combined = 3")]
    public void Generate_EnumConstants_CompilesSwitch(string underlyingType, string fields)
    {
        var output = Generate($$"""
            using Microsoft.FluentUI.AspNetCore.Components.Generators;
            namespace Example;
            public enum Selected : {{underlyingType}} { {{fields}} }
            [EnumAttributeValues(typeof(Selected))]
            internal static partial class Lookup { }
            """);

        Assert.NotNull(output.GetTypeByMetadataName("Example.Lookup"));
    }

    [Theory]
    [InlineData("Color", false, 1)]
    [InlineData("Color?", false, 1)]
    [InlineData("System.Enum", false, 0)]
    [InlineData("Color", true, 0)]
    public async Task Analyze_EnumConversion_ReportsOnlyMissingRegistrationsAsync(string parameterType, bool registered, int expectedDiagnostics)
    {
        var registration = registered ? "[EnumAttributeValues(typeof(Color))] internal static partial class Lookup { }" : string.Empty;
        var output = Generate($$"""
            using Microsoft.FluentUI.AspNetCore.Components;
            using Microsoft.FluentUI.AspNetCore.Components.Extensions;
            using Microsoft.FluentUI.AspNetCore.Components.Generators;
            namespace Example;
            {{registration}}
            public class Component
            {
                public string? Render({{parameterType}} value) => value.ToAttributeValue();
            }
            """, "Component.razor.g.cs");

        var diagnostics = await output.WithAnalyzers(ImmutableArray.Create<DiagnosticAnalyzer>(new EnumAttributeValueAnalyzer())).GetAnalyzerDiagnosticsAsync(TestContext.Current.CancellationToken);

        Assert.Equal(expectedDiagnostics, diagnostics.Length);
        Assert.All(diagnostics, diagnostic => Assert.Equal("FLUENTGEN001", diagnostic.Id));
    }

    private static Compilation Generate(string source, string path = "Input.cs")
    {
        var runtimeDirectory = Path.GetDirectoryName(typeof(object).Assembly.Location)!;
        var references = new[]
        {
            typeof(object).Assembly.Location,
            typeof(DescriptionAttribute).Assembly.Location,
            typeof(Color).Assembly.Location,
            Path.Combine(runtimeDirectory, "System.Runtime.dll"),
        }.Select(path => MetadataReference.CreateFromFile(path));
        var compilation = CSharpCompilation.Create("GeneratorTests",
            [CSharpSyntaxTree.ParseText(source, path: path, cancellationToken: TestContext.Current.CancellationToken)], references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, nullableContextOptions: NullableContextOptions.Enable));
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EnumAttributeValueGenerator().AsSourceGenerator());

        driver.RunGeneratorsAndUpdateCompilation(compilation, out var output, out var diagnostics, TestContext.Current.CancellationToken);

        Assert.Empty(diagnostics);
        Assert.DoesNotContain(output.GetDiagnostics(TestContext.Current.CancellationToken), diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);
        return output;
    }
}