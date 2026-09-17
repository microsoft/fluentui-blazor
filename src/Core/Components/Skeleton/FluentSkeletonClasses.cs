// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0048:File name must match type name", Justification = "To have a clearer codebase")]
public partial class FluentSkeleton
{
    /// <summary>
    /// Fill the component with a skeleton loading effect: a rectangle with a height of 4 pixels.
    /// </summary>
    public static readonly string ClassRectangle1 = "fluent-skeleton-1";

    /// <summary>
    /// Fill the component with a skeleton loading effect: a rectangle with a height of 8 pixels.
    /// </summary>
    public static readonly string ClassRectangle2 = "fluent-skeleton-2";

    /// <summary>
    /// Fill the component with a skeleton loading effect: a rectangle with a height of 12 pixels.
    /// </summary>
    public static readonly string ClassRectangle3 = "fluent-skeleton-3";

    /// <summary>
    /// Fill the component with a skeleton loading effect: a rectangle with a height of 16 pixels.
    /// </summary>
    public static readonly string ClassRectangle4 = "fluent-skeleton-4";

    /// <summary>
    /// Fill the component with a skeleton loading effect: a rectangle with a height of 20 pixels.
    /// </summary>
    public static readonly string ClassRectangle5 = "fluent-skeleton-5";

    /// <summary>
    /// Fill the component with a skeleton loading effect: a rectangle with a height of 24 pixels.
    /// </summary>
    public static readonly string ClassRectangle6 = "fluent-skeleton-6";

    /// <summary>
    /// Fill the component with a skeleton loading effect: a rectangle with a height of 28 pixels.
    /// </summary>
    public static readonly string ClassRectangle7 = "fluent-skeleton-7";

    /// <summary>
    /// Fill the component with a skeleton loading effect: a rectangle with a height of 32 pixels.
    /// </summary>
    public static readonly string ClassRectangle8 = "fluent-skeleton-8";

    /// <summary>
    /// Fill the component with a skeleton loading effect: a circle with a radius of 4 pixels.
    /// </summary>
    public static readonly string ClassCircular1 = "fluent-skeleton-circular-1";

    /// <summary>
    /// Fill the component with a skeleton loading effect: a circle with a radius of 8 pixels.
    /// </summary>
    public static readonly string ClassCircular2 = "fluent-skeleton-circular-2";

    /// <summary>
    /// Fill the component with a skeleton loading effect: a circle with a radius of 12 pixels.
    /// </summary>
    public static readonly string ClassCircular3 = "fluent-skeleton-circular-3";

    /// <summary>
    /// Fill the component with a skeleton loading effect: a circle with a radius of 16 pixels.
    /// </summary>
    public static readonly string ClassCircular4 = "fluent-skeleton-circular-4";

    /// <summary>
    /// Fill the component with a skeleton loading effect: a circle with a radius of 20 pixels.
    /// </summary>
    public static readonly string ClassCircular5 = "fluent-skeleton-circular-5";

    /// <summary>
    /// Fill the component with a skeleton loading effect: a circle with a radius of 24 pixels.
    /// </summary>
    public static readonly string ClassCircular6 = "fluent-skeleton-circular-6";

    /// <summary>
    /// Fill the component with a skeleton loading effect: a circle with a radius of 28 pixels.
    /// </summary>
    public static readonly string ClassCircular7 = "fluent-skeleton-circular-7";

    /// <summary>
    /// Fill the component with a skeleton loading effect: a circle with a radius of 32 pixels.
    /// </summary>
    public static readonly string ClassCircular8 = "fluent-skeleton-circular-8";

    /// <summary>
    /// Generates a class name for a loading skeleton based on the specified parameters.
    /// </summary>
    /// <param name="condition">An optional boolean value indicating whether the loading skeleton should be generated.</param>
    /// <param name="size">The size of the loading skeleton: must be between 0 and 8.</param>
    /// <param name="circular">A boolean value indicating whether the loading skeleton should have a circular shape.</param>
    /// <returns>A string representing the class name for the loading skeleton</returns>
    public static string? LoadingClass(bool? condition = true, uint size = 0, bool circular = false)
    {
        if (size < 0 || size > 8)
        {
            throw new ArgumentOutOfRangeException(nameof(size), "Size must be between 0 and 8.");
        }

        if (condition == false)
        {
            return null;
        }

        return $"fluent-skeleton{(circular ? "-circular" : null)}-{size}";
    }

    /// <summary>
    /// Generates a class name for a loading skeleton based on the specified parameters.
    /// </summary>
    /// <param name="when">An boolean condition indicating whether the loading skeleton should be generated.</param>
    /// <param name="size">The size of the loading skeleton: must be between 0 and 8.</param>
    /// <param name="circular">A boolean value indicating whether the loading skeleton should have a circular shape.</param>
    /// <returns>A string representing the class name for the loading skeleton</returns>
    public static string? LoadingClass(Func<bool> when, uint size = 0, bool circular = false)
    {
        if (when == null)
        {
            throw new ArgumentNullException(nameof(when), "Condition function cannot be null.");
        }

        return LoadingClass(when.Invoke(), size, circular);
    }
}
