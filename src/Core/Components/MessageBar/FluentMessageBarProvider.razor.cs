// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Container component that renders all message bars registered with the <see cref="INotificationService"/>.
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
    /// Gets or sets the section identifier for the message bar provider.
    /// This is used to scope the message bars to a specific section of the page: 
    /// only message bars with the same section identifier will be rendered in this provider.
    /// </summary>
    [Parameter, EditorRequired]
    public required string Section { get; set; }

    /// <summary />
    protected virtual INotificationService? NotificationService => GetCachedServiceOrNull<INotificationService>();

    /// <summary />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (NotificationService is NotificationService service)
        {
            // Register this provider as a subscriber. Multiple providers can coexist:
            // each one is notified and decides (via Section) which messages to render.
            service.Subscribe(Id, _ => InvokeAsync(StateHasChanged));
        }
    }

    /// <summary />
    public void Dispose()
    {
        if (NotificationService is NotificationService service && !string.IsNullOrEmpty(Id))
        {
            service.Unsubscribe(Id);
        }
    }

    /// <summary />
    private IEnumerable<IMessageBarInstance> MessageBarItems 
        => NotificationService?.Items.Values
                               .Where(item => item is IMessageBarInstance)
                               .Cast<IMessageBarInstance>()
        ?? [];

    /// <summary />
    private IEnumerable<IMessageBarInstance> GetRenderedMessageBars()
        => MessageBarItems.Where(messageBar => string.Compare(messageBar.Options.Section, Section, StringComparison.OrdinalIgnoreCase) == 0 &&
                                 messageBar.LifecycleStatus == MessageBarLifecycleStatus.Visible)
            .OrderBy(messageBar => messageBar.Index);

    /// <summary />
    private RenderFragment RenderMessageBarContent(IMessageBarInstance? messageBar) => builder =>
    {
        if (messageBar is null || string.IsNullOrEmpty(messageBar.Options.Message))
        {
            return;
        }

        builder.AddContent(0, new MarkupStringSanitized(messageBar.Options.Message, MarkupStringSanitized.Formats.Html, LibraryConfiguration));
    };
}
