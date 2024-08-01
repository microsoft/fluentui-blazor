// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Reflection;
using FluentUI.Demo.DocViewer.Models;

namespace FluentUI.Demo.DocViewer.Services;

public class DocViewerService
{
    private IEnumerable<Page>? _pages;

    public required string PageTitle { get; init; }

    /// <summary>
    /// Gets the assembly containing the razor components to display in markdown pages.
    /// </summary>
    public required Assembly ComponentsAssembly { get; init; }

    /// <summary>
    /// Gets the assembly containing the embedded markdown pages.
    /// </summary>
    public required Assembly ResourcesAssembly { get; init; }

    /// <summary>
    /// Gets the list of all markdown pages found in the resources
    /// </summary>
    public IEnumerable<Page> Pages => _pages ??= LoadAllPages();

    /// <summary>
    /// Returns the <see cref="Page" /> associated to the <paramref name="routeName"/>.
    /// Or null if not found.
    /// </summary>
    /// <param name="routeName"></param>
    /// <returns></returns>
    public Page? FromRoute(string routeName)
    {
        return Pages.FirstOrDefault(i => string.Compare(i.Route, routeName, StringComparison.InvariantCultureIgnoreCase) == 0 ||
                                         string.Compare(i.Route, $"/{routeName}", StringComparison.InvariantCultureIgnoreCase) == 0);
    }

    private List<Page> LoadAllPages()
    {
        var resourceNames = ResourcesAssembly.GetManifestResourceNames();
        var pages = new List<Page>();

        foreach (var resourceName in resourceNames)
        {
            if (resourceName.EndsWith(".md"))
            {
                using var stream = ResourcesAssembly.GetManifestResourceStream(resourceName);
                if (stream != null)
                {
                    using var reader = new StreamReader(stream);
                    var content = reader.ReadToEnd();
                    pages.Add(new Page(content));
                }
            }
        }

        return pages;
    }
}
