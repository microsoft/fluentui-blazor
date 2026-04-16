// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;

namespace FluentUI.Demo.DocApiGen.Abstractions;

/// <summary>
/// Provides XML documentation comments for reflected types and members.
/// </summary>
public interface IDocumentationCommentProvider
{
    /// <summary>
    /// Gets the summary text for a type.
    /// </summary>
    /// <param name="type">The reflected type.</param>
    /// <returns>The resolved summary, or an empty string.</returns>
    string GetComponentSummary(Type type);

    /// <summary>
    /// Gets the summary text for a member.
    /// </summary>
    /// <param name="member">The reflected member.</param>
    /// <returns>The resolved summary, or an empty string.</returns>
    string GetMemberSummary(MemberInfo member);
}
