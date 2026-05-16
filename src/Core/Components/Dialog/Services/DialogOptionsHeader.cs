// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Options for configuring a dialog header.
/// </summary>
public class DialogOptionsHeader
{
    private readonly List<DialogOptionsHeaderAction> _extraActions = [];

    /// <summary />
    public DialogOptionsHeader()
    {
    }

    /// <summary>
    /// Gets or sets the title of the dialog.
    /// For security reasons, the content is sanitized using the configured <see cref="LibraryConfiguration.MarkupSanitized"/> before rendering.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the close action for the header.
    /// When the user clicks the close action, the dialog will be closed with the result of <see cref="DialogResult{TContent}.Cancel()"/>.
    /// </summary>
    public DialogOptionsHeaderAction CloseAction { get; } = new(isClosedAction: true);

    /// <summary>
    /// Gets or sets the info action for the header.
    /// When the user clicks the info action, the dialog will not be closed by default.
    /// You can set the <see cref="DialogOptionsHeaderAction.OnClickAsync"/> property to handle the click event of the info action.
    /// </summary>
    public DialogOptionsHeaderAction InfoAction { get; } = new(isClosedAction: false);

    /// <summary>
    /// Adds an extra action button to the dialog header.
    /// Extra actions are displayed before the <see cref="InfoAction"/> and the <see cref="CloseAction"/>.
    /// </summary>
    /// <param name="options">A delegate used to configure the new <see cref="DialogOptionsHeaderAction"/>.</param>
    /// <returns>The newly created <see cref="DialogOptionsHeaderAction"/>.</returns>
    public DialogOptionsHeaderAction AddAction(Action<DialogOptionsHeaderAction> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var action = new DialogOptionsHeaderAction() { Visible = true };
        options(action);
        _extraActions.Add(action);
        return action;
    }

    /// <summary />
    internal IEnumerable<DialogOptionsHeaderAction> GetActions() => [.. _extraActions, InfoAction, CloseAction];

    /// <summary />
    internal bool HasActions => CloseAction.ToDisplay || InfoAction.ToDisplay || _extraActions.Any(a => a.ToDisplay);
}
