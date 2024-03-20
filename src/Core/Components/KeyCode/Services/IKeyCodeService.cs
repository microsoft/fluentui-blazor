// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

public interface IKeyCodeService
{
    IEnumerable<IKeyCodeListener> Listeners { get; }

    Guid RegisterListener(IKeyCodeListener listener);

    Guid RegisterListener(Func<FluentKeyCodeEventArgs, Task> handler);

    void UnregisterListener(IKeyCodeListener listener);

    void Clear();
}
