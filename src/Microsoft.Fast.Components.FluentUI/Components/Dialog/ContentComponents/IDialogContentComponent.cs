using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// A component implementing this interface can be used as dialog content.
/// </summary>
public interface IDialogContentComponent
{
    /// <summary>
    /// Gets or sets the title dialog.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }


    ///// <summary>
    ///// Dialog instance specific settings
    ///// </summary>
    //[Parameter]
    //public DialogSettings Settings { get; set; }

}
