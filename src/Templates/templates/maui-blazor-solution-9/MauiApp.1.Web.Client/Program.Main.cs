#if (IndividualLocalAuth)
using MauiApp._1.Web.Client;
using Microsoft.AspNetCore.Components.Authorization;
#endif
#if (SampleContent)
using MauiApp._1.Shared.Services;
using MauiApp._1.Web.Client.Services;
#endif

namespace MauiApp._1.Web.Client;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.Services.AddFluentUIComponents();

#if (SampleContent)
        // Add device-specific services used by the MauiApp._1.Shared project
        builder.Services.AddSingleton<IFormFactor, FormFactor>();
#endif
#if (IndividualLocalAuth)
        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddAuthenticationStateDeserialization();

#endif
        await builder.Build().RunAsync();
    }
}
