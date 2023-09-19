namespace Microsoft.Fast.Components.FluentUI.Utilities;

public readonly struct StyleBuilder
{
    private readonly HashSet<string> _styles = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="StyleBuilder"/> class.
    /// </summary>
    public StyleBuilder()
    {       
    }

    /// <summary>
    /// Adds a conditional in-line style to the builder with space separator and closing semicolon.
    /// </summary>
    /// <param name="style"></param>
    public StyleBuilder AddStyle(string? style) => AddRaw(style);

    /// <summary>
    /// Adds a conditional in-line style to the builder with space separator and closing semicolon..
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="value">Style to add</param>
    /// <returns>StyleBuilder</returns>
    public StyleBuilder AddStyle(string prop, string? value) => AddRaw($"{prop}: {value}");

    /// <summary>
    /// Adds a conditional in-line style to the builder with space separator and closing semicolon..
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="value">Style to conditionally add.</param>
    /// <param name="when">Condition in which the style is added.</param>
    /// <returns>StyleBuilder</returns>
    public StyleBuilder AddStyle(string prop, string? value, bool when = true) => when ? this.AddStyle(prop, value) : this;

    /// <summary>
    /// Adds a conditional in-line style to the builder with space separator and closing semicolon..
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="value">Style to conditionally add.</param>
    /// <param name="when">Condition in which the style is added.</param>
    /// <returns>StyleBuilder</returns>
    public StyleBuilder AddStyle(string prop, string? value, Func<bool> when) => this.AddStyle(prop, value, when != null && when());

    /// <summary>
    /// Finalize the completed Style as a string.
    /// </summary>
    /// <returns>string</returns>
    public string? Build()
    {
        if (!_styles.Any())
        {
            return null;
        }

        return string.Concat(_styles.Select(s => $"{s}; ")).TrimEnd();
    }

    /// <summary>
    /// ToString should only and always call Build to finalize the rendered string.
    /// </summary>
    /// <returns></returns>
    public override string? ToString() => Build();

    /// <summary>
    /// Adds a raw string to the builder that will be concatenated with the next style or value added to the builder.
    /// </summary>
    /// <param name="style"></param>
    /// <returns>StyleBuilder</returns>
    private StyleBuilder AddRaw(string? style)
    {
        if (!string.IsNullOrWhiteSpace(style))
        {
            _styles.Add(style.Trim().TrimEnd(';'));
        }
        
        return this;
    }
}