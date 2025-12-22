// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public class FluentInputFileErrorEventArgs : EventArgs
{
    /// <summary />
    public static readonly (string Code, string Message) MaximumSizeReached = ("MaximumSizeReached", "The maximum size allowed is reached. The `FileName` property specifies the concerned file.");

    /// <summary />
    public static readonly (string Code, string Message) FileCountExceeded = ("FileCountExceeded", "The maximum number of files has been exceeded. The `FileCount` property specifies the total number of files that were attempted for upload.");

    /// <summary />
    internal FluentInputFileErrorEventArgs(string code, string message, int? fileCount = null, string? fileName = null)
    {
        ErrorCode = code;
        ErrorMessage = message;
        FileCount = fileCount;
        FileName = fileName;
    }

    /// <summary>
    /// Gets the error code.
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public string ErrorMessage { get; }

    /// <summary>
    /// Gets the file concerned by the error.
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// Gets the number of files currently being up.
    /// </summary>
    public int? FileCount { get; }
}
