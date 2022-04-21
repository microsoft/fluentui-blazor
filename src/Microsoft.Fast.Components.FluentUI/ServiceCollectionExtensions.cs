using Microsoft.Extensions.DependencyInjection;
using Microsoft.Fast.Components.FluentUI.DesignTokens;

namespace Microsoft.Fast.Components.FluentUI;

public static class ServiceCollectionExtensions
{
    public static void AddFluentUIComponents(this IServiceCollection services)
    {
        services.AddScoped<IconService>();
        services.AddDesignTokens();
    }
}