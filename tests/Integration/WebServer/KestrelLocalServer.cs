// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.WebServer;

/// <summary>
/// Simple HTTP Web Server, based on Kestrel.
/// Need to include the following reference in the .csproj file
/// <code>
///   <ItemGroup>
///     <FrameworkReference Include="Microsoft.AspNetCore.App" />
///   </ItemGroup>
/// </code>
/// </summary>
internal class KestrelLocalServer : IAsyncDisposable
{
    private IWebHost? _host;

    public async Task StartAsync(string currentDirectory, int port, CancellationToken? cancellationToken = null)
    {
        _host = await RunAsync(currentDirectory, port, cancellationToken);
    }

    public async Task StopAsync()
    {
        if (_host == null)
        {
            return;
        }

        await _host.StopAsync();
    }

    protected virtual async Task<IWebHost> RunAsync(string currentDirectory, int port, CancellationToken? cancellationToken)
    {
        var directory = Path.GetFullPath(currentDirectory);

        var host = new WebHostBuilder()
            .PreferHostingUrls(true)
            .UseKestrel(options =>
            {
                options.ListenLocalhost(port);
            })
            .UseWebRoot(directory)
            .UseContentRoot(directory)
            .UseEnvironment("Production")
            .SuppressStatusMessages(true)
            .UseStartup<Startup>()
            .Build();

        await host.StartAsync(cancellationToken ?? CancellationToken.None);

        return host;
    }

    public async ValueTask DisposeAsync()
    {
        await StopAsync();
    }
}
