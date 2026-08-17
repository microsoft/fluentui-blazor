// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Serialization;

public class FluentUIJsonSerializerContextTests
{
    [Fact]
    public void KeyPress_RoundTrips()
    {
        var value = KeyPress.For(KeyCode.Enter).AndCtrlKey().AndAltKey().WithPreventDefault(false);

        var json = JsonSerializer.Serialize(value, FluentUIJsonSerializerContext.Default.KeyPress);
        var result = JsonSerializer.Deserialize(json, FluentUIJsonSerializerContext.Default.KeyPress);

        Assert.NotNull(result);
        Assert.Equal(value, result);
        Assert.Contains("\"key\"", json, StringComparison.Ordinal);
        Assert.Contains("\"ctrlKey\"", json, StringComparison.Ordinal);
        Assert.Contains("\"preventDefault\"", json, StringComparison.Ordinal);
    }

    [Fact]
    public void KeyPressArray_RoundTrips()
    {
        KeyPress[] value = [KeyPress.For(KeyCode.Escape).AndShiftKey()];

        var json = JsonSerializer.Serialize(value, FluentUIJsonSerializerContext.Default.KeyPressArray);
        var result = JsonSerializer.Deserialize(json, FluentUIJsonSerializerContext.Default.KeyPressArray);

        Assert.NotNull(result);
        Assert.Equal(value, result);
    }

    [Fact]
    public void ThemeSettingsDto_UsesCamelCaseProperties()
    {
        var value = new ThemeSettingsDto("#0078D4", 0.1, 0.2, "dark", true);

        var json = JsonSerializer.Serialize(value, FluentUIJsonSerializerContext.Default.ThemeSettingsDto);
        var result = JsonSerializer.Deserialize(json, FluentUIJsonSerializerContext.Default.ThemeSettingsDto);

        Assert.Equal(value, result);
        Assert.Contains("\"color\"", json, StringComparison.Ordinal);
        Assert.Contains("\"hueTorsion\"", json, StringComparison.Ordinal);
        Assert.Contains("\"isExact\"", json, StringComparison.Ordinal);
    }
}