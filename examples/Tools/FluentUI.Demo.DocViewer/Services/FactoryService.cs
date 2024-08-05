// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Markdig;

namespace FluentUI.Demo.DocViewer.Services;

internal class FactoryService
{
    public static readonly MarkdownPipeline MarkdownPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

    public required DocViewerService DocViewerService { get; init; }

    public required IStaticAssetService StaticAssetService { get; init; }
}
