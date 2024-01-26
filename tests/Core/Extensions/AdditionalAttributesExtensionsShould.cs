using System.Collections;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Extensions;

public class AdditionalAttributesExtensionsShould
{
    public sealed class RenderedAttributesEqualityTestData : IEnumerable<object?[]>
    {
        public IEnumerator<object?[]> GetEnumerator()
        {
            yield return new object?[]
            {
                null,
                null,
                true
            };

            yield return new object?[]
            {
                null,
                new Dictionary<string,object>(0),
                true
            };

            yield return new object?[]
            {
                new Dictionary<string,object>(0),
                new Dictionary<string,object>(0),
                true
            };
            yield return new object?[]
            {
                null,
                new Dictionary<string,object?> {["attr"] = null},
                true
            };
            yield return new object?[]
            {
                new Dictionary<string,object>(0),
                new Dictionary<string,object?> {["attr"] = null},
                true
            };
            yield return new object?[]
            {
                new Dictionary<string,object> { ["attr"] = "value" },
                new Dictionary<string,object> { ["attr"] = "value" },
                true
            };
            yield return new object?[]
            {
                new Dictionary<string,object> { ["attr"] = "value" },
                new Dictionary<string,object> { ["attr2"] = "value" },
                false
            };
            yield return new object?[]
            {
                new Dictionary<string,object> { ["attr"] = "value", ["attr2"] = "value2" },
                new Dictionary<string,object> { ["attr2"] = "value2" },
                false
            };
            yield return new object?[]
            {
                new Dictionary<string,object> { ["attr"] = "value", ["attr2"] = 15 },
                new Dictionary<string,object> { ["attr"] = "value", ["attr2"] = 15 },
                true
            };
            yield return new object?[]
            {
                new Dictionary<string,object> { ["attr"] = "value", ["attr2"] = 15 },
                new Dictionary<string,object> { ["attr"] = "value", ["attr2"] = "value2" },
                false
            };
            yield return new object?[]
            {
                new Dictionary<string,object> { ["attr"] = "value" },
                new Dictionary<string,object?> { ["attr"] = "value", ["attr2"] = null },
                true
            };
            yield return new object?[]
            {
                new Dictionary<string,object> { ["attr"] = "value" },
                null,
                false
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    [Theory]
    [ClassData(typeof(RenderedAttributesEqualityTestData))]
    public void CompareProperly_RenderedAttributes(
        IReadOnlyDictionary<string, object>? x,
        IReadOnlyDictionary<string, object>? y,
        bool expected)
    {
        // Arrange && Act
        var actual = x.RenderedAttributesEqual(y);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [ClassData(typeof(RenderedAttributesEqualityTestData))]
    public void CompareProperly_RenderedAttributesOtherWayRound(
       IReadOnlyDictionary<string, object>? x,
       IReadOnlyDictionary<string, object>? y,
       bool expected)
    {
        // Arrange && Act
        var actual = y.RenderedAttributesEqual(x);

        // Assert
        Assert.Equal(expected, actual);
    }

}
