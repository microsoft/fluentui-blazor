// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Client.Documentation.Components.Dialog.Examples.Dialog;

public class PersonDetails
{
    public string Age { get; set; } = "";

    public MarkupString VeryLongDescription
        => (MarkupString)string.Join("", SampleData.Text.LoremIpsum.Select(i => $"<p>{i}</p>"));

    public override string ToString() => $"Age: {Age}";
}
