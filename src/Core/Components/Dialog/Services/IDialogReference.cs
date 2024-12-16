// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Interface for DialogReference
/// </summary>
public interface IDialogReference
{
    /// <summary />
    Task<DialogResult> Result { get; }

    /// <summary />
    DialogInstance Instance { get; set; }

    /// <summary />
    Task CloseAsync();

    /// <summary />
    Task CloseAsync(DialogResult result);
}
