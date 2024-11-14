// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.WebServer;

public class LocalWebApplicationFactory : WebApplicationFactory<Startup>
{
    private const int PORT = 5050;
    private IWebHostBuilder? _host;

    public string ServerAddress
    {
        get
        {
            EnsureServer();
            return $"http://localhost:{PORT}";
        }
    }

    protected override IWebHostBuilder? CreateWebHostBuilder()
    {
        var directory = Path.GetFullPath(@"C:\VSO\Perso\fluentui-blazor-v5\tests\Integration\bin\publish");

        var host = new WebHostBuilder()
            .PreferHostingUrls(true)
            .UseWebRoot(directory)
            .UseContentRoot(directory)
            .UseEnvironment("Development")
            .UseKestrel(options =>
            {
                options.ListenLocalhost(PORT);
            })
            .UseStartup<Startup>();

        return host;
    }

    private void EnsureServer()
    {
        if (_host is null)
        {
            // This forces WebApplicationFactory to bootstrap the server
            _host = CreateWebHostBuilder();
            _host?.Start();
        }
    }
}
