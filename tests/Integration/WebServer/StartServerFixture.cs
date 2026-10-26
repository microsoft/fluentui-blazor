// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.WebServer;

/// <summary>
/// Fixture that starts the web server before the tests and stops it after the tests.
/// </summary>
public class StartServerFixture : IAsyncLifetime
{
    private const int KILL_TIMEOUT_MILLISECONDS = 1000;
    private const int START_TIMEOUT_SECONDS = 30;
    private const int START_POLL_MILLISECONDS = 200;
    private const string PROCESS_FILENAME = "Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.exe";
    private const string PROJECT_FILENAME = "Components.IntegrationTests.csproj";

    // .NET Framework Version - Change this to net8.0, net9.0, or net10.0 to match Directory.Build.props
    private const string NET_VERSION = "net10.0";

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
    public async Task InitializeAsync()
    {
        // Kill the existing server process (if the previous DisposeAsync was not called)
        KillExistingServerProcess();

#if DEBUG
        var mode = "Debug";
#else
        var mode = "Release";
#endif

        // Start the process
        // dotnet run -c Release -f {NET_VERSION} --no-build --urls "http://localhost:5050"
        _serverProcess = new Process();
        _serverProcess.StartInfo.WorkingDirectory = GetProjectFolder();
        _serverProcess.StartInfo.FileName = "dotnet";
        _serverProcess.StartInfo.Arguments = $"run --urls \"{BaseUrl}\" -f {NET_VERSION} -c {mode} --no-build";

        var started = _serverProcess.Start();

        await WaitForServerAsync(_serverProcess);
    }

    /// <summary>
    /// Waits until the server serves the Blazor script, because `dotnet run` returns before the server listens:
    /// without this wait, the first tests fail with ERR_CONNECTION_REFUSED.
    /// </summary>
    /// <param name="serverProcess">The started server process.</param>
    /// <exception cref="InvalidOperationException">The server process exited before the server was ready.</exception>
    /// <exception cref="TimeoutException">The server was not ready within <see cref="START_TIMEOUT_SECONDS"/>.</exception>
    private async Task WaitForServerAsync(Process serverProcess)
    {
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(START_TIMEOUT_SECONDS));
        using var client = new HttpClient();
        Exception? lastError = null;

        try
        {
            while (true)
            {
                if (serverProcess.HasExited)
                {
                    throw new InvalidOperationException($"The server process exited with code {serverProcess.ExitCode} before {BaseUrl} was ready.", lastError);
                }

                try
                {
                    using var response = await client.GetAsync($"{BaseUrl}/_framework/blazor.web.js", timeout.Token);
                    response.EnsureSuccessStatusCode();
                    return;
                }
                catch (HttpRequestException ex)
                {
                    // Not listening yet, or not ready to serve the script
                    lastError = ex;
                }

                await Task.Delay(START_POLL_MILLISECONDS, timeout.Token);
            }
        }
        catch (OperationCanceledException) when (timeout.IsCancellationRequested)
        {
            throw new TimeoutException($"{BaseUrl} was not ready within {START_TIMEOUT_SECONDS} seconds.", lastError);
        }
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
