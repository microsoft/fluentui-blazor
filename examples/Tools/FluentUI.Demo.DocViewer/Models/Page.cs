// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace FluentUI.Demo.DocViewer.Models;

/// <summary>
/// Represents a page of a markdown document.
/// </summary>
public record Page
{
    /// <summary>
    /// 
    ///   ---
    ///   title: Button
    ///   route: /Button
    ///   ---
    ///   My content
    /// 
    /// </summary>
    /// <param name="content"></param>
    public Page(string content)
    {
        var items = ExtractHeaderContent(content);

        Headers = items.Where(i => i.Key != "content").ToDictionary();
        Content = GetItem(items, "content");
        Title = GetItem(items, "title");
        Route = GetItem(items, "route");
    }

    /// <summary>
    /// Gets the header items.
    /// </summary>
    public IDictionary<string, string> Headers { get; }

    /// <summary>
    /// Gets the page content
    /// </summary>
    public string Content { get; }

    /// <summary>
    /// Gets the route defined in the <see cref="Headers"/>
    /// </summary>
    public string Route { get; } = string.Empty;

    /// <summary>
    /// Gets the page title defined in the <see cref="Headers"/>
    /// </summary>
    public string Title { get; } = string.Empty;

    /// <summary>
    /// Returns the key value or a string.empty (if not found)
    /// </summary>
    /// <param name="items"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    private static string GetItem(Dictionary<string, string> items, string key)
    {
        return items.TryGetValue(key, out var value) ? value : string.Empty;
    }

    /// <summary>
    /// Extract the Markdown header content
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private static Dictionary<string, string> ExtractHeaderContent(string input)
    {
        var dictionary = new Dictionary<string, string>();
        var regex = new Regex(@"---\s*(.*?)\s*---", RegexOptions.Singleline);
        var match = regex.Match(input);

        if (match.Success)
        {
            var lines = match.Groups[1].Value.Split('\n');
            foreach (var line in lines)
            {
                var parts = line.Split([':'], 2);
                if (parts.Length == 2)
                {
                    var key = parts[0].Trim();
                    var value = parts[1].Split('#')[0].Trim(); // Remove comments
                    dictionary[key] = value;
                }
            }

            var content = input[(match.Index + match.Length)..].Trim();
            dictionary["content"] = content;

        }

        return dictionary;
    }
}
