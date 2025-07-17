#if (IndividualLocalAuth)
using BlazorWeb_CSharp.Client;
using Microsoft.AspNetCore.Components.Authorization;
#endif
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

#if (IndividualLocalAuth)
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

#endif
await builder.Build().RunAsync();
