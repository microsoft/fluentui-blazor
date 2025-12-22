// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public class FluentInputFileEventArgs : EventArgs
{
    /// <summary>
    /// Gets a list of all files currently in an upload process.
    /// </summary>
    public required IEnumerable<UploadedFileDetails> AllFiles { get; init; }

    /// <summary>
    /// Gets the index of the current file in an upload process.
    /// </summary>
    public required int Index { get; init; }

    /// <summary>
    /// Gets the name of the current file in an upload process.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the content type of the current file in an upload process.
    /// </summary>
    public required string ContentType { get; init; }

    /// <summary>
    /// Gets the size (in bytes) of the current file in an upload process.
    /// </summary>
    public required long Size { get; init; }

    /// <summary>
    /// Gets the last modified date of the current file in an upload process.
    /// </summary>
    public required DateTimeOffset LastModified { get; init; }

    /// <summary>
    /// Set this property to True to cancel the current upload file.
    /// </summary>
    public required bool IsCancelled { get; set; }

    /// <summary>
    /// Gets the local file of the current file in an upload process.
    /// Only if Mode = SaveToTemporaryFolder (otherwise, this value is null).
    /// </summary>
    public FileInfo? LocalFile { get; internal set; }

    /// <summary>
    /// Gets a small buffer data of the current file in an upload process.
    /// Only if Mode = Buffer.
    /// </summary>
    public FluentInputFileBuffer Buffer { get; internal set; } = default!;

    /// <summary>
    /// Gets a reference to the current stream in an upload process.
    /// Only if Mode = Stream (otherwise, this value is null).
    /// The OnProgressChange event will not be triggered.
    /// </summary>
    public Stream? Stream { get; internal set; }

    /// <summary>
    /// Gets the label to display in an upload process.
    /// </summary>
    public string? ProgressTitle { get; internal set; }

    /// <summary>
    /// Gets the global percent value in an upload process.
    /// </summary>
    public int ProgressPercent { get; internal set; }

    /// <summary>
    /// Gets the error message (or null if no error occurred).
    /// </summary>
    public string? ErrorMessage { get; internal set; }
}
