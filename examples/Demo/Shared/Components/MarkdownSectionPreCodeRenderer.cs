using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace FluentUI.Demo.Shared.Components;

/// <summary>
/// Modified version of original markdig CodeBlockRenderer
/// </summary>
/// <see cref="https://github.com/xoofx/markdig/blob/master/src/Markdig/Renderers/Html/CodeBlockRenderer.cs"/>
internal class MarkdownSectionPreCodeRenderer : HtmlObjectRenderer<CodeBlock>
{
    private HashSet<string>? _blocksAsDiv;
    private readonly string? _preTagClass;
    public MarkdownSectionPreCodeRenderer(string? PreTagClass = null)
    {
        _preTagClass = PreTagClass;
    }

    public bool OutputAttributesOnPre { get; set; }

    /// <summary>
    /// Gets a map of fenced code block infos that should be rendered as div blocks instead of pre/code blocks.
    /// </summary>
    public HashSet<string> BlocksAsDiv => _blocksAsDiv ??= new HashSet<string>(StringComparer.OrdinalIgnoreCase);

    protected override void Write(HtmlRenderer renderer, CodeBlock obj)
    {
        renderer.EnsureLine();

        if (_blocksAsDiv is not null && (obj as FencedCodeBlock)?.Info is string info && _blocksAsDiv.Contains(info))
        {
            var infoPrefix = (obj.Parser as FencedCodeBlockParser)?.InfoPrefix ??
                             FencedCodeBlockParser.DefaultInfoPrefix;

            // We are replacing the HTML attribute `language-mylang` by `mylang` only for a div block
            // NOTE that we are allocating a closure here

            if (renderer.EnableHtmlForBlock)
            {
                renderer.Write("<div")
                        .WriteAttributes(obj.TryGetAttributes(),
                            cls => cls.StartsWith(infoPrefix, StringComparison.Ordinal) ? cls.Substring(infoPrefix.Length) : cls)
                        .Write('>');
            }

            renderer.WriteLeafRawLines(obj, true, true, true);

            if (renderer.EnableHtmlForBlock)
            {
                renderer.WriteLine("</div>");
            }
        }
        else
        {
            if (renderer.EnableHtmlForBlock)
            {
                if (_preTagClass is string)
                {
                    renderer.Write($"<pre class=\"{_preTagClass}\"");
                }
                else
                {
                    renderer.Write("<pre");
                }

                if (OutputAttributesOnPre)
                {
                    renderer.WriteAttributes(obj);
                }

                renderer.Write("><code");

                if (!OutputAttributesOnPre)
                {
                    renderer.WriteAttributes(obj);
                }

                renderer.Write('>');
            }

            renderer.WriteLeafRawLines(obj, true, true);

            if (renderer.EnableHtmlForBlock)
            {
                renderer.WriteLine("</code></pre>");
            }
        }

        renderer.EnsureLine();
    }
}
