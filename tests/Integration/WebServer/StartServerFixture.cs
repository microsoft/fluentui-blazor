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

/// <summary>
/// Fixture that starts the web server before the tests and stops it after the tests.
/// </summary>
public class StartServerFixture : IAsyncLifetime
{
    private const int KILL_TIMEOUT_MILLISECONDS = 1000;
    private const string PROCESS_FILENAME = "Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.exe";
    private const string PROJECT_FILENAME = "Components.IntegrationTests.csproj";

    private Process? _serverProcess;

    /// <summary>
    /// Gets the server URL.
    /// </summary>
    public string BaseUrl => "http://localhost:5050";

    /// <summary>
    /// Gets the folder where the screenshots are saved.
    /// </summary>
    public string ScreenshotsFolder => @"C:\Temp\FluentUI\Screenshots\";

    /// <summary>
    /// Starts the server process.
    /// </summary>
    /// <returns></returns>
    public Task InitializeAsync()
    {
        // Kill the existing server process (if the previous DisposeAsync was not called)
        KillExistingServerProcess();

#if DEBUG 
        var mode = "Debug";
#else
        var mode = "Release";
#endif

        // Start the process
        // dotnet run -c Release -f net9.0 --no-build --urls "http://localhost:5050"
        _serverProcess = new Process();
        _serverProcess.StartInfo.WorkingDirectory = GetProjectFolder();
        _serverProcess.StartInfo.FileName = "dotnet";
        _serverProcess.StartInfo.Arguments = $"run --urls \"{BaseUrl}\" -f net9.0 -c {mode} --no-build";

        var started = _serverProcess.Start();

        return Task.CompletedTask;
    }

    /// <summary>
    /// Stops the server process.
    /// </summary>
    /// <returns></returns>
    public Task DisposeAsync()
    {
        // Kill the process
        if (_serverProcess != null && !_serverProcess.HasExited)
        {
            _serverProcess.Kill();
            _serverProcess.WaitForExit(KILL_TIMEOUT_MILLISECONDS);
        }

        return Task.CompletedTask;
    }

    private void KillExistingServerProcess()
    {
        // Find the process with the specified file path
        var targetProcess = Process.GetProcesses().FirstOrDefault(p =>
        {
            try
            {
                return p.MainModule?.FileName.Equals(PROCESS_FILENAME, StringComparison.OrdinalIgnoreCase) ?? false;
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
            targetProcess.WaitForExit(KILL_TIMEOUT_MILLISECONDS);
        }
    }

    private string GetProjectFolder()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory != null && !directory.GetFiles(PROJECT_FILENAME).Any())
        {
            directory = directory.Parent;
        }

        return directory?.FullName ?? throw new FileNotFoundException($"Project '{PROJECT_FILENAME}' file not found.");
    }
}
