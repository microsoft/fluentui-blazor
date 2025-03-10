// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Collections;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Dialog.Templates;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Dialog.Data;

public class ShortcutData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // Primary Action
        yield return [new ShortcutDataItem(ExpectedCancelled: false,
                                            Pressed: ToPress("Enter"))];

        yield return [new ShortcutDataItem(ExpectedCancelled: false,
                                            Pressed: ToPress("Enter"),
                                            PrimaryClickAsync: (e) => e.CloseAsync() )];

        yield return [new ShortcutDataItem(ExpectedCancelled: false,
                                            PrimaryShortcut: "Enter",
                                            Pressed: ToPress("Enter"))];

        yield return [new ShortcutDataItem(ExpectedCancelled: false,
                                            PrimaryShortcut: "Enter;Ctrl+Enter",
                                            Pressed: ToPress("Enter", ctrlKey: true))];

        yield return [new ShortcutDataItem(ExpectedCancelled: false,
                                            PrimaryShortcut: "Ctrl+Alt+Shift+Enter",
                                            Pressed: ToPress("Enter", ctrlKey: true, shiftKey: true, altKey: true))];

        // Secondary Action
        yield return [new ShortcutDataItem(ExpectedCancelled: true,
                                            Pressed: ToPress("Escape"))];

        yield return [new ShortcutDataItem(ExpectedCancelled: true,
                                            Pressed: ToPress("Escape"),
                                            SecondaryClickAsync: (e) => e.CancelAsync() )];

        yield return [new ShortcutDataItem(ExpectedCancelled: true,
                                            SecondaryShortcut: "Escape",
                                            Pressed: ToPress("Escape"))];

        yield return [new ShortcutDataItem(ExpectedCancelled: true,
                                            SecondaryShortcut: "Escape;Ctrl+Escape",
                                            Pressed: ToPress("Escape", ctrlKey: true))];

        yield return [new ShortcutDataItem(ExpectedCancelled: true,
                                            SecondaryShortcut: "Ctrl+Alt+Shift+Escape",
                                            Pressed: ToPress("Escape", ctrlKey: true, shiftKey: true, altKey: true))];

        // Unknown Shortcut
        yield return [new ShortcutDataItem(ExpectedCancelled: false,
                                            Pressed: ToPress("A"),
                                            RenderOptions: CloseAfter200ms)];

        yield return [new ShortcutDataItem(ExpectedCancelled: false,
                                            PrimaryShortcut: "",
                                            Pressed: ToPress("A"),
                                            RenderOptions: CloseAfter200ms)];

        yield return [new ShortcutDataItem(ExpectedCancelled: false,
                                            SecondaryShortcut: "",
                                            Pressed: ToPress("A"),
                                            RenderOptions: CloseAfter200ms)];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private static DialogRenderOptions CloseAfter200ms => new()
    {
        AutoClose = true,
        AutoCloseDelay = 200
    };

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

public record ShortcutDataItem(
    bool ExpectedCancelled,
    string? PrimaryShortcut = null,
    Func<IDialogInstance, Task>? PrimaryClickAsync = null,
    string? SecondaryShortcut = null,
    Func<IDialogInstance, Task>? SecondaryClickAsync = null,
    KeyboardEventArgs? Pressed = null,
    DialogRenderOptions? RenderOptions = null);
