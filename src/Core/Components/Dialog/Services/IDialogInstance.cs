// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Interface for DialogReference
/// </summary>
public interface IDialogInstance
{
    /// <summary />
    string Id { get; }

    /// <summary />
    Task<DialogResult> Result { get; }

    /// <summary />
    Task CloseAsync();

    /// <summary />
    Task CloseAsync(DialogResult result);
}
