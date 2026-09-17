// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests;

/// <summary>
/// Helper attribute for skippable tests.
/// </summary>
public class SkippableFactAttribute : FactAttribute
{
    public SkippableFactAttribute([CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        : base(sourceFilePath, sourceLineNumber)
    {
    }
}
