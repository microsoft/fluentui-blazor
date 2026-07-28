// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocViewer.Services;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.DocViewer.Components;

/// <summary>
/// Resolves a component from its name and renders it through a <see cref="DynamicComponent"/>.
/// </summary>
/// <remarks>
/// A render mode cannot be applied directly to a <see cref="DynamicComponent"/> because its
/// <see cref="DynamicComponent.Type"/> parameter (a <see cref="Type"/>) is not serializable.
/// This wrapper accepts a serializable component name instead, so the render mode can be applied
/// to it and the <see cref="Type"/> is resolved inside the interactive boundary.
/// </remarks>
public partial class SampleContainer
{
    private Type? _componentType;

    /// <summary />
    [Inject]
    internal DocViewerService DocViewerService { get; set; } = default!;

    /// <summary>
    /// Gets or sets the name of the component to render.
    /// </summary>
    [Parameter]
    public required string ComponentName { get; set; }

    /// <summary />
    protected override void OnParametersSet()
    {
        _componentType = DocViewerService.ComponentsAssembly?
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(ComponentBase)) && !t.IsAbstract)
            .FirstOrDefault(i => i.Name == ComponentName);
    }
}
