namespace Microsoft.FluentUI.AspNetCore.Components;

public static class AdditionalAttributesExtensions
{
    /// <summary> Determines whether two sets of attributes are equal when rendered. </summary>
    /// <param name="x">The compared set</param>
    /// <param name="y">The set to compare with</param>
    /// <remarks></remarks>
    /// <returns><c>true</c> if both sets render the same attributes; otherwise, <c>false</c>.</returns>
    public static bool RenderedAttributesEqual(
        this IReadOnlyDictionary<string, object>? x,
        IReadOnlyDictionary<string, object>? y)
    {
        if ((x?.Count ?? 0) == 0 && (y?.Count ?? 0) == 0)
        {
            return true;
        }

        if (x is { Count: > 0 } &&
            !x.AllRenderedAttributesInAndEqual(y))
        {
            return false;
        }

        if (y is { Count: > 0 } &&
            !y.AllRenderedAttributesInAndEqual(x))
        {
            return false;
        }

        return true;
    }

    private static bool AllRenderedAttributesInAndEqual(
        this IReadOnlyDictionary<string, object> x,
        IReadOnlyDictionary<string, object>? y)
    {
        foreach (var xKvp in x)
        {
            if (xKvp.Value is null)
            {
                continue;
            }

            if (y is null)
            {
                return false;
            }

            if (!y.TryGetValue(xKvp.Key, out var yValue) ||
                !xKvp.Value.Equals(yValue))
            {
                return false;
            }
        }

        return true;
    }
}
