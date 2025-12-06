// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.Client;
using FluentUI.Mcp.Shared;
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
    // Set default values for FluentButton component
    // config.DefaultValues.For<FluentButton>().Set(p => p.Appearance, ButtonAppearance.Primary);
    // config.DefaultValues.For<FluentButton>().Set(p => p.Shape, ButtonShape.Circular);

    // Use a custom localizer
    config.Localizer = new FluentUI.Demo.MyLocalizer();
});

// Add Demo server services
builder.Services.AddFluentUIDemoServices().ForServer();

// Initialize MCP capabilities data from the MCP Server assembly
McpCapabilitiesData.Initialize(typeof(FluentUI.Mcp.Server.Services.FluentUIDocumentationService).Assembly);

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

// API endpoint to expose MCP capabilities for WebAssembly clients
app.MapGet("/api/mcp/capabilities", () => McpCapabilitiesData.GetSummary())
    .WithName("GetMcpCapabilities");

app.MapRazorComponents<FluentUI.Demo.Components.App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(FluentUI.Demo.Client._Imports).Assembly);

app.Run();
