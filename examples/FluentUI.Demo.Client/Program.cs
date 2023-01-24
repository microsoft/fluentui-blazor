using FluentUI.Demo.Shared;
using FluentUI.Demo.Shared.SampleData;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Fast.Components.FluentUI;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddFluentUIComponents(configuration =>
{
    configuration!.EmojiConfiguration.Groups = new[]
    {
        EmojiGroup.Activities,
        EmojiGroup.Animals_Nature,
        EmojiGroup.Flags,
        EmojiGroup.Food_Drink,
        EmojiGroup.Objects,
        EmojiGroup.Smileys_Emotion,
        EmojiGroup.Symbols,
        EmojiGroup.Travel_Places,
    };
    configuration!.EmojiConfiguration.Styles = new[]
    {
        EmojiStyle.Color
    };
    configuration!.IconConfiguration.Sizes = new[]
    {
        IconSize.Size10,
        IconSize.Size12,
        IconSize.Size16,
        IconSize.Size20,
        IconSize.Size24,
        IconSize.Size48
    };
    configuration!.IconConfiguration.Variants = new[]
    {
        IconVariant.Regular,
        IconVariant.Filled
    };
});

builder.Services.AddScoped<DataSource>();

await builder.Build().RunAsync();