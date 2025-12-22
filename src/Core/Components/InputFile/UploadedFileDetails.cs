// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the details of an uploaded file, including its name, size, and content type.
/// </summary>
/// <param name="Name">The file that has been uploaded</param>
/// <param name="Size">The size of the file in bytes</param>
/// <param name="ContentType">MIME content type</param>
public record struct UploadedFileDetails(string Name, long Size, string ContentType);
