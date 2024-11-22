// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// 
/// </summary>
public interface IFluentLocalizer
{

    /// <summary>
    /// Returns the localized string for the given key.
    /// By default, the English (InvariantCulture) resource is used.</summary>
    /// <param name="key"></param>
    /// <param name="arguments"></param>
    /// <returns></returns>
    string? this[string key, params object[] arguments] { get; }
}
