// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Runtime.CompilerServices;
using FluentUI.Demo.DocViewer.Services;

namespace FluentUI.Demo.DocViewer.Tests.Services;

internal class FactoryServiceTests : FactoryService
{
    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public FactoryServiceTests()
    {
        DocViewerService = new DocViewerService(new DocViewerOptions());
        StaticAssetService = new StaticAssetServiceTests();
    }
}
