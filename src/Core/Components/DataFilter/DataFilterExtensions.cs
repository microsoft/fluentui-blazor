namespace Microsoft.FluentUI.AspNetCore.Components;

public static class DataFilterExtensions
{
    /// <summary>
    /// Is operator in/not in.
    /// </summary>
    /// <param name="operator"></param>
    /// <returns></returns>
    public static bool IsIn(this DataFilterComparisonOperator @operator) =>
        @operator == DataFilterComparisonOperator.In || @operator == DataFilterComparisonOperator.NotIn;

    /// <summary>
    /// Is operator empty/not empty.
    /// </summary>
    /// <param name="operator"></param>
    /// <returns></returns>
    public static bool IsEmpty(this DataFilterComparisonOperator @operator) =>
        @operator == DataFilterComparisonOperator.Empty || @operator == DataFilterComparisonOperator.NotEmpty;
}
