using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Microsoft.FluentUI.AspNetCore.Components.Generators;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class EnumAttributeValueAnalyzer : DiagnosticAnalyzer
{
    private static readonly DiagnosticDescriptor MissingRegistration = new(
        "FLUENTGEN001",
        "Register enum attribute values for generation",
        "Register '{0}' with EnumAttributeValuesAttribute and import its generated extensions to avoid reflection and boxing",
        "Performance",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [MissingRegistration];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();
        context.RegisterOperationAction(static context =>
        {
            var invocation = (IInvocationOperation)context.Operation;
            if (invocation.TargetMethod.Name != "ToAttributeValue" ||
                invocation.TargetMethod.ContainingType.ToDisplayString() != "Microsoft.FluentUI.AspNetCore.Components.Extensions.EnumExtensions")
            {
                return;
            }

            var receiver = invocation.Arguments[0].Value;
            while (receiver is IConversionOperation conversion)
            {
                receiver = conversion.Operand;
            }

            var type = receiver.Type;
            if (type is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } nullableType)
            {
                type = nullableType.TypeArguments[0];
            }

            if (type?.TypeKind == TypeKind.Enum)
            {
                context.ReportDiagnostic(Diagnostic.Create(MissingRegistration, invocation.Syntax.GetLocation(), type.ToDisplayString()));
            }
        }, OperationKind.Invocation);
    }
}