using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentHighlighter : FluentComponentBase
{
    private Memory<string> _fragments;
    private string _regex = string.Empty;

    /// <summary>
    /// Whether or not the highlighted text is case sensitive
    /// </summary>
    [Parameter]
    public bool CaseSensitive { get; set; } = false;

    /// <summary>
    /// The fragment of text to be highlighted
    /// </summary>
    [Parameter]
    public string HighlightedText { get; set; } = string.Empty;

    /// <summary>
    /// The whole text in which a fragment will be highlighted
    /// </summary>
    [Parameter]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// List of delimiters chars. Example: " ,;".
    /// </summary>
    [Parameter]
    public string Delimiters { get; set; } = string.Empty;

    protected override void OnParametersSet()
    {
        var highlightedTexts = string.IsNullOrEmpty(Delimiters)
                             ? new string[] { HighlightedText }
                             : HighlightedText.Split(Delimiters.ToCharArray());

        _fragments = Highlighter.Splitter.GetFragments(Text, highlightedTexts, out _regex, CaseSensitive);
    }
}
