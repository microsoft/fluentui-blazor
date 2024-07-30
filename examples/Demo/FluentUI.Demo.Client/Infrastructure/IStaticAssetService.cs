// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.Client;

/// <summary />
public interface IStaticAssetService
{
    /// <summary />
    public Task<string?> GetAsync(string assetUrl, bool useCache = true);
}
