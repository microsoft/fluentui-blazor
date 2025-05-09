// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents configuration options for handling file input and upload functionality.
/// </summary>
public class InputFileOptions : IInputFileOptions
{
    /// <inheritdoc cref="IInputFileOptions.Multiple" />
    public bool Multiple { get; set; }

    /// <inheritdoc cref="IInputFileOptions.MaximumFileCount" />
    public int MaximumFileCount { get; set; } = 10;

    /// <inheritdoc cref="IInputFileOptions.MaximumFileSize" />
    public long MaximumFileSize { get; set; } = 10 * 1024 * 1024;

    /// <inheritdoc cref="IInputFileOptions.BufferSize" />
    public uint BufferSize { get; set; } = 10 * 1024;

    /// <inheritdoc cref="IInputFileOptions.Accept" />
    public string Accept { get; set; } = string.Empty;

    /// <inheritdoc cref="IInputFileOptions.Mode" />
    public InputFileMode Mode { get; set; }

    /// <summary>
    /// Raise when a file is completely uploaded.
    /// </summary>
    public Func<FluentInputFileEventArgs, Task>? OnFileUploadedAsync { get; set; }

    /// <summary>
    /// Raise when a progression step is updated.
    /// </summary>
    public Func<FluentInputFileEventArgs, Task>? OnProgressChangeAsync { get; set; }

    /// <summary>
    /// Raise when a file raised an error.
    /// </summary>
    public Func<FluentInputFileErrorEventArgs, Task>? OnFileErrorAsync { get; set; }
}
