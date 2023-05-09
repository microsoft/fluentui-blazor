using FluentUI.Demo.Shared;
using FluentUI.Demo.Shared.SampleData;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Fast.Components.FluentUI;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddFluentUIComponents(options =>
{
    options.HostingModel = BlazorHostingModel.WebAssembly;
    options.IconConfiguration = ConfigurationGenerator.GetIconConfiguration();
    options.EmojiConfiguration = ConfigurationGenerator.GetEmojiConfiguration();
});

builder.Services.AddScoped<DataSource>();

await builder.Build().RunAsync();