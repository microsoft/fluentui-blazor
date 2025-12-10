// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer;
using Microsoft.Extensions.Hosting;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder
    .ConfigureMcpLogging()
    .Services
    .AddFluentUIDocumentation()
    .AddFluentUIMcpServer();

await builder.Build().RunAsync();

