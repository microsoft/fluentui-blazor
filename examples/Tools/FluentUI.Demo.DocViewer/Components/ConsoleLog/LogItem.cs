// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.DocViewer.Components.ConsoleLog;

/// <summary>
/// Log item.
/// </summary>
public record LogItem
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LogItem"/> class.
    /// </summary>
    /// <param name="message"></param>
    public LogItem(string message)
    {
        Message = message;
        Recorded = DateTime.Now;
    }

    /// <summary>
    /// Gets the message logged.
    /// </summary>
    public string Message { get; init; }

    /// <summary>
    /// Gets the date and time the message was logged.
    /// </summary>
    public DateTime Recorded { get; init; }
}
