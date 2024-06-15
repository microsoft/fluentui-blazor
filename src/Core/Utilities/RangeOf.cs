// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.Utilities;

/// <summary>
/// Manage a range of values.
/// </summary>
/// <typeparam name="T"></typeparam>
public class RangeOf<T> where T : struct, IComparable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RangeOf{T}"/> class.
    /// </summary>
    public RangeOf()
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RangeOf{T}"/> class.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public RangeOf(T? start, T? end)
    {
        Start = start;
        End = end;
    }

    /// <summary>
    /// Gets or sets the start value.
    /// </summary>
    public virtual T? Start { get; set; }

    /// <summary>
    /// Gets or sets the end value.
    /// </summary>
    public virtual T? End { get; set; }

    /// <summary>
    /// Gets whether the range is empty: <see cref="Start"/> and <see cref="End"/> are null.
    /// </summary>
    public virtual bool IsEmpty() => !Start.HasValue && !End.HasValue;

    /// <summary>
    /// Gets whether the range is valid: <see cref="Start"/> and <see cref="End"/> are not null and are not identical.
    /// </summary>
    public virtual bool IsValid() => Start.HasValue && End.HasValue && Start.Value.CompareTo(End.Value) != 0;

    /// <summary>
    /// Gets whether the range is valid: <see cref="Start"/> and <see cref="End"/> are not null and are identical.
    /// </summary>
    public virtual bool IsSingle() => Start.HasValue && End.HasValue && Start.Value.CompareTo(End.Value) == 0;

    /// <summary>
    /// Clear the range: <see cref="Start"/> and <see cref="End"/> are set to null.
    /// </summary>
    public virtual void Clear() => Start = End = null;    

    /// <summary>
    /// Returns whether the range includes the specified value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public virtual bool Includes(T value)
    {
        var min = Min();
        var max = Max();

        if (!min.HasValue && !max.HasValue)
        {
            return false;
        }

        return !(min.HasValue && value.CompareTo(min.Value) < 0) &&
               !(max.HasValue && value.CompareTo(max.Value) > 0);
    }

    /// <summary>
    /// Returns all values between <see cref="Start"/> and <see cref="End"/>, using the specified function to compute the intermediate values between the Min and the Max values.
    /// </summary>
    /// <param name="allBetweenMinAndMax"></param>
    /// <returns></returns>
    protected virtual T[] GetRangeValues(Func<T, T, T[]> allBetweenMinAndMax)
    {
        var min = Min();
        var max = Max();

        if (min.HasValue && max.HasValue)
        {
            return allBetweenMinAndMax.Invoke(min.Value, max.Value);
        }
        else if (min.HasValue)
        {
            return new[] { min.Value };
        }
        else if (max.HasValue)
        {
            return new[] { max.Value };
        }

        return Array.Empty<T>();
    }

    /// <summary />
    protected virtual T? Min() => IsStartLowerThanEnd() ? Start : End;

    /// <summary />
    protected virtual T? Max() => IsStartLowerThanEnd() ? End : Start;

    /// <summary />
    private bool IsStartLowerThanEnd() => Start.HasValue && End.HasValue && Start.Value.CompareTo(End.Value) < 0;

    public override string ToString()
    {
        return $"From {Start} to {End}.";
    }
}
