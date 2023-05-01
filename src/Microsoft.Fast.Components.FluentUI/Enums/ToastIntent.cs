using System.ComponentModel;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public enum ToastIntent
{
    /// <summary>
    /// Neutral (informational )intent
    /// </summary>
    Neutral,

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
    Danger,

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
