// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Globalization;
using System.Text;

namespace FluentUI.Demo.DocViewer.Components.ConsoleLog;

/// <summary />
public class ConsoleLogService : TextWriter
{
    private readonly TextWriter _originalConsoleOut;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleLogService"/> class.
    /// </summary>
    public ConsoleLogService()
    {
        _originalConsoleOut = Console.Out;
        Console.SetOut(this);
    }

    /// <summary>
    /// Gets the list of messages logged.
    /// </summary>
    public List<LogItem> Traces { get; } = [];

    /// <summary>
    /// Gets or sets the message logged event.
    /// </summary>
    public Action<LogItem> OnTraceLogged { get; set; } = _ => { };

    /// <summary>
    /// Gets the encoding.
    /// </summary>
    public override Encoding Encoding => _originalConsoleOut.Encoding;

    /// <summary>
    /// Clears the list of messages logged.
    /// </summary>
    public void Clear()
    {
        Traces.Clear();
        OnTraceLogged.Invoke(new LogItem(string.Empty));
    }

    /// <summary>
    /// Captures the original console output and logs the message: to the original console output and to the <see cref="Traces"/> list.
    /// </summary>
    /// <param name="value"></param>
    public override void WriteLine(string? value)
    {
        _originalConsoleOut.WriteLine(value);
        OnTraceLoggedHandler(value);
    }

    /// <summary>
    /// Captures the original console output and logs the message: to the original console output and to the <see cref="Traces"/> list.
    /// </summary>
    /// <param name="value"></param>
    public override void Write(char value)
    {
        _originalConsoleOut.Write(value);
        OnTraceLoggedHandler(Convert.ToString(value, CultureInfo.InvariantCulture));
    }
    /// <summary>
    /// Captures the original console output and logs the message: to the original console output and to the <see cref="Traces"/> list.
    /// </summary>
    /// <param name="value"></param>
    public override void Write(string? value)
    {
        _originalConsoleOut.Write(value);
        OnTraceLoggedHandler(value);
    }

    /// <summary>
    /// Raises the <see cref="OnTraceLogged"/> event.
    /// </summary>
    /// <param name="message"></param>
    protected virtual void OnTraceLoggedHandler(string? message)
    {
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        var trace = new LogItem(message);
        Traces.Add(trace);
        OnTraceLogged.Invoke(trace);
    }
}
