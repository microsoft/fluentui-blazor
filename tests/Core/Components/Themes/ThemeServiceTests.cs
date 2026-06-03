// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Themes;

public class ThemeServiceTests
{
    [Fact]
    public async Task CreateCustomThemeAsync_WhenJsReturnsNull_Throws()
    {
        var js = new FakeJSRuntime();
        js.SetResult("Blazor.theme.createBrandTheme", (Dictionary<string, object?>?)null);
        var sut = new ThemeService(js);

        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.CreateCustomThemeAsync(new ThemeSettings("", 0, 0, ThemeMode.Dark)));
        var inv = Assert.Single(js.Invocations);
        Assert.Equal("Blazor.theme.createBrandTheme", inv.Identifier);
    }

    [Fact]
    public async Task CreateCustomThemeAsync_WhenJsReturnsDictionary_ReturnsTheme()
    {
        var js = new FakeJSRuntime();
        js.SetResult(
            "Blazor.theme.createBrandTheme",
            new Dictionary<string, object?>
            {
                ["borderRadiusSmall"] = "2px",
                ["fontWeightBold"] = 700,
            });

        var sut = new ThemeService(js);
        var result = await sut.CreateCustomThemeAsync(new ThemeSettings("#0078D4", 0.1, 0.2, ThemeMode.Light, IsExact: true));

        Assert.NotNull(result);

        var dict = result!.ToCompactDictionary();
        Assert.Equal("2px", dict["borderRadiusSmall"]);
        Assert.Equal(700, dict["fontWeightBold"]);

        var inv = Assert.Single(js.Invocations);
        Assert.Equal("Blazor.theme.createBrandTheme", inv.Identifier);
        var payload = Assert.Single(inv.Arguments);
        Assert.Equal("#0078D4", GetPropertyValue<string>(payload!, "color"));
        Assert.Equal(0.1, GetPropertyValue<double>(payload!, "hueTorsion"));
        Assert.Equal(0.2, GetPropertyValue<double>(payload!, "vibrancy"));
        Assert.Equal("light", GetPropertyValue<string>(payload!, "mode"));
        Assert.True(GetPropertyValue<bool>(payload!, "isExact"));
    }

    [Fact]
    public async Task SetThemeAsync_Theme_InvokesSetBrandThemeFromTheme()
    {
        var js = new FakeJSRuntime();
        var sut = new ThemeService(js);
        var theme = new Theme();
        theme.Borders.Radius.Small = "2px";

        await sut.SetThemeAsync(theme);

        var inv = Assert.Single(js.Invocations);
        Assert.Equal("Blazor.theme.setBrandThemeFromTheme", inv.Identifier);
        var arg = Assert.Single(inv.Arguments);
        Assert.NotNull(arg);

        var dict = Assert.IsAssignableFrom<IDictionary<string, object?>>(arg!);
        Assert.Equal("2px", dict["borderRadiusSmall"]);
    }

    [Fact]
    public async Task SetThemeAsync_ThemeTypeDefault_InvokesSetWebTheme()
    {
        var js = new FakeJSRuntime();
        var sut = new ThemeService(js);

        await sut.SetThemeAsync(ThemeColorVariant.Default);

        var inv = Assert.Single(js.Invocations);
        Assert.Equal("Blazor.theme.setWebTheme", inv.Identifier);
        Assert.Empty(inv.Arguments);
    }

    [Fact]
    public async Task SetThemeAsync_ThemeTypeTeams_InvokesSetTeamsTheme()
    {
        var js = new FakeJSRuntime();
        var sut = new ThemeService(js);

        await sut.SetThemeAsync(ThemeColorVariant.Teams);

        var inv = Assert.Single(js.Invocations);
        Assert.Equal("Blazor.theme.setTeamsTheme", inv.Identifier);
        Assert.Empty(inv.Arguments);
    }

    [Fact]
    public async Task SetThemeAsync_ThemeTypeInvalid_ThrowsArgumentOutOfRangeException()
    {
        var js = new FakeJSRuntime();
        var sut = new ThemeService(js);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.SetThemeAsync((ThemeColorVariant)123));
        Assert.Empty(js.Invocations);
    }

    [Fact]
    public async Task SetThemeAsync_TypeAndMode_Default_InvokesSetThemeModeWithAttributeValue()
    {
        var js = new FakeJSRuntime();
        var sut = new ThemeService(js);

        await sut.SetThemeAsync(ThemeColorVariant.Default, ThemeMode.Dark);

        var inv = Assert.Single(js.Invocations);
        Assert.Equal("Blazor.theme.setThemeMode", inv.Identifier);
        Assert.Equal("dark", Assert.Single(inv.Arguments));
    }

    [Fact]
    public async Task SetThemeAsync_TypeAndMode_Teams_InvokesSetTeamsThemeModeWithAttributeValue()
    {
        var js = new FakeJSRuntime();
        var sut = new ThemeService(js);

        await sut.SetThemeAsync(ThemeColorVariant.Teams, ThemeMode.System);

        var inv = Assert.Single(js.Invocations);
        Assert.Equal("Blazor.theme.setTeamsThemeMode", inv.Identifier);
        Assert.Equal("system", Assert.Single(inv.Arguments));
    }

    [Fact]
    public async Task SetThemeAsync_TypeAndMode_InvalidType_ThrowsArgumentOutOfRangeException()
    {
        var js = new FakeJSRuntime();
        var sut = new ThemeService(js);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.SetThemeAsync((ThemeColorVariant)123, ThemeMode.Dark));
        Assert.Empty(js.Invocations);
    }

    [Fact]
    public async Task SetThemeAsync_Mode_InvokesSetThemeModeWithAttributeValue()
    {
        var js = new FakeJSRuntime();
        var sut = new ThemeService(js);

        await sut.SetThemeAsync(ThemeMode.Dark);

        var inv = Assert.Single(js.Invocations);
        Assert.Equal("Blazor.theme.setThemeMode", inv.Identifier);
        Assert.Equal("dark", Assert.Single(inv.Arguments));
    }

    [Fact]
    public async Task SetThemeAsync_Settings_SystemMode_PassesNullMode()
    {
        var js = new FakeJSRuntime();
        var sut = new ThemeService(js);

        await sut.SetThemeAsync(new ThemeSettings("#0078D4", 0, 0, ThemeMode.System));

        var inv = Assert.Single(js.Invocations);
        Assert.Equal("Blazor.theme.setBrandThemeFromSettings", inv.Identifier);
        var payload = Assert.Single(inv.Arguments);
        Assert.Null(GetPropertyValue<object?>(payload!, "mode"));
    }

    [Fact]
    public async Task SetThemeAsync_Settings_DarkModeAndIsExact_PassesModeAndIsExact()
    {
        var js = new FakeJSRuntime();
        var sut = new ThemeService(js);

        await sut.SetThemeAsync(new ThemeSettings("#0078D4", 0.1, 0.2, ThemeMode.Dark, IsExact: true));

        var inv = Assert.Single(js.Invocations);
        Assert.Equal("Blazor.theme.setBrandThemeFromSettings", inv.Identifier);
        var payload = Assert.Single(inv.Arguments);
        Assert.Equal("dark", GetPropertyValue<string>(payload!, "mode"));
        Assert.True(GetPropertyValue<bool>(payload!, "isExact"));
    }

    [Fact]
    public async Task SwitchThemeAsync_ReturnsValueFromJs()
    {
        var js = new FakeJSRuntime();
        js.SetResult("Blazor.theme.switchTheme", true);
        var sut = new ThemeService(js);

        var result = await sut.SwitchThemeAsync();

        Assert.True(result);
        var inv = Assert.Single(js.Invocations);
        Assert.Equal("Blazor.theme.switchTheme", inv.Identifier);
        Assert.Equal(CancellationToken.None, inv.CancellationToken);
    }

    [Fact]
    public async Task SetThemeToElementAsync_FromSettings_InvokesJsWithElementReference()
    {
        var js = new FakeJSRuntime();
        var sut = new ThemeService(js);
        var element = default(ElementReference);
        var settings = new ThemeSettings("#0078D4", 0, 0, ThemeMode.Dark, IsExact: true);

        await sut.SetThemeToElementAsync(element, settings);

        var inv = Assert.Single(js.Invocations);
        Assert.Equal("Blazor.theme.setBrandThemeToElementFromSettings", inv.Identifier);
        Assert.Equal(2, inv.Arguments.Count);
        Assert.IsType<ElementReference>(inv.Arguments[0]);
        Assert.Equal("#0078D4", GetPropertyValue<string>(inv.Arguments[1]!, "color"));
        Assert.Equal("dark", GetPropertyValue<string>(inv.Arguments[1]!, "mode"));
        Assert.True(GetPropertyValue<bool>(inv.Arguments[1]!, "isExact"));
    }

    [Fact]
    public async Task SetThemeToElementAsync_FromSettings_SystemMode_PassesNullMode()
    {
        var js = new FakeJSRuntime();
        var sut = new ThemeService(js);
        var element = default(ElementReference);
        var settings = new ThemeSettings("#0078D4", 0, 0, ThemeMode.System);

        await sut.SetThemeToElementAsync(element, settings);

        var inv = Assert.Single(js.Invocations);
        Assert.Equal("Blazor.theme.setBrandThemeToElementFromSettings", inv.Identifier);
        Assert.Equal(2, inv.Arguments.Count);
        Assert.IsType<ElementReference>(inv.Arguments[0]);
        Assert.Null(GetPropertyValue<object?>(inv.Arguments[1]!, "mode"));
    }

    [Fact]
    public async Task SetThemeAsync_StringColor_InvokesSetBrandThemeFromSettingsWithSystemMode()
    {
        var js = new FakeJSRuntime();
        var sut = new ThemeService(js);

        await sut.SetThemeAsync("#ff0000", isExact: true);

        var inv = Assert.Single(js.Invocations);
        Assert.Equal("Blazor.theme.setBrandThemeFromSettings", inv.Identifier);
        var payload = Assert.Single(inv.Arguments);
        Assert.Equal("#ff0000", GetPropertyValue<string>(payload!, "color"));
        Assert.Equal(0d, GetPropertyValue<double>(payload!, "hueTorsion"));
        Assert.Equal(0d, GetPropertyValue<double>(payload!, "vibrancy"));
        Assert.Null(GetPropertyValue<object?>(payload!, "mode"));
        Assert.True(GetPropertyValue<bool>(payload!, "isExact"));
    }

    [Fact]
    public async Task SetThemeAsync_StringColor_DefaultIsExactFalse()
    {
        var js = new FakeJSRuntime();
        var sut = new ThemeService(js);

        await sut.SetThemeAsync("#ff0000");

        var inv = Assert.Single(js.Invocations);
        Assert.Equal("Blazor.theme.setBrandThemeFromSettings", inv.Identifier);
        var payload = Assert.Single(inv.Arguments);
        Assert.Equal("#ff0000", GetPropertyValue<string>(payload!, "color"));
        Assert.False(GetPropertyValue<bool>(payload!, "isExact"));
    }

    [Fact]
    public async Task SetThemeAsync_Settings_LightMode_PassesModeAsAttributeValue()
    {
        var js = new FakeJSRuntime();
        var sut = new ThemeService(js);

        await sut.SetThemeAsync(new ThemeSettings("#0078D4", 0, 0, ThemeMode.Light));

        var inv = Assert.Single(js.Invocations);
        Assert.Equal("Blazor.theme.setBrandThemeFromSettings", inv.Identifier);
        var payload = Assert.Single(inv.Arguments);
        Assert.Equal("light", GetPropertyValue<string>(payload!, "mode"));
    }

    [Fact]
    public async Task SwitchDirectionAsync_InvokesSwitchDirection()
    {
        var js = new FakeJSRuntime();
        var sut = new ThemeService(js);

        await sut.SwitchDirectionAsync();

        var inv = Assert.Single(js.Invocations);
        Assert.Equal("Blazor.theme.switchDirection", inv.Identifier);
        Assert.Empty(inv.Arguments);
    }

    [Fact]
    public async Task ClearStoredThemeSettingsAsync_InvokesClearStoredThemeSettings()
    {
        var js = new FakeJSRuntime();
        var sut = new ThemeService(js);

        await sut.ClearStoredThemeSettingsAsync();

        var inv = Assert.Single(js.Invocations);
        Assert.Equal("Blazor.theme.clearStoredThemeSettings", inv.Identifier);
        Assert.Empty(inv.Arguments);
    }

    [Fact]
    public async Task IsSystemDarkAsync_ReturnsValueFromJs()
    {
        var js = new FakeJSRuntime();
        js.SetResult("Blazor.theme.isSystemDark", true);
        var sut = new ThemeService(js);

        var result = await sut.IsSystemDarkAsync();

        Assert.True(result);
        var inv = Assert.Single(js.Invocations);
        Assert.Equal("Blazor.theme.isSystemDark", inv.Identifier);
        Assert.Empty(inv.Arguments);
    }

    [Fact]
    public async Task IsDarkModeAsync_ReturnsValueFromJs()
    {
        var js = new FakeJSRuntime();
        js.SetResult("Blazor.theme.isDarkMode", false);
        var sut = new ThemeService(js);

        var result = await sut.IsDarkModeAsync();

        Assert.False(result);
        var inv = Assert.Single(js.Invocations);
        Assert.Equal("Blazor.theme.isDarkMode", inv.Identifier);
        Assert.Empty(inv.Arguments);
    }

    [Fact]
    public async Task GetColorRampAsync_ReturnsDictionaryFromJs()
    {
        var js = new FakeJSRuntime();
        js.SetResult(
            "Blazor.theme.getColorRamp",
            new Dictionary<string, string>
            {
                ["10"] = "#111111",
                ["20"] = "#222222",
            });
        var sut = new ThemeService(js);

        var result = await sut.GetColorRampAsync();

        Assert.NotNull(result);
        Assert.Equal("#111111", result!["10"]);
        Assert.Equal("#222222", result!["20"]);
        var inv = Assert.Single(js.Invocations);
        Assert.Equal("Blazor.theme.getColorRamp", inv.Identifier);
    }

    [Fact]
    public async Task GetColorRampFromSettingsAsync_PassesSettingsAndReturnsDictionary()
    {
        var js = new FakeJSRuntime();
        js.SetResult(
            "Blazor.theme.getColorRampFromSettings",
            new Dictionary<string, string>
            {
                ["10"] = "#111111",
            });
        var sut = new ThemeService(js);
        var settings = new ThemeSettings("#0078D4", 0.25, -0.25, ThemeMode.Dark, IsExact: true);

        var result = await sut.GetColorRampFromSettingsAsync(settings);

        Assert.NotNull(result);
        Assert.Equal("#111111", result!["10"]);

        var inv = Assert.Single(js.Invocations);
        Assert.Equal("Blazor.theme.getColorRampFromSettings", inv.Identifier);
        var payload = Assert.Single(inv.Arguments);
        Assert.Equal("#0078D4", GetPropertyValue<string>(payload!, "color"));
        Assert.Equal(0.25, GetPropertyValue<double>(payload!, "hueTorsion"));
        Assert.Equal(-0.25, GetPropertyValue<double>(payload!, "vibrancy"));
        Assert.Equal("dark", GetPropertyValue<string>(payload!, "mode"));
        Assert.True(GetPropertyValue<bool>(payload!, "isExact"));
    }

    [Fact]
    public async Task GetBrandColorAsync_ReturnsValueFromJs()
    {
        var js = new FakeJSRuntime();
        js.SetResult("Blazor.theme.getBrandColor", "#0078D4");
        var sut = new ThemeService(js);

        var result = await sut.GetBrandColorAsync();

        Assert.Equal("#0078D4", result);
        var inv = Assert.Single(js.Invocations);
        Assert.Equal("Blazor.theme.getBrandColor", inv.Identifier);
        Assert.Empty(inv.Arguments);
    }

    [Fact]
    public async Task SetThemeToElementAsync_FromTheme_InvokesJsWithElementReference()
    {
        var js = new FakeJSRuntime();
        var sut = new ThemeService(js);
        var element = default(ElementReference);
        var theme = new Theme();
        theme.Borders.Radius.Small = "2px";

        await sut.SetThemeToElementAsync(element, theme);

        var inv = Assert.Single(js.Invocations);
        Assert.Equal("Blazor.theme.setBrandThemeToElementFromTheme", inv.Identifier);
        Assert.Equal(2, inv.Arguments.Count);
        Assert.IsType<ElementReference>(inv.Arguments[0]);

        var dict = Assert.IsAssignableFrom<IDictionary<string, object?>>(inv.Arguments[1]!);
        Assert.Equal("2px", dict["borderRadiusSmall"]);
    }
    private static T? GetPropertyValue<T>(object obj, string propertyName)
    {
        var prop = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
        Assert.NotNull(prop);
        return (T?)prop!.GetValue(obj);
    }

    private sealed class FakeJSRuntime : IJSRuntime
    {
        private readonly Dictionary<string, object?> _results = new(StringComparer.Ordinal);

        public List<Invocation> Invocations { get; } = new();

        public void SetResult(string identifier, object? result) => _results[identifier] = result;

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
            => InvokeAsync<TValue>(identifier, CancellationToken.None, args);

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
        {
            Invocations.Add(new Invocation(identifier, cancellationToken, args ?? Array.Empty<object?>()));

            if (_results.TryGetValue(identifier, out var result))
            {
                return new ValueTask<TValue>((TValue)(object?)result!);
            }

            return new ValueTask<TValue>(default(TValue)!);
        }

        public sealed record Invocation(string Identifier, CancellationToken CancellationToken, IReadOnlyList<object?> Arguments);
    }
}
