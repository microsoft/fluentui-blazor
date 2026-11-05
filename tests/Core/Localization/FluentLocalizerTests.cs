// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using System.Reflection;
using System.Resources;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Localization;

public class FluentLocalizerTests : Bunit.BunitContext
{
    [Theory]
    [InlineData("en")]
    [InlineData("fr")]
    public void FluentLocalizer_Default(string language)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);

        // Arrange
        Services.AddFluentUIComponents();
        var localizer = Services.GetRequiredService<IFluentLocalizer>();

        // Act
        var value1 = localizer["Fake_Hello", "Denis"];                  // Only English value is available in the FluentUI-Blazor lib
        var value2 = localizer.GetDefault("Fake_Hello", "Denis");       // GetDefault returns always the English value.

        // Assert
        Assert.Equal("Hello Denis", value1);
        Assert.Equal("Hello Denis", value2);
    }

    [Theory]
    [InlineData("de-DE", "Hello 4321,5")]
    [InlineData("de-CH", "Hello 4321.5")]
    [InlineData("fr-BE", "Hello 4321,5")]
    public void FluentLocalizer_DefaultArguments_UsesCurrentCulture(string cultureName, string expected)
    {
        var originalCulture = CultureInfo.CurrentCulture;
        var originalUICulture = CultureInfo.CurrentUICulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(cultureName);
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            Services.AddFluentUIComponents();
            var localizer = Services.GetRequiredService<IFluentLocalizer>();

            var value = localizer["Fake_Hello", 4321.5m];

            Assert.Equal(expected, value);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
            CultureInfo.CurrentUICulture = originalUICulture;
        }
    }

    [Theory]
    [InlineData("en")]
    [InlineData("fr")]
    public void FluentLocalizer_Customized(string language)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);

        // Arrange
        Services.AddFluentUIComponents(config =>
        {
            config.Localizer = new CustomLocalizer();
        });

        var localizer = Services.GetRequiredService<IFluentLocalizer>();

        // Act - GetDefault returns always the English value.
        var value = localizer["Fake_Hello"];

        // Assert
        Assert.Equal($"CustomLocalizer '{language}'", value);
    }

    [Theory]
    [InlineData("en", "Hello Denis")]
    [InlineData("fr", "Bonjour Denis")]     // See the "FluentLocalizer.fr.resx" embedded resource file in the project
    [InlineData("nl", "Dag Denis")]         // See the "FluentLocalizer.nl.resx" embedded resource file in the project
    public void FluentLocalizer_EmbeddedResources(string language, string expectedValue)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);

        // Arrange
        Services.AddFluentUIComponents(config => config.Localizer = new EmbeddedLocalizer());
        var localizer = Services.GetRequiredService<IFluentLocalizer>();

        // Act - GetDefault returns always the English value.
        var value = localizer["Fake_Hello", "Denis"];

        // Assert
        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("en", "Hello Denis")]
    [InlineData("fr", "Bonjour Denis")]     // See the "FluentLocalizer.fr.resx" embedded resource file in the project
    [InlineData("nl", "Dag Denis")]         // See the "FluentLocalizer.nl.resx" embedded resource file in the project
    public void FluentLocalizer_EmbeddedCodeGeneratedResources(string language, string expectedValue)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);

        // Arrange
        Services.AddFluentUIComponents(config => config.Localizer = new EmbeddedCodeGeneratedLocalizer());
        var localizer = Services.GetRequiredService<IFluentLocalizer>();

        // Act - GetDefault returns always the English value.
        var value = localizer["Fake_Hello", "Denis"];

        // Assert
        Assert.Equal(expectedValue, value);
    }

    [Fact]
    public void FluentLocalizer_UnknownKey()
    {
        // Arrange
        Services.AddFluentUIComponents();
        var localizer = Services.GetRequiredService<IFluentLocalizer>();

        // Act // ArgumentException($"Key '{key}' not found in LanguageResource."
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            var value = localizer["INVALID_KEY"];
        });

        // Assert
        Assert.Equal("Key 'INVALID_KEY' not found in LanguageResource. (Parameter 'key')", ex.Message);
    }

    private class CustomLocalizer : IFluentLocalizer
    {
        public string this[string key, params object[] arguments]
            => $"CustomLocalizer '{CultureInfo.CurrentUICulture.TwoLetterISOLanguageName}'";
    }
}
