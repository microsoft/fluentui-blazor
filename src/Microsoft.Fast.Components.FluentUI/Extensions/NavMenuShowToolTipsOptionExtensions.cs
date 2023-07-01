namespace Microsoft.Fast.Components.FluentUI;

internal static class NavMenuShowToolTipsOptionExtensions
{
    public static bool ShouldDisplay(this NavMenuShowToolTipsOption option, bool expanded) =>
        option switch
        {
            NavMenuShowToolTipsOption.Never => false,
            NavMenuShowToolTipsOption.Always => true,
            NavMenuShowToolTipsOption.CollapsedOnly => !expanded,
            NavMenuShowToolTipsOption.ExpandedOnly => expanded,
            _ => throw new NotImplementedException(
                $"Combination not supported: {nameof(option)}={option}, {nameof(expanded)}={expanded}")
        };
}
