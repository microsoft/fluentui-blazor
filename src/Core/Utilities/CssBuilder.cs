namespace Microsoft.Fast.Components.FluentUI.Utilities;

public readonly struct CssBuilder
{
    private readonly HashSet<string> _classes = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="CssBuilder"/> class.
    /// </summary>
    public CssBuilder()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CssBuilder"/> class.
    /// </summary>
    public CssBuilder(string? value)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Adds a CSS Class to the builder with space separator.
    /// </summary>
    /// <param name="value">CSS Class to add</param>
    /// <returns>CssBuilder</returns>
    public CssBuilder AddClass(string? value) => AddRaw(value);

    /// <summary>
    /// Adds a conditional CSS Class to the builder with space separator.
    /// </summary>
    /// <param name="value">CSS Class to conditionally add.</param>
    /// <param name="when">Condition in which the CSS Class is added.</param>
    /// <returns>CssBuilder</returns>
    public CssBuilder AddClass(string? value, bool when = true) => when ? AddClass(value) : this;

    /// <summary>
    /// Adds a conditional CSS Class to the builder with space separator.
    /// </summary>
    /// <param name="value">CSS Class to conditionally add.</param>
    /// <param name="when">Condition in which the CSS Class is added.</param>
    /// <returns>CssBuilder</returns>
    public CssBuilder AddClass(string? value, Func<bool>? when = null) => AddClass(value, when != null && when());

    /// <summary>
    /// Finalize the completed CSS Classes as a string.
    /// </summary>
    /// <returns>string</returns>
    public string? Build()
    {
        if (!_classes.Any())
        {
            return null;
        }

        return string.Join(" ", _classes);
    }

    /// <summary>
    /// ToString should only and always call Build to finalize the rendered string.
    /// </summary>
    /// <returns></returns>
    public override string? ToString() => Build();

    /// <summary>
    /// Adds a raw string to the builder that will be concatenated with the next classes or value added to the builder.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>StyleBuilder</returns>
    private CssBuilder AddRaw(string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            _classes.Add(value.Trim());
        }

        return this;
    }
}
