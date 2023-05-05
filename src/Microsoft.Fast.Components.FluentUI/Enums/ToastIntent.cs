using System.ComponentModel;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public enum ToastIntent
{
    /// <summary>
    /// Neutral intent
    /// </summary>
    Neutral,

    /// <summary>
    /// Informational intent
    /// </summary>
    Info,

    /// <summary>
    /// Indicates a success 
    /// </summary>
    Success,

    /// <summary>
    /// Indicates a warning
    /// </summary>
    Warning,

    /// <summary>
    /// Indicates an error
    /// </summary>
    Error,

    /// <summary>
    /// Indicates progress
    /// </summary>
    Progress,

    /// <summary>
    /// Indicate upload progress
    /// </summary>
    ProgressUpload,

    /// <summary>
    /// Indicates download progress
    /// </summary>
    ProgressDownload,

    /// <summary>
    /// Indicates a mention
    /// </summary>
    Mention
}
