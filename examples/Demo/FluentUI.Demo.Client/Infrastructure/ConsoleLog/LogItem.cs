// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.Client.Infrastructure.ConsoleLog;

public record LogItem
{
    public LogItem(string message)
    {
        Message = message;
        Recorded = DateTime.Now;
    }

    public string Message { get; init; }

    public DateTime Recorded { get; init; }
}
