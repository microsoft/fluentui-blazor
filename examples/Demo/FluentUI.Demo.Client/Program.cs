// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Demo.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add FluentUI services
builder.Services.AddFluentUIComponents();

// Add Demo server services
builder.Services.AddFluentUIDemoServices().ForClient();

await builder.Build().RunAsync();
