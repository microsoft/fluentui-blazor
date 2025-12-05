// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Mcp.Server;
using Microsoft.Extensions.Hosting;
using FluentUI.Mcp.Server.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder
    .ConfigureMcpLogging()
    .Services
    .AddFluentUIDocumentation()
    .AddFluentUIMcpServer();

await builder.Build().RunAsync();

