// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests;

/// <summary>
/// Exception for skipping tests in xUnit.
/// </summary>
public class SkipException : Exception
{
    public SkipException(string reason) : base(reason)
    {
    }
}
