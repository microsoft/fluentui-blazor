// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

internal interface IInputFileOptions
{
    /// <summary>
    /// To enable multiple file selection and upload, set the Multiple property to true.
    /// Set <see cref="MaximumFileCount"/> to change the number of allowed files.
    /// </summary>
    bool Multiple { get; set; }

    /// <summary>
    /// To select multiple files, set the maximum number of files allowed to be uploaded.
    /// Default value is 10.
    /// </summary>
    int MaximumFileCount { get; set; }

    /// <summary>
    /// Gets or sets the maximum size of a file to be uploaded (in bytes).
    /// Default value is 10 MiB.
    /// </summary>
    long MaximumFileSize { get; set; }

    /// <summary>
    /// Gets or sets the sze of buffer to read bytes from uploaded file (in bytes).
    /// Default value is 10 KiB.
    /// </summary>
    uint BufferSize { get; set; }

    /// <summary>
    /// Gets or sets the filter for what file types the user can pick from the file input dialog box.
    /// Example: ".gif, .jpg, .png, .doc", "audio/*", "video/*", "image/*"
    /// See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/accept">https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/accept</see>
    /// for more information.
    /// </summary>
    string Accept { get; set; }

    /// <summary>
    /// Gets or sets the type of file reading.
    /// For SaveToTemporaryFolder, use <see cref="FluentInputFileEventArgs.LocalFile" /> to retrieve the file.
    /// For Buffer, use <see cref="FluentInputFileEventArgs.Buffer" /> to retrieve bytes.
    /// For Stream, use <see cref="FluentInputFileEventArgs.Stream"/> to have full control over retrieving the file.
    /// </summary>
    InputFileMode Mode { get; set; }
}
