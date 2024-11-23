// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Globalization;
using FluentUI.Demo.Client;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddHttpClient();

// Add localization services
builder.Services.AddLocalization();

// Add FluentUI services
builder.Services.AddFluentUIComponents(config =>
{
    config.Localizer = new MyLocalizer();
});

// Add Demo server services
builder.Services.AddFluentUIDemoServices().ForServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapStaticAssets();

app.UseAntiforgery();

// Use the localization services
app.UseRequestLocalization(new RequestLocalizationOptions().AddSupportedUICultures(["en", "fr"]));

app.MapRazorComponents<FluentUI.Demo.Components.App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(FluentUI.Demo.Client._Imports).Assembly);

app.Run();

internal class MyLocalizer : IFluentLocalizer
{
    public string this[string key, params object[] arguments]
    {
        get
        {
            // Need to add
            //  - builder.Services.AddLocalization();
            //  - app.UseRequestLocalization(new RequestLocalizationOptions().AddSupportedUICultures(["fr"]));
            var language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            Console.WriteLine(language);

            // Returns the French version of the string
            if (language == "fr")
            {
                return key switch
                {
                    "FluentSample_Hello" => "Bonjour",
                    _ => IFluentLocalizer.GetDefault(key, arguments),
                };
            }

            // By default, returns the English version of the string
            return IFluentLocalizer.GetDefault(key, arguments);
        }
    }
}
