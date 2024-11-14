// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.WebServer;

public class Startup
{
    private readonly IWebHostEnvironment _environment;

    public Startup(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorComponents()
                .AddInteractiveServerComponents();

        services.AddHttpClient();
        services.AddCors();
        services.AddRouting();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseCors(policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
        app.UseHttpsRedirection();
        app.UseStatusCodePages("text/html", "<html><head><title>Error {0}</title></head><body><h1>HTTP {0}</h1></body></html>");
        app.UseDeveloperExceptionPage();

        app.UseRouting();
        app.UseAntiforgery();
        app.UseStaticFiles();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorComponents<Components.App>()
                     .AddInteractiveServerRenderMode();
        });
    }
}
