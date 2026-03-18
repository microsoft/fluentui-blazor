// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

#pragma warning disable IL2111

using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentToastProvider : FluentComponentBase
{
    /// <summary />
    public FluentToastProvider(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-toast-provider")
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
    protected virtual IToastService? ToastService => GetCachedServiceOrNull<IToastService>();

    /// <summary />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (ToastService is not null)
        {
            ToastService.ProviderId = Id;
            ToastService.OnUpdatedAsync = async (item) =>
            {
                await InvokeAsync(StateHasChanged);
            };
        }
    }

    /// <summary />
    private static Action<ToastEventArgs> EmptyOnStateChange => (_) => { };

    private static Type GetToastType(IToastInstance toast)
    {
        if (Equals(toast.ComponentType, typeof(FluentToast)) || Equals(toast.ComponentType, typeof(FluentProgressToast)))
        {
            return toast.ComponentType;
        }

        throw new InvalidOperationException($"Unsupported toast component type '{toast.ComponentType.FullName}'. Only {nameof(FluentToast)} and {nameof(FluentProgressToast)} are supported.");
    }

    private Dictionary<string, object?> GetToastParameters(IToastInstance toast)
    {
        var toastOptions = toast.Options;
        var parameters = new Dictionary<string, object?>(StringComparer.Ordinal)
        {
            [nameof(FluentToastComponentBase.Id)] = toast.Id,
            [nameof(FluentToastComponentBase.Class)] = toastOptions?.ClassValue,
            [nameof(FluentToastComponentBase.Style)] = toastOptions?.StyleValue,
            [nameof(FluentToastComponentBase.Data)] = toastOptions?.Data,
            [nameof(FluentToastComponentBase.Timeout)] = toastOptions?.Timeout ?? 5000,
            [nameof(FluentToastComponentBase.Instance)] = toast,
            [nameof(FluentToastComponentBase.OnStateChange)] = EventCallback.Factory.Create<ToastEventArgs>(this, toastOptions?.OnStateChange ?? EmptyOnStateChange),
            [nameof(FluentToastComponentBase.AdditionalAttributes)] = toastOptions?.AdditionalAttributes,
        };

        if (toastOptions is null || toastOptions.Parameters is null)
        {
            return parameters;
        }

        foreach (var parameter in toastOptions.Parameters)
        {
            if (string.Equals(parameter.Key, nameof(FluentToastComponentBase.Instance), StringComparison.Ordinal))
            {
                continue;
            }

            parameters[parameter.Key] = parameter.Value;
        }

        return parameters;
    }

    /// <summary>
    /// Only for Unit Tests
    /// </summary>
    /// <param name="id"></param>
    internal void UpdateId(string? id)
    {
        Id = id;

        if (ToastService is not null)
        {
            ToastService.ProviderId = id;
        }
    }
}

#pragma warning restore IL2111
