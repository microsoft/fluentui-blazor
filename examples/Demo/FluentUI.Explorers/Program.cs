// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Explorers.Components;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register Fluent UI services
builder.Services.AddFluentUIComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

// Override any default CSP headers with our custom policy
app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        // Remove X-Frame-Options (it conflicts with CSP frame-ancestors)
        context.Response.Headers.Remove("X-Frame-Options");

        // Define allowed frame ancestors
        string[] allowedFrameAncestors =
        [
            "'self'",
            "https://localhost:7026",
            "https://localhost:7062",
            "https://fluentui-blazor.net",
            "https://www.fluentui-blazor.net",
            "https://preview.fluentui-blazor.net",
            "https://fluentui-explorer-v5.azurewebsites.net"
        ];

        // Set a single CSP header, replacing any that Blazor might have added
        context.Response.Headers["Content-Security-Policy"] =
            $"frame-ancestors {string.Join(" ", allowedFrameAncestors)};";

        return Task.CompletedTask;
    });
    await next();
});

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
