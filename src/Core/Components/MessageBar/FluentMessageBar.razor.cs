// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Component to communicate important information about the state of the entire application or surface
/// </summary>
public partial class FluentMessageBar : FluentComponentBase
{
    private static readonly Icon IconError = new CoreIcons.Regular.Size20.QuestionCircle();

    /// <summary />
    public FluentMessageBar(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary>
    /// Gets or sets the intent of the message bar. 
    /// Default is <see cref="MessageIntent.Info"/>.
    /// </summary>
    [Parameter]
    public MessageIntent? Intent { get; set; }

    /// <summary>
    /// Gets or sets the icon to show in the message bar based on the intent of the message.
    /// </summary>
    [Parameter]
    public Icon? Icon { get; set; }

    /// <summary>
    /// Gets or sets the message to be shown when not using the MessageService methods.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    private string? GetIntentString()
    {
        if (Intent == null || Intent == MessageIntent.Custom)
        {
            return null;
        }

        return Intent.ToAttributeValue();
    }

    private Icon? GetIcon()
    {
        if (Icon is null)
        {
            return Intent switch
            {
                MessageIntent.Error => IconError,
                MessageIntent.Warning => IconWarning,
                MessageIntent.Success => IconCheckmarkCircle,
                MessageIntent.Info => IconInfo,
                _ => null
            };
        }
    }
}
