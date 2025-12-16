// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

//using Microsoft.AspNetCore.Components.Routing;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A container for a <see cref="FluentNav"/> subgroup
/// </summary>
public partial class FluentNavSubItem : FluentNavItem
{
    /// <summary />
    public FluentNavSubItem(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the parent <see cref="FluentNavCategory"/> component for this instance.
    /// </summary>
    /// <remarks>This property is typically set automatically by the Blazor framework when the component is
    /// used within a <see cref="FluentNav"/>. It enables the component to access shared state or functionality from
    /// its parent navigation menu.</remarks>
    [CascadingParameter(Name = "Category")]
    public required FluentNavCategory? Category { get; set; }

    /// <summary />
    protected new string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-navsubitem")
        .AddClass("disabled", Disabled)
        .Build();

    /// <summary>
    /// Validates that this component is used within a FluentNav.
    /// </summary>
    protected override void OnParametersSet()
    {
        // Validate that this component is used within a FluentNavCategory inside a FluentNav
        if (Owner == null || Category is null || Category.GetType() != typeof(FluentNavCategory))
        {
            throw new InvalidOperationException(
                $"{nameof(FluentNavSubItem)} must be used as a child of {nameof(FluentNavCategory)} within a {nameof(FluentNav)}.");
        }

        base.OnParametersSet();
    }

    /// <summary />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        Category?.RegisterSubitem(this);
    }

    /// <summary>
    /// Overrides the location changed handler to notify the parent category when this subitem's active state changes.
    /// </summary>
    protected override void OnLocationChanged(object? sender, LocationChangedEventArgs args)
    {
        var shouldBeActiveNow = ShouldMatch(args.Location);
        if (shouldBeActiveNow != _isActive)
        {
            _isActive = shouldBeActiveNow;

            Category?.OnSubitemActiveStateChanged();
        }
    }

    /// <inheritdoc />
    public new void Dispose()
    {
        Category?.UnregisterSubitem(this);
        base.Dispose();
    }
}
