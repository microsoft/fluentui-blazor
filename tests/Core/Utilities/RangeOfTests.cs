// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Utilities;

public class RangeOfTests
{
    [Fact]
    public void RangeOf_DefaultAndConfiguredValues()
    {
        var empty = new RangeOf<int>();
        var configured = new RangeOf<int>(3, 1);

        Assert.True(empty.IsEmpty());
        Assert.True(configured.IsValid());
        Assert.Equal(1, configured.Includes(1) ? 1 : 0);
        Assert.Equal("From 3 to 1.", configured.ToString());
    }

    [Fact]
    public void RangeOf_SingleAndClear()
    {
        var range = new RangeOf<int>(4, 4);

        Assert.True(range.IsSingle());
        Assert.False(range.Includes(3));

        range.Clear();

        Assert.True(range.IsEmpty());
        Assert.False(range.Includes(4));
    }

    [Fact]
    public void RangeOf_GetRangeValues_HandlesEachBoundary()
    {
        var range = new TestRange();

        Assert.Equal([1, 2, 3], range.Values(new RangeOf<int>(1, 3)));
        Assert.Equal([2], range.Values(new RangeOf<int>(null, 2)));
        Assert.Equal([2], range.Values(new RangeOf<int>(2, null)));
        Assert.Empty(range.Values(new RangeOf<int>()));
    }

    private sealed class TestRange : RangeOf<int>
    {
        public int[] Values(RangeOf<int> source)
        {
            Start = source.Start;
            End = source.End;
            return GetRangeValues((min, max) => Enumerable.Range(min, max - min + 1).ToArray());
        }
    }
}
