using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using System.Reflection;
using System;

namespace FluentUI.Demo.Shared.Pages.Index;
public partial class Index
{
    private string? _version;

    protected override void OnInitialized()
    {
        _version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;

    }
}
