// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Threading.Tasks;
using FluentUI.Demo.DocViewer.Services;

namespace FluentUI.Demo.DocViewer.Tests.Services;

internal class StaticAssetServiceTests : IStaticAssetService
{
    public Func<string, string> GetFunc { get; set; } = (assetUrl) => $"Content of {assetUrl} file.";

    public Task<string?> GetAsync(string assetUrl)
    {
        var content = GetFunc(assetUrl);
        return Task.FromResult<string?>(content);
    }
}
