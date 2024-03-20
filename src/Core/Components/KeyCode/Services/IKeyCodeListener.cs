// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

public interface IKeyCodeListener
{
    Task OnKeyDownAsync(FluentKeyCodeEventArgs args);
}
