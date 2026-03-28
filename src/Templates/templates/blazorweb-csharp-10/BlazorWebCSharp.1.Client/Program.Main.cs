using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
#if (UseWebAssembly)
using Microsoft.FluentUI.AspNetCore.Components;
#endif

namespace BlazorWebCSharp._1.Client;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

#if (IndividualLocalAuth)
        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddAuthenticationStateDeserialization();
#endif

#if (UseWebAssembly)
        builder.Services.AddFluentUIComponents();
#endif

        await builder.Build().RunAsync();
    }
}
