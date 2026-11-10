// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Xunit;
using LanguageResource = Microsoft.FluentUI.AspNetCore.Components.Localization.LanguageResource;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Extensions;

public class ToTimeAgoTests
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
        Assert.Equal(expectedValue, delay.ToTimeAgo(new FrenchTimeAgoLocalizer()));
    }

    [Theory]
    [InlineData("de-DE")]
    [InlineData("de-CH")]
    [InlineData("fr-BE")]
    public void ToTimeAgo_Customized_UsesCurrentCulture(string cultureName)
    {
        var originalCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(cultureName);
            var delay = TimeSpan.FromDays(4321 * 365);
            var expectedValue = string.Format(CultureInfo.CurrentCulture, "Il y a {0:N0} ans", 4321);

            var result = delay.ToTimeAgo(new FrenchTimeAgoLocalizer());

            Assert.Equal(expectedValue, result);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    [Theory]
    [InlineData("000.00:00:02", "Just now")]
    public void ToTimeAgo_Ctor(string delayAsString, string expectedValue)
    {
        var delay = TimeSpan.ParseExact(delayAsString, @"ddd\.hh\:mm\:ss", null);

        Assert.Equal(expectedValue, delay.ToTimeAgo(localizer: null));
    }

    private class FrenchTimeAgoLocalizer : IFluentLocalizer
    {
        public string this[string key, params object[] arguments]
        {
            get
            {
                Assert.Single(arguments);

                var localizedString = key switch
                {
                    LanguageResource.TimeAgo_SecondAgo => "Maintenant",
                    LanguageResource.TimeAgo_SecondsAgo => "Il y a {0} secondes",
                    LanguageResource.TimeAgo_MinuteAgo => "Il y a une minute",
                    LanguageResource.TimeAgo_MinutesAgo => "Il y a {0} minutes",
                    LanguageResource.TimeAgo_HourAgo => "Il y a une heure",
                    LanguageResource.TimeAgo_HoursAgo => "Il y a {0} heures",
                    LanguageResource.TimeAgo_DayAgo => "Il y a un jour",
                    LanguageResource.TimeAgo_DaysAgo => "Il y a {0} jours",
                    LanguageResource.TimeAgo_MonthAgo => "Il y a un mois",
                    LanguageResource.TimeAgo_MonthsAgo => "Il y a {0} mois",
                    LanguageResource.TimeAgo_YearAgo => "Il y a {0} an",
                    LanguageResource.TimeAgo_YearsAgo => "Il y a {0:N0} ans",

                    // Fallback to the Default/English if no translation is found
                    _ => null,
                };

                return localizedString is null
                    ? IFluentLocalizer.GetDefault(key, arguments)
                    : string.Format(CultureInfo.CurrentCulture, localizedString, arguments);
            }
        }
    }
}
