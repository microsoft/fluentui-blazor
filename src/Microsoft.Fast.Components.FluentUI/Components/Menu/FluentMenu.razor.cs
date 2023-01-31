using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentMenu
{
    private readonly Dictionary<string, FluentMenuItem> items = new();

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(MenuChangeEventArgs))]
    public FluentMenu()
    {
        
    }

    internal void Register(FluentMenuItem item)
    {
        items.Add(item.Id, item);
    }

    internal void Unregister(FluentMenuItem item)
    {
        items.Remove(item.Id);
    }
}