// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
internal interface IInternalListContextOptions
{
    /// <summary>
    /// Adds an option to the <see cref="FluentListBase{TOption}"/>
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    string? AddOption(FluentOption option);

    /// <summary>
    /// Removes an option to the <see cref="FluentListBase{TOption}"/>
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    string? RemoveOption(FluentOption option);
}
