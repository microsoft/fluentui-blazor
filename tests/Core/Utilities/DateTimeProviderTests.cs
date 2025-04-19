// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Utilities;

public class DateTimeProviderTests
{
    public DateTimeProviderTests(ITestOutputHelper testOutputHelper)
    {
        DateTimeProvider.RequiredActiveContext = true;
    }

    [Fact]
    public void DateTimeProvider_WithContext()
    {
        using var context = new DateTimeProviderContext(new DateTime(2020, 5, 26));

        var year = MyUserClass.GetCurrentYear();

        Assert.Equal(2020, year);
    }

    [Fact]
    public void DateTimeProvider_WithoutContext()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            var year = MyUserClass.GetCurrentYear();
        });
    }

    [Fact]
    public void DateTimeProvider_SystemDate_WithRequiredContext()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            var date = DateTimeProvider.GetSystemDate(requiredContext: true);
        });
    }

    [Fact]
    public void DateTimeProvider_SystemDate_WithoutRequiredContext()
    {
        var date = DateTimeProvider.GetSystemDate(requiredContext: false);

        Assert.Equal(DateTime.Today.Year, date.Date.Year);
    }

    [Fact]
    public void DateTimeProvider_DisposeEmptyContext()
    {
        using var context = new DateTimeProviderContext(new DateTime(2020, 5, 26));

        context.Dispose();
        context.Dispose();
    }

    [Fact]
    public void DateTimeProvider_UtcNow()
    {
        var date = new DateTime(2020, 5, 26);
        var currentOffset = Math.Abs(new DateTimeOffset(date).Offset.TotalHours);

        using var context = new DateTimeProviderContext(date);
        var contextOffset = Math.Abs((DateTimeProvider.UtcNow - DateTimeProvider.Now).TotalHours);

        Assert.Equal(currentOffset, contextOffset);
    }

    [Fact]
    public void DateTimeProvider_ResetCurrentIndex()
    {
        const uint maxValue = uint.MaxValue;

        var currentIndex = 0u;
        using var context = new DateTimeProviderContext(i =>
        {
            currentIndex = i;
            return new DateTime(2020, 5, 26);
        });

        context.ForceNextValue(maxValue);

        // First call => Max value
        _ = DateTimeProvider.Today;
        Assert.Equal(maxValue, currentIndex);

        // Second call => Reset
        _ = DateTimeProvider.Today;
        Assert.Equal(0u, currentIndex);
    }

    [Fact]
    public void DateTimeProvider_SimpleTest()
    {
        const int year = 2020;

        // Context 1
        using var context1 = new DateTimeProviderContext(new DateTime(year, 5, 26));
        Assert.Equal(year, DateTimeProvider.Today.Year);

        using (var context2 = new DateTimeProviderContext(new DateTime(year + 1, 5, 26)))
        {
            // Context 2
            Assert.Equal(year + 1, DateTimeProvider.Today.Year);
        }

        // Context 1
        Assert.Equal(year, DateTimeProvider.Today.Year);
    }

    [Fact]
    public void DateTimeProvider_Sequence()
    {
        const int year = 2020;

        // Context Sequence
        using var contextSequence = new DateTimeProviderContext(i => i switch
        {
            0 => new DateTime(year + 10, 5, 26),
            1 => new DateTime(year + 11, 5, 27),
            _ => DateTime.MinValue,
        });

        Assert.Equal(year + 10, DateTimeProvider.Today.Year);    // Sequence 0
        Assert.Equal(year + 11, DateTimeProvider.Today.Year);    // Sequence 1
    }

    [Fact]
    public void DateTimeProvider_UsingListOfDates()
    {
        const int year = 2020;

        // Context Sequence
        using var contextSequence = new DateTimeProviderContext(
        [
            new DateTime(year + 10, 5, 26),
            new DateTime(year + 11, 5, 27)
        ]);

        Assert.Equal(year + 10, DateTimeProvider.Today.Year);    // Sequence 0
        Assert.Equal(year + 11, DateTimeProvider.Today.Year);    // Sequence 1

        Assert.Throws<InvalidOperationException>(() => DateTimeProvider.Today); // No more dates are available
    }
    [Fact]
    public void DateTimeProviderContext_RecordTypeTest()
    {
        // Arrange
        var date = new DateTime(2020, 5, 26);

        // Act
        using var context = new DateTimeProviderContext(date);

        // Assert
        Assert.NotNull(context);
        Assert.IsType<DateTimeProviderContext>(context);
    }

    private class MyUserClass
    {
        public static int GetCurrentYear()
        {
            return DateTimeProvider.Today.Year;
        }
    }
}
