using Microsoft.Extensions.DependencyInjection;


namespace BcdLib.Components
{
    public static class ServiceExt
    {
        public static IServiceCollection AddBcdLibPullComponent(this IServiceCollection services)
        {
            services.AddScoped<DocumentJsInterop>();
            return services;
        }
    }
}
