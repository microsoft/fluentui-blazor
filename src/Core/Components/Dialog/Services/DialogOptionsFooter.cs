// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Options for configuring a dialog footer.
/// </summary>
public class DialogOptionsFooter
{
    /// <summary />
    public DialogOptionsFooter()
    {

    }

    /// <summary>
    /// Gets or sets whether the primary action is displayed first.
    /// </summary>
    public bool IsPrimaryFirst { get; set; } = true;

    /// <summary>
    /// Gets or sets the primary action for the footer.
    /// </summary>
    public DialogOptionsFooterAction PrimaryAction { get; } = new(ButtonAppearance.Primary);

    /// <summary>
    /// Gets or sets the secondary action for the footer.
    /// </summary>
    public DialogOptionsFooterAction SecondaryAction { get; } = new(ButtonAppearance.Default);

    /// <summary />
    internal IEnumerable<DialogOptionsFooterAction> GetActions()
        => IsPrimaryFirst ? [PrimaryAction, SecondaryAction] : [SecondaryAction, PrimaryAction];

    /// <summary />
    internal bool HasActions => PrimaryAction.ToDisplay || SecondaryAction.ToDisplay;
}
