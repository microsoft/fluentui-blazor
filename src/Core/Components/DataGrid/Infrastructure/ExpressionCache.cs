// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Linq.Expressions;


namespace Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

internal static class ExpressionCache<TGridItem, T>
{
    private static readonly ConcurrentDictionary<Expression<Func<TGridItem, T>>, Func<TGridItem, T>> Cache =
        new();

    public static Func<TGridItem, T>? CachedCompile(Expression<Func<TGridItem, T>> targetSelector)
    {
        if (targetSelector == null)
        {
            return null;
        }

        return Cache.GetOrAdd(targetSelector, key => key.Compile());
    }
}
