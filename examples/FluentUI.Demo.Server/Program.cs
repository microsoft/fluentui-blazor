using System.Net.Http.Headers;
using System.Net;
using FluentUI.Demo.Shared.SampleData;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddHttpClient();
builder.Services.AddServerSideBlazor();

builder.Services.AddFluentUIComponents(options => 
{
    options.HostingModel = BlazorHostingModel.Server;
    options.IconConfiguration = ConfigurationGenerator.GetIconConfiguration();
    options.EmojiConfiguration = ConfigurationGenerator.GetEmojiConfiguration();
});

builder.Services.AddScoped<DataSource>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();


app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
