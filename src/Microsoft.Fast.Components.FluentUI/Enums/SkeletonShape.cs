namespace Microsoft.Fast.Components.FluentUI;

public enum SkeletonShape
{
    Rect,
    Circle
}

public static class SkeletonShapeExtensions
{
    private static readonly Dictionary<SkeletonShape, string> _shapeValues =
        Enum.GetValues<SkeletonShape>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this SkeletonShape? value) => value == null ? null : _shapeValues[value.Value];
}