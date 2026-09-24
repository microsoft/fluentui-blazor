#pragma warning disable IDE0073
using System.Collections.Immutable;
using System.ComponentModel;
using System.Reflection;
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
    [InlineData("Selected")]
    [InlineData("WizardStepStatus")]
    public void Generate_FlagsRegistration_EmitsConcreteOverloads(string enumType)
    {
        var output = Generate($$"""
            using Microsoft.FluentUI.AspNetCore.Components;
            using Microsoft.FluentUI.AspNetCore.Components.Generators;
            namespace Example;
            [System.Flags] public enum Selected { First = 1, Alias = 1, Second = 2 }
            public enum Regular { First }
            [EnumAttributeValues(typeof(Regular))]
            [EnumAttributeValues(typeof({{enumType}}))]
            internal static partial class Lookup { }
            """);

        var methods = output.GetTypeByMetadataName("Example.Lookup")!.GetMembers("ToAttributeValue").OfType<IMethodSymbol>().ToArray();

        Assert.Equal(8, methods.Length);
        Assert.All(methods, method => Assert.False(method.IsGenericMethod));
        Assert.Contains(methods, method => method.Parameters[0].Type.Name == enumType);
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData(null, true)]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(2, false)]
    [InlineData(3, false)]
    [InlineData(3, true)]
    [InlineData(3, null)]
    [InlineData(4, false)]
    [InlineData(5, false)]
    [InlineData(6, false)]
    [InlineData(7, false)]
    [InlineData(7, true)]
    [InlineData(8, false)]
    [InlineData(8, true)]
    [InlineData(-1, false)]
    public void ToAttributeValue_GeneratedFlagsOverloads_MatchReflection(int? rawValue, bool? returnEmptyAsNull)
    {
        var output = Generate("""
            using System.ComponentModel;
            using Microsoft.FluentUI.AspNetCore.Components.Extensions;
            using Microsoft.FluentUI.AspNetCore.Components.Generators;
            namespace Example;
            [System.Flags]
            public enum Selected
            {
                [Description("none-token")] None = 0,
                [Description("none-token")] ZeroAlias = 0,
                [Description("first-token")] First = 1,
                [Description("first-token")] FirstAlias = 1,
                [Description("second-token")] Second = 2,
                [Description("second-token")] SecondAlias = 2,
                Third = 4,
                [Description("all-token")] All = 7,
                [Description("all-token")] AllAlias = 7
            }
            [EnumAttributeValues(typeof(Selected))]
            internal static partial class Lookup { }
            public static class Probe
            {
                public static string?[] Convert(int? rawValue, bool? returnEmptyAsNull)
                {
                    Selected? value = rawValue.HasValue ? (Selected)rawValue.Value : null;
                    return
                    [
                        Lookup.ToAttributeValue(value.GetValueOrDefault(), returnEmptyAsNull),
                        EnumExtensions.ToAttributeValue(value.GetValueOrDefault(), returnEmptyAsNull),
                        Lookup.ToAttributeValue(value, returnEmptyAsNull),
                        EnumExtensions.ToAttributeValue(value, returnEmptyAsNull),
                        Lookup.ToAttributeValue(value.GetValueOrDefault(), Selected.All, returnEmptyAsNull),
                        EnumExtensions.ToAttributeValue(value.GetValueOrDefault(), Selected.All, returnEmptyAsNull),
                        Lookup.ToAttributeValue(value, Selected.All, returnEmptyAsNull),
                        EnumExtensions.ToAttributeValue(value, Selected.All, returnEmptyAsNull)
                    ];
                }
            }
            """);
        using var stream = new MemoryStream();
        var result = output.Emit(stream, cancellationToken: TestContext.Current.CancellationToken);
        Assert.True(result.Success, string.Join(Environment.NewLine, result.Diagnostics));
        var assembly = Assembly.Load(stream.ToArray());
        var convert = assembly.GetType("Example.Probe")!.GetMethod("Convert")!.CreateDelegate<Func<int?, bool?, string?[]>>();

        var actual = convert(rawValue, returnEmptyAsNull);

        Assert.Equal(actual[1], actual[0]);
        Assert.Equal(actual[3], actual[2]);
        Assert.Equal(actual[5], actual[4]);
        Assert.Equal(actual[7], actual[6]);
    }

    [Theory]
    [InlineData("")]
    [InlineData("[System.Flags]")]
    public void Generate_ConflictingAliases_UsesFirstDeclaredDescriptionWithoutRuntimeNameLookup(string flagsAttribute)
    {
        var output = Generate($$"""
            using System.ComponentModel;
            using Microsoft.FluentUI.AspNetCore.Components.Generators;
            namespace Example;
            {{flagsAttribute}}
            public enum Selected
            {
                [Description("first-token")] ZFirst = 1,
                [Description("alias-token")] AAlias = 1
            }
            [EnumAttributeValues(typeof(Selected))]
            internal static partial class Lookup { }
            """);

        var generatedNodes = output.SyntaxTrees.Skip(1)
            .SelectMany(tree => tree.GetRoot(TestContext.Current.CancellationToken).DescendantNodes()).ToArray();
        var expression = Assert.Single(generatedNodes.OfType<SwitchExpressionSyntax>());

        Assert.Equal(2, expression.Arms.Count);
        Assert.Equal("first-token", Assert.IsType<LiteralExpressionSyntax>(expression.Arms[0].Expression).Token.ValueText);
        Assert.DoesNotContain(generatedNodes.OfType<InvocationExpressionSyntax>(), invocation =>
            invocation.Expression is MemberAccessExpressionSyntax { Name.Identifier.ValueText: "ToString" or "GetName" });
    }

    [Theory]
    [InlineData("[EnumAttributeValues]", "CS7036")]
    [InlineData("[EnumAttributeValues(typeof(Missing))]", "CS0246")]
    public void Generate_MalformedRegistration_PreservesValidRegistrations(string registration, string expectedError)
    {
        var output = Generate($$"""
            using Microsoft.FluentUI.AspNetCore.Components.Generators;
            namespace Example;
            public enum Selected { First }
            [EnumAttributeValues(typeof(Selected))]
            {{registration}}
            internal static partial class Lookup { }
            """, out var diagnostics);

        Assert.Empty(diagnostics);
        var method = Assert.Single(output.GetTypeByMetadataName("Example.Lookup")!.GetMembers("GetDescription").OfType<IMethodSymbol>());
        Assert.Equal("Selected", method.Parameters[0].Type.Name);
        var error = Assert.Single(output.GetDiagnostics(TestContext.Current.CancellationToken).Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error));
        Assert.Equal(expectedError, error.Id);
    }

    [Theory]
    [InlineData("[System.ComponentModel.Description] Value", "")]
    [InlineData("[System.ComponentModel.Description(null)] Value", "value")]
    [InlineData("[System.ComponentModel.Description(\"\")] Value", "")]
    [InlineData("[System.ComponentModel.Description(\"quoted\\\"\\nvalue\")] Value", "quoted\"\nvalue")]
    [InlineData("NoAttribute", "noattribute")]
    public void ToAttributeValue_DescriptionAttribute_MatchesReflection(string field, string expected)
    {
        var output = Generate($$"""
            using Microsoft.FluentUI.AspNetCore.Components.Extensions;
            using Microsoft.FluentUI.AspNetCore.Components.Generators;
            namespace Example;
            public enum Selected { {{field}} }
            [EnumAttributeValues(typeof(Selected))]
            internal static partial class Lookup { }
            public static class Probe
            {
                public static string?[] Convert() =>
                [
                    Lookup.ToAttributeValue((Selected)0),
                    EnumExtensions.ToAttributeValue((Selected)0),
                    Lookup.ToAttributeValue((Selected)0, returnEmptyAsNull: true),
                    EnumExtensions.ToAttributeValue((Selected)0, returnEmptyAsNull: true)
                ];
            }
            """);

        var arm = output.SyntaxTrees.SelectMany(tree => tree.GetRoot(TestContext.Current.CancellationToken).DescendantNodes())
            .OfType<SwitchExpressionArmSyntax>().First();
        using var stream = new MemoryStream();
        var result = output.Emit(stream, cancellationToken: TestContext.Current.CancellationToken);
        Assert.True(result.Success, string.Join(Environment.NewLine, result.Diagnostics));
        var assembly = Assembly.Load(stream.ToArray());
        var convert = assembly.GetType("Example.Probe")!.GetMethod("Convert")!.CreateDelegate<Func<string?[]>>();

        var actual = convert();

        Assert.Equal(expected, Assert.IsType<LiteralExpressionSyntax>(arm.Expression).Token.ValueText);
        Assert.Equal(expected, actual[0]);
        Assert.Equal(actual[1], actual[0]);
        Assert.Equal(expected.Length == 0 ? null : expected, actual[2]);
        Assert.Equal(actual[3], actual[2]);
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
    [InlineData("WizardStepStatus", false, 1)]
    [InlineData("WizardStepStatus?", false, 1)]
    [InlineData("Color", false, 1, "EnumExtensions.ToAttributeValue(returnEmptyAsNull: true, value: value)")]
    [InlineData("Color?", false, 1, "EnumExtensions.ToAttributeValue(returnEmptyAsNull: true, value: value)")]
    [InlineData("System.Enum", false, 0, "EnumExtensions.ToAttributeValue(returnEmptyAsNull: true, value: value)")]
    [InlineData("System.Enum", false, 0, "EnumExtensions.ToAttributeValue(isNull: Color.Default, value: value)")]
    public async Task Analyze_EnumConversion_ReportsOnlyMissingRegistrationsAsync(string parameterType, bool registered, int expectedDiagnostics, string expression = "value.ToAttributeValue()")
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
                public string? Render({{parameterType}} value) => {{expression}};
            }
            """, "Component.razor.g.cs");

        var diagnostics = await output.WithAnalyzers(ImmutableArray.Create<DiagnosticAnalyzer>(new EnumAttributeValueAnalyzer())).GetAnalyzerDiagnosticsAsync(TestContext.Current.CancellationToken);

        Assert.Equal(expectedDiagnostics, diagnostics.Length);
        Assert.All(diagnostics, diagnostic =>
        {
            Assert.Equal("FUIBGEN001", diagnostic.Id);
            Assert.EndsWith("with the [EnumAttributeValues] attribute (for example, in GeneratedEnumExtensions.cs) and import its generated extensions to avoid reflection and boxing",
                diagnostic.GetMessage(System.Globalization.CultureInfo.InvariantCulture));
        });
    }

    private static Compilation Generate(string source, string path = "Input.cs")
    {
        var output = Generate(source, out var diagnostics, path);

        Assert.Empty(diagnostics);
        Assert.DoesNotContain(output.GetDiagnostics(TestContext.Current.CancellationToken), diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);
        return output;
    }

    private static Compilation Generate(string source, out ImmutableArray<Diagnostic> diagnostics, string path = "Input.cs")
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

        driver.RunGeneratorsAndUpdateCompilation(compilation, out var output, out diagnostics, TestContext.Current.CancellationToken);

        return output;
    }
}