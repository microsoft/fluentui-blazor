namespace Microsoft.FluentUI.AspNetCore.Components.Utilities;

public readonly struct CssBuilder
{
    private readonly HashSet<string> _classes;
    private readonly string? _userClasses;

    /// <summary>
    /// Initializes a new instance of the <see cref="CssBuilder"/> class.
    /// </summary>

    public CssBuilder()
    {
        _classes = new HashSet<string>();
        _userClasses = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CssBuilder"/> class.
    /// </summary>
    /// <param name="userClasses">The user classes to include at the end.</param>
    public CssBuilder(string? userClasses)
    {
        _classes = [];
        _userClasses = string.IsNullOrWhiteSpace(userClasses)
                     ? null
                     : string.Join(" ", userClasses.Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }

    /// <summary>
    /// Adds a CSS Class to the builder with space separator.
    /// </summary>
    /// <param name="value">CSS Class to add</param>
    /// <returns>CssBuilder</returns>
    public CssBuilder AddClass(string? value) => AddClasses(value);

    /// <summary>
    /// Adds a conditional CSS Class to the builder with space separator.
    /// </summary>
    /// <param name="value">CSS Class to conditionally add.</param>
    /// <param name="when">Condition in which the CSS Class is added.</param>
    /// <returns>CssBuilder</returns>
    public CssBuilder AddClass(string? value, bool when = true) => when ? AddClasses(value) : this;

    /// <summary>
    /// Adds a conditional CSS Class to the builder with space separator.
    /// </summary>
    /// <param name="value">CSS Class to conditionally add.</param>
    /// <param name="when">Condition in which the CSS Class is added.</param>
    /// <returns>CssBuilder</returns>
    public CssBuilder AddClass(string? value, Func<bool>? when = null) => AddClasses(value, when != null && when());

    /// <summary>
    /// Adds multiple CSS Classes to the builder with space separator.
    /// </summary>
    /// <param name="values">Space-separated CSS Classes to add</param>
    /// <returns>CssBuilder</returns>
    public CssBuilder AddClasses(string? values)
    {
        if (!string.IsNullOrWhiteSpace(values))
        {
            var classes = values.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            _classes.UnionWith(classes);
        }
        return this;
    }

    /// <summary>
    /// Adds multiple CSS Classes to the builder with space separator, based on a condition.
    /// </summary>
    /// <param name="value">Space-separated CSS Classes to add</param>
    /// <param name="when">Condition in which the CSS Classes are added.</param>
    /// <returns>CssBuilder</returns>
    public CssBuilder AddClasses(string? value, bool when) => when ? AddClasses(value) : this;

    /// <summary>
    /// Adds multiple CSS Classes to the builder with space separator, based on a condition.
    /// </summary>
    /// <param name="value">Space-separated CSS Classes to add</param>
    /// <param name="when">Function that returns a condition in which the CSS Classes are added.</param>
    /// <returns>CssBuilder</returns>
    public CssBuilder AddClasses(string? value, Func<bool> when) => when() ? AddClasses(value) : this;

    /// <summary>
    /// Finalize the completed CSS Classes as a string.
    /// </summary>
    /// <returns>string</returns>
    public string? Build()
    {
        var allClasses = string.IsNullOrWhiteSpace(_userClasses)
                       ? _classes
                       : _classes.Union(new[] { _userClasses });

        if (!allClasses.Any())
        {
            return null;
        }

        return string.Join(" ", allClasses);
    }

    /// <summary>
    /// ToString should only and always call Build to finalize the rendered string.
    /// </summary>
    /// <returns></returns>
    public override string? ToString() => Build();
}
