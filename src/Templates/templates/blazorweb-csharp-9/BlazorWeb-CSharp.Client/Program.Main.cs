using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
#if (UseWebAssembly)
using Microsoft.FluentUI.AspNetCore.Components;
#endif

namespace BlazorWeb_CSharp.Client;

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
