// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.DocViewer.Services;

/// <summary>
/// Service to load static assets.
/// </summary>
public interface IStaticAssetService
{
    /// <summary>
    /// Load the asset from the given <paramref name="assetUrl"/>.
    /// </summary>
    /// <param name="assetUrl"></param>
    /// <returns></returns>
    Task<string?> GetAsync(string assetUrl);
}