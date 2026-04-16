// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;
using FluentUI.Demo.DocApiGen.Abstractions;
using FluentUI.Demo.DocApiGen.Extensions;

namespace FluentUI.Demo.DocApiGen.Models;

/// <summary>
/// Resolves XML documentation comments across one or more documentation inputs.
/// </summary>
public sealed class DocumentationCommentProvider : IDocumentationCommentProvider
{
    private readonly IReadOnlyDictionary<Assembly, DocumentationInput> _inputsByAssembly;
    private readonly IReadOnlyList<DocumentationInput> _inputs;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentationCommentProvider"/> class.
    /// </summary>
    /// <param name="inputs">The documentation inputs to resolve comments from.</param>
    public DocumentationCommentProvider(IReadOnlyList<DocumentationInput> inputs)
    {
        ArgumentNullException.ThrowIfNull(inputs);

        if (inputs.Count == 0)
        {
            throw new ArgumentException("At least one documentation input is required.", nameof(inputs));
        }

        _inputs = inputs;
        _inputsByAssembly = inputs.ToDictionary(input => input.Assembly);
    }

    /// <inheritdoc/>
    public string GetComponentSummary(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        return TryGetInput(type.Assembly, out var input)
            ? input.DocXmlReader.GetComponentSummary(type)
            : string.Empty;
    }

    /// <inheritdoc/>
    public string GetMemberSummary(MemberInfo member)
    {
        ArgumentNullException.ThrowIfNull(member);

        var assembly = member.Module.Assembly;
        return TryGetInput(assembly, out var input)
            ? input.DocXmlReader.GetMemberSummary(member)
            : string.Empty;
    }

    private bool TryGetInput(Assembly assembly, out DocumentationInput input)
    {
        if (_inputsByAssembly.TryGetValue(assembly, out input!))
        {
            return true;
        }

        input = _inputs[0];
        return false;
    }
}
