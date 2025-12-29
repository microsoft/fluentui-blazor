// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests;

/// <summary>
/// Helper class for skipping tests.
/// </summary>
public static class Skip
{
    /// <summary>
    /// Skips the test if the condition is not met.
    /// </summary>
    /// <param name="condition">The condition to check.</param>
    /// <param name="reason">The reason for skipping.</param>
    public static void IfNot(bool condition, string reason)
    {
        if (!condition)
        {
            // Use xUnit's built-in skip mechanism via throwing SkipException
            throw new SkipException(reason);
        }
    }
}
