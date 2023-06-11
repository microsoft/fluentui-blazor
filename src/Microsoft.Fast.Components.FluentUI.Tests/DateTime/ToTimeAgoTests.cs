using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.DateTime;

public class ToTimeAgoTests : TestBase
{
    [Theory]
    [InlineData("000.00:00:02", "Just now")]
    [InlineData("000.00:00:25", "25 seconds ago")]
    [InlineData("000.00:01:00", "1 minute ago")]
    [InlineData("000.00:05:00", "5 minutes ago")]
    [InlineData("000.01:00:00", "1 hour ago")]
    [InlineData("000.05:00:00", "5 hours ago")]
    [InlineData("001.00:00:00", "1 day ago")]
    [InlineData("005.00:00:00", "5 days ago")]
    [InlineData("031.00:00:00", "1 month ago")]
    [InlineData("035.00:00:00", "2 months ago")]
    [InlineData("150.00:00:00", "6 months ago")]
    [InlineData("370.00:00:00", "1 year ago")]
    [InlineData("740.00:00:00", "2 years ago")]
    [InlineData("900.00:00:00", "2 years ago")]
    [InlineData("920.00:00:00", "3 years ago")]
    public void ToTimeAgo_Tests(string delayAsString, string expectedValue)
    {
        var delay = TimeSpan.ParseExact(delayAsString, @"ddd\.hh\:mm\:ss", null);

        Assert.Equal(expectedValue, delay.ToTimeAgo());
    }
}
