using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentMenu
{
    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(MenuChangeEventArgs))]
    public FluentMenu()
    {
        
    }
}