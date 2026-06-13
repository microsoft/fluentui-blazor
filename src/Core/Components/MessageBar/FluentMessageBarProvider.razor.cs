// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Container component that renders all message bars registered with the <see cref="IMessageBarService"/>.
/// </summary>
public partial class FluentMessageBarProvider : FluentComponentBase
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
        .AddStyle("z-index", ZIndex.Toast.ToString(CultureInfo.InvariantCulture))
        .Build();

    /// <summary>
    /// Gets or sets the injected service provider.
    /// </summary>
    [Inject]
    public IServiceProvider? ServiceProvider { get; set; }

    /// <summary />
    protected virtual IMessageBarService? MessageBarService => GetCachedServiceOrNull<IMessageBarService>();

    /// <summary />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (MessageBarService is not null)
        {
            MessageBarService.ProviderId = Id;
            MessageBarService.OnUpdatedAsync = async (_) =>
            {
                await InvokeAsync(StateHasChanged);
            };
        }
    }

    /// <summary />
    private IEnumerable<IMessageBarInstance> GetRenderedMessageBars()
        => MessageBarService?.Items.Values
            .Where(messageBar => messageBar.LifecycleStatus == MessageBarLifecycleStatus.Visible)
            .OrderBy(messageBar => messageBar.Index)
            ?? Enumerable.Empty<IMessageBarInstance>();
}
