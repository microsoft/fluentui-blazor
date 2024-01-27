using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.DateTime;

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
    public void ToTimeAgo_Default(string delayAsString, string expectedValue)
    {
        var delay = TimeSpan.ParseExact(delayAsString, @"ddd\.hh\:mm\:ss", null);

        Assert.Equal(expectedValue, delay.ToTimeAgo());
    }

    [Theory]
    [InlineData("000.00:00:02", "Maintenant")]
    [InlineData("000.00:00:25", "Il y a 25 secondes")]
    [InlineData("000.00:01:00", "Il y a une minute")]
    [InlineData("000.00:05:00", "Il y a 5 minutes")]
    [InlineData("000.01:00:00", "Il y a une heure")]
    [InlineData("000.05:00:00", "Il y a 5 heures")]
    [InlineData("001.00:00:00", "Il y a un jour")]
    [InlineData("005.00:00:00", "Il y a 5 jours")]
    [InlineData("031.00:00:00", "Il y a un mois")]
    [InlineData("035.00:00:00", "Il y a 2 mois")]
    [InlineData("150.00:00:00", "Il y a 6 mois")]
    [InlineData("370.00:00:00", "Il y a 1 an")]
    [InlineData("740.00:00:00", "Il y a 2 ans")]
    [InlineData("900.00:00:00", "Il y a 2 ans")]
    [InlineData("920.00:00:00", "Il y a 3 ans")]
    public void ToTimeAgo_Customized(string delayAsString, string expectedValue)
    {
        var delay = TimeSpan.ParseExact(delayAsString, @"ddd\.hh\:mm\:ss", null);
        var options = new TimeAgoOptions()
        {
            SecondAgo = "Maintenant",
            SecondsAgo = "Il y a {0} secondes",
            MinuteAgo = "Il y a une minute",
            MinutesAgo = "Il y a {0} minutes",
            HourAgo = "Il y a une heure",
            HoursAgo = "Il y a {0} heures",
            DayAgo = "Il y a un jour",
            DaysAgo = "Il y a {0} jours",
            MonthAgo = "Il y a un mois",
            MonthsAgo = "Il y a {0} mois",
            YearAgo = "Il y a {0} an",
            YearsAgo = "Il y a {0} ans",
        };

        Assert.Equal(expectedValue, delay.ToTimeAgo(options));
    }

    [Theory]
    [InlineData("000.00:00:02", "Just now")]

    public void ToTimeAgo_Ctor(string delayAsString, string expectedValue)
    {
        var delay = TimeSpan.ParseExact(delayAsString, @"ddd\.hh\:mm\:ss", null);

        Assert.Equal(expectedValue, delay.ToTimeAgo(resources: null));
    }
}
