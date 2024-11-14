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
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.WebServer;

internal class Startup
{
    private readonly IWebHostEnvironment _environment;

    public Startup(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        const bool UseGzip = true;
        const bool UseBrotli = true;

        services.AddRazorComponents()
                .AddInteractiveServerComponents();

        services.AddHttpClient();
        services.AddCors();
        services.AddRouting();
        services.AddResponseCompression(options =>
        {
            options.Providers.Clear();

            if (UseGzip == true || UseBrotli == true)
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes
                    .Append("application/dll")
                    .Append("application/dat");
            }

            if (UseBrotli == true)
            {
                var brotliCompressionProvider = new BrotliCompressionProvider(new BrotliCompressionProviderOptions
                {
                    Level = System.IO.Compression.CompressionLevel.Fastest
                });

                options.Providers.Add(brotliCompressionProvider);
            }

            if (UseGzip == true)
            {
                var gzipCompressionProvider = new GzipCompressionProvider(new GzipCompressionProviderOptions
                {
                    Level = System.IO.Compression.CompressionLevel.Fastest
                });

                options.Providers.Add(gzipCompressionProvider);
            }
        });
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
        //app.UseMiddleware<KestrelThrottlingMiddleware>();
        app.UseDeveloperExceptionPage();

        app.UseRouting();
        app.UseAntiforgery();
        app.UseStaticFiles();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorComponents<Components.App>()
                     .AddInteractiveServerRenderMode();
        });

        ConfigureFileServer(app);
    }

    private void ConfigureFileServer(IApplicationBuilder app)
    {
        var contentTypeProvider = new FileExtensionContentTypeProvider();

        contentTypeProvider.Mappings.Add(".dll", "application/dll");
        contentTypeProvider.Mappings.Add(".dat", "application/dat");

        app.UseResponseCompression();

        app.UseFileServer(new FileServerOptions
        {
            EnableDefaultFiles = true,
            EnableDirectoryBrowsing = true,
            StaticFileOptions =
                {
                    ServeUnknownFileTypes = true,
                    ContentTypeProvider = contentTypeProvider
                },
        });
    }
}
