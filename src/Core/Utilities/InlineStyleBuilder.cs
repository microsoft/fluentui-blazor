using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Utilities;

public readonly struct InlineStyleBuilder
{
    private readonly Dictionary<string, StyleBuilder> _styles;

    /// <summary>
    /// Initializes a new instance of the <see cref="InlineStyleBuilder"/> class.
    /// </summary>
    public InlineStyleBuilder()
    {
        _styles = [];
    }

    /// <summary>
    /// Adds a conditional in-line style to the builder with space separator and closing semicolon..
    /// </summary>
    /// <param name="name"></param>
    /// <param name="prop"></param>
    /// <param name="value">Style to add</param>
    /// <returns>StyleBuilder</returns>
    public InlineStyleBuilder AddStyle(string name, string prop, string? value)
        => AddRaw(name, prop, value);

    /// <summary>
    /// Adds a conditional in-line style to the builder with space separator and closing semicolon..
    /// </summary>
    /// <param name="name"></param>
    /// <param name="prop"></param>
    /// <param name="value">Style to conditionally add.</param>
    /// <param name="when">Condition in which the style is added.</param>
    /// <returns>StyleBuilder</returns>
    public InlineStyleBuilder AddStyle(string name, string prop, string? value, bool when = true)
        => when ? AddStyle(name, prop, value) : this;

    /// <summary>
    /// Adds a conditional in-line style to the builder with space separator and closing semicolon..
    /// </summary>
    /// <param name="name"></param>
    /// <param name="prop"></param>
    /// <param name="value">Style to conditionally add.</param>
    /// <param name="when">Condition in which the style is added.</param>
    /// <returns>StyleBuilder</returns>
    public InlineStyleBuilder AddStyle(string name, string prop, string? value, Func<bool> when)
         => AddStyle(name, prop, value, when != null && when());

    /// <summary>
    /// Finalize the completed Style as a string.
    /// </summary>
    /// <returns>string</returns>
    public string? Build(bool newLineSeparator = true)
    {
        var separator = newLineSeparator ? Environment.NewLine : " ";

        var styles = _styles.Select(item =>
        {
            var style = item.Value.Build();
            if (string.IsNullOrWhiteSpace(style))
            {
                return string.Empty;
            }

            return $"{item.Key} {{ {item.Value.Build()} }}";
        });

        var result = string.Join(separator, styles.Where(i => !string.IsNullOrEmpty(i)));

        if (string.IsNullOrWhiteSpace(result))
        {
            return null;
        }

        return $"<style>{separator}{result}{separator}</style>";
    }

    /// <summary>
    /// Finalize the completed Style as a string.
    /// </summary>
    /// <returns>string</returns>
    public MarkupString BuildMarkupString()
    {
        var styles = Build();
        return styles != null ? (MarkupString)styles : (MarkupString)string.Empty;
    }

    /// <summary>
    /// Adds a raw string to the builder that will be concatenated with the next style or value added to the builder.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="prop"></param>
    /// <param name="value"></param>
    /// <returns>StyleBuilder</returns>
    private InlineStyleBuilder AddRaw(string name, string prop, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            if (!_styles.TryGetValue(name, out var styleBuilder))
            {
                styleBuilder = new StyleBuilder();
                _styles.Add(name, styleBuilder);
            }

            styleBuilder.AddStyle(prop, value);
        }

        return this;
    }
}
