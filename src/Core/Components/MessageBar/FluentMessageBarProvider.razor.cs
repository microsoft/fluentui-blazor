// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Container component that renders all message bars registered with the <see cref="IMessageBarService"/>.
/// </summary>
public partial class FluentMessageBarProvider : FluentComponentBase, IDisposable
{
    /// <summary />
    public FluentMessageBarProvider(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-message-bar-provider")
        .Build();

    /// <summary />
    internal string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary>
    /// Gets or sets the injected service provider.
    /// </summary>
    [Inject]
    public IServiceProvider? ServiceProvider { get; set; }

    /// <summary>
    /// Gets or sets the section identifier for the message bar provider.
    /// This is used to scope the message bars to a specific section of the page: 
    /// only message bars with the same section identifier will be rendered in this provider.
    /// </summary>
    [Parameter, EditorRequired]
    public required string Section { get; set; }

    /// <summary />
    protected virtual IMessageBarService? MessageBarService => GetCachedServiceOrNull<IMessageBarService>();

    /// <summary />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (MessageBarService is MessageBarService service)
        {
            // Register this provider as a subscriber. Multiple providers can coexist:
            // each one is notified and decides (via Section) which messages to render.
            service.Subscribe(Id, _ => InvokeAsync(StateHasChanged));
        }
    }

    /// <summary />
    public void Dispose()
    {
        if (MessageBarService is MessageBarService service && !string.IsNullOrEmpty(Id))
        {
            service.Unsubscribe(Id);
        }
    }

    /// <summary />
    private IEnumerable<IMessageBarInstance> GetRenderedMessageBars()
        => MessageBarService?.Items.Values
            .Where(messageBar => string.Compare(messageBar.Options.Section, Section, StringComparison.OrdinalIgnoreCase) == 0 &&
                                 messageBar.LifecycleStatus == MessageBarLifecycleStatus.Visible)
            .OrderBy(messageBar => messageBar.Index)
            ?? Enumerable.Empty<IMessageBarInstance>();
}
