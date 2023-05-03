using FluentUI.Demo.Shared;
using FluentUI.Demo.Shared.SampleData;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Fast.Components.FluentUI;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

LibraryConfiguration config = new(ConfigurationGenerator.GetIconConfiguration(), ConfigurationGenerator.GetEmojiConfiguration());

builder.Services.AddFluentUIComponents(config);

//builder.Services.AddFluentUIComponents(config =>
//{
//    if (config is not null)
//    {
//        config.IconConfiguration = ConfigurationGenerator.GetIconConfiguration();
//        config.EmojiConfiguration = ConfigurationGenerator.GetEmojiConfiguration();

//        config.ToastConfiguration.ToasterPosition = ToasterPosition.BottomLeft;
//        config.ToastConfiguration.NewestOnTop = false;
//        config.ToastConfiguration.ShowCloseIcon = false;
//        config.ToastConfiguration.VisibleStateDuration = 1000;
//        config.ToastConfiguration.HideTransitionDuration = 100;
//        config.ToastConfiguration.ShowTransitionDuration = 100;
//    }
//});

builder.Services.AddScoped<DataSource>();

await builder.Build().RunAsync();