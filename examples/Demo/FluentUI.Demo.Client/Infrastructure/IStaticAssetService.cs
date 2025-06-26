// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.Client;

/// <summary />
public interface IStaticAssetService
{
    /// <summary />
    public Task<string?> GetAsync(string assetUrl, bool useCache = true);
}
