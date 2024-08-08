// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Client;

public static class MarkupStringExtensions
{
    /// <summary>
    /// Returns a MarkupString that represents a list of objects.
    /// </summary>
    /// <param name="list">List of items</param>
    /// <param name="tagName">HTML Tag Name</param>
    /// <returns></returns>
    public static MarkupString ToMarkupList(this IEnumerable<object> list, string? tagName = null)
    {
        return new MarkupString(string.Join(Environment.NewLine, list.Select(i => string.IsNullOrEmpty(tagName) ? $"{i}" : $"<{tagName}>{i}</{tagName}>")));
    }
}
