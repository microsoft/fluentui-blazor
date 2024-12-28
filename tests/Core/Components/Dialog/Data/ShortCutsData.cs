// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Collections;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Dialog.Data;

public class ShortcutsData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // Primary Action
        yield return CreateDataItem(expectedCancelled: false,
                                    pressed: ToPress("Enter"));

        yield return CreateDataItem(expectedCancelled: false,
                                    primaryShortcut: "Enter",
                                    pressed: ToPress("Enter"));

        yield return CreateDataItem(expectedCancelled: false,
                                    primaryShortcut: "Enter;Ctrl+Enter",
                                    pressed: ToPress("Enter", ctrlKey: true));

        yield return CreateDataItem(expectedCancelled: false,
                                    primaryShortcut: "Ctrl+Alt+Shift+Enter",
                                    pressed: ToPress("Enter", ctrlKey: true, shiftKey: true, altKey: true));

        // Secondary Action
        yield return CreateDataItem(expectedCancelled: true,
                                    pressed: ToPress("Escape"));

        yield return CreateDataItem(expectedCancelled: true,
                                    secondaryShortcut: "Escape",
                                    pressed: ToPress("Escape"));

        yield return CreateDataItem(expectedCancelled: true,
                                    secondaryShortcut: "Escape;Ctrl+Escape",
                                    pressed: ToPress("Escape", ctrlKey: true));

        yield return CreateDataItem(expectedCancelled: true,
                                    secondaryShortcut: "Ctrl+Alt+Shift+Escape",
                                    pressed: ToPress("Escape", ctrlKey: true, shiftKey: true, altKey: true));
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private static object[] CreateDataItem(bool expectedCancelled, string? primaryShortcut = null, string? secondaryShortcut = null, KeyboardEventArgs? pressed = null)
    {
        return [new ShortcutsDataItem(expectedCancelled, primaryShortcut, secondaryShortcut, pressed ?? new KeyboardEventArgs())];
    }

    private static KeyboardEventArgs ToPress(string key, bool? ctrlKey = null, bool? shiftKey = null, bool? altKey = null)
    {
        return new KeyboardEventArgs()
        {
            Key = key,
            CtrlKey = ctrlKey ?? false,
            ShiftKey = shiftKey ?? false,
            AltKey = altKey ?? false
        };
    }
}

public record ShortcutsDataItem(bool ExpectedCancelled, string? PrimaryShortcut, string? SecondaryShortcut, KeyboardEventArgs Pressed);
