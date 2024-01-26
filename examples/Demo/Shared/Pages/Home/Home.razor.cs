using System;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Pages.Home;
public partial class Home
{
    private string? _version;

    protected override void OnInitialized()
    {
        _version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;

    }
}
