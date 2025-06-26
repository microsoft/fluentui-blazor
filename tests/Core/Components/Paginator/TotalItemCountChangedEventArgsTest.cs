// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Paginator;

public class TotalItemCountChangedEventArgsTests
{
    [Fact]
    public void Constructor_SetsTotalItemCount()
    {
        // Arrange
        int? expectedCount = 42;

        // Act
        var args = new TotalItemCountChangedEventArgs(expectedCount);

        // Assert
        Assert.Equal(expectedCount, args.TotalItemCount);
    }

    [Fact]
    public void Constructor_AllowsNullTotalItemCount()
    {
        // Act
        var args = new TotalItemCountChangedEventArgs(null);

        // Assert
        Assert.Null(args.TotalItemCount);
    }
}
