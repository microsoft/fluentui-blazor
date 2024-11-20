// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.WebServer;

public class StartServerFixture : IAsyncLifetime
{
    private const string ExeFileName = @"C:\VSO\Perso\fluentui-blazor-v5\tests\Integration\bin\Debug\net9.0\Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.exe";

    private Process? ServerProcess { get; set; }

    public Task InitializeAsync()
    {
        // Find the process with the specified file path
        var targetProcess = Process.GetProcesses().FirstOrDefault(p =>
        {
            try
            {
                return p.MainModule?.FileName.Equals(ExeFileName, StringComparison.OrdinalIgnoreCase) ?? false;
            }
            catch (Exception)
            {
                // Some processes may not allow access to their MainModule
                return false;
            }
        });

        // Kill the process if it is running
        if (targetProcess != null)
        {
            targetProcess.Kill();
            targetProcess.WaitForExit();
        }

        // Start the process
        ServerProcess = new Process();
        ServerProcess.StartInfo.FileName = ExeFileName;
        ServerProcess.StartInfo.UseShellExecute = false;
        ServerProcess.StartInfo.CreateNoWindow = false;
        var started = ServerProcess.Start();

        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        // Kill the process
        if (ServerProcess != null && !ServerProcess.HasExited)
        {
            ServerProcess.Kill();
            ServerProcess.WaitForExit(1000);
        }

        return Task.CompletedTask;
    }
}
