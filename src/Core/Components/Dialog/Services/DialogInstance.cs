// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public sealed class DialogInstance
{
    public DialogInstance(Type? type, DialogParameters parameters, object content)
    {
        ContentType = type;
        Parameters = parameters;
        Content = content;
        Id = Parameters.Id ?? Identifier.NewId();
    }

    public string Id { get; }

    public Type? ContentType { get; }

    public object Content { get; internal set; } = default!;

    public IJSObjectReference? PreviouslyFocusedElement { get; set; }

    public DialogParameters Parameters { get; internal set; }

    internal Dictionary<string, object>? GetParameterDictionary()
    {
        if (Content is null)
        {
            return null;
        }
        else
        {
            return new Dictionary<string, object> { { "Content", Content } };
        }
    }
}
