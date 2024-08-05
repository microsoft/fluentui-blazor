// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.DocViewer.Services;

internal class FactoryService
{
    public required DocViewerService DocViewerService { get; init; }

    public required IStaticAssetService StaticAssetService { get; init; }
}
