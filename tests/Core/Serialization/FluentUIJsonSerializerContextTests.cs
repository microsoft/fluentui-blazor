// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json;
using Xunit;

#pragma warning disable FLUENTUI0001

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Serialization;

public class FluentUIJsonSerializerContextTests
{
    [Fact]
    public void KeyPress_RoundTrips()
    {
        const string expected = """
            {
              "key": 13,
              "ctrlKey": true,
              "shiftKey": false,
              "altKey": true,
              "metaKey": false,
              "preventDefault": false
            }
            """;
        var value = KeyPress.For(KeyCode.Enter).AndCtrlKey().AndAltKey().WithPreventDefault(false);

        var json = JsonSerializer.Serialize(value, FluentUIJsonSerializerContext.Default.KeyPress);
        var result = JsonSerializer.Deserialize(json, FluentUIJsonSerializerContext.Default.KeyPress);

        Assert.NotNull(result);
        Assert.Equal(value, result);
        Assert.Equal(expected.ReplaceLineEndings("\n"), json.ReplaceLineEndings("\n"));
    }

    [Fact]
    public void KeyPressArray_RoundTrips()
    {
        const string expected = """
            [
              {
                "key": 27,
                "ctrlKey": false,
                "shiftKey": true,
                "altKey": false,
                "metaKey": false,
                "preventDefault": true
              }
            ]
            """;
        KeyPress[] value = [KeyPress.For(KeyCode.Escape).AndShiftKey()];

        var json = JsonSerializer.Serialize(value, FluentUIJsonSerializerContext.Default.KeyPressArray);
        var result = JsonSerializer.Deserialize(json, FluentUIJsonSerializerContext.Default.KeyPressArray);

        Assert.NotNull(result);
        Assert.Equal(value, result);
        Assert.Equal(expected.ReplaceLineEndings("\n"), json.ReplaceLineEndings("\n"));
    }

    [Fact]
    public void ThemeSettingsDictionary_SerializesScalarValues()
    {
        const string expected = """
            {
              "color": "#0078D4",
              "hueTorsion": 0.1,
              "vibrancy": 0.2,
              "mode": null,
              "isExact": true
            }
            """;
        var value = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            ["color"] = "#0078D4",
            ["hueTorsion"] = 0.1,
            ["vibrancy"] = 0.2,
            ["mode"] = null!,
            ["isExact"] = true,
        };

        var json = JsonSerializer.Serialize(value, FluentUIJsonSerializerContext.Default.DictionaryStringObject);

        Assert.Equal(expected.ReplaceLineEndings("\n"), json.ReplaceLineEndings("\n"));
    }
}

#pragma warning restore FLUENTUI0001