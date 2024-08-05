// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.DocViewer.Models;

internal class ApiCodeComment
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "To implement")]
    public string GetSummary(string name)
    {
        return $"DOC: {name}";
    }
}
