// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A component which highlights words or phrases within text.
/// </summary>
public partial class FluentHighlighter : FluentComponentBase
{
    private Memory<string> _fragments;
    private string _regex = string.Empty;

    /// <summary />
    public FluentHighlighter(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary>
    /// Gets or sets a value indicating whether the highlighted text is case sensitive.
    /// </summary>
    [Parameter]
    public bool CaseSensitive { get; set; } = false;

    /// <summary>
    /// Gets or sets the fragment of text to be highlighted.
    /// </summary>
    [Parameter]
    public string HighlightedText { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the whole text in which a fragment will be highlighted.
    /// </summary>
    [Parameter]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of delimiters chars. Example: " ,;".
    /// </summary>
    [Parameter]
    public string Delimiters { get; set; } = string.Empty;

    /// <summary>
    /// If true, highlights the text until the next regex boundary.
    /// </summary>
    [Parameter]
    public bool UntilNextBoundary { get; set; }

    /// <summary />
    protected override void OnParametersSet()
    {
        var highlightedTexts = string.IsNullOrEmpty(Delimiters)
                             ? [HighlightedText]
                             : HighlightedText.Split(Delimiters.ToCharArray());

        _fragments = HighlighterSplitter.GetFragments(Text, highlightedTexts, out _regex, CaseSensitive, UntilNextBoundary);
    }
}
