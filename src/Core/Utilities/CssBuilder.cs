using System.Text.RegularExpressions;

namespace Microsoft.FluentUI.AspNetCore.Components.Utilities;

public readonly partial struct CssBuilder
{
    private readonly HashSet<string> _classes;
    private readonly string[]? _userClasses;
    private static readonly Regex ValidClassNameRegex = GenerateValidClassNameRegex();

    /// <summary>
    /// Validate CSS class, which must respect the following regex: "^-?[_a-zA-Z]+[_a-zA-Z0-9-]*$".
    /// Default is true.
    /// </summary>
    public static bool ValidateClassNames = true;

    private readonly bool _validateClassNames = ValidateClassNames;

    /// <summary>
    /// Initializes a new instance of the <see cref="CssBuilder"/> class.
    /// </summary>
    public CssBuilder()
    {
        _classes = [];
        _userClasses = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CssBuilder"/> class.
    /// </summary>
    internal CssBuilder(bool validateClassNames, string? userClasses) : this(userClasses)
    {
        _validateClassNames = validateClassNames;
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
                     : SplitAndValidate(userClasses).ToArray();
    }

    /// <summary>
    /// Adds one or more CSS Classes to the builder with space separator.
    /// </summary>
    /// <param name="values">Space-separated CSS Classes to add</param>
    /// <returns>CssBuilder</returns>
    public CssBuilder AddClass(string? values)
    {
        if (!string.IsNullOrWhiteSpace(values))
        {
            _classes.UnionWith(SplitAndValidate(values));
        }
        return this;
    }

    /// <summary>
    /// Adds one or more CSS Classes to the builder with space separator, based on a condition.
    /// </summary>
    /// <param name="value">Space-separated CSS Classes to add</param>
    /// <param name="when">Condition in which the CSS Classes are added.</param>
    /// <returns>CssBuilder</returns>
    public CssBuilder AddClass(string? value, bool when) => when ? AddClass(value) : this;

    /// <summary>
    /// Adds one or more CSS Classes to the builder with space separator, based on a condition.
    /// </summary>
    /// <param name="value">Space-separated CSS Classes to add</param>
    /// <param name="when">Function that returns a condition in which the CSS Classes are added.</param>
    /// <returns>CssBuilder</returns>
    public CssBuilder AddClass(string? value, Func<bool> when) => when() ? AddClass(value) : this;

    /// <summary>
    /// Finalize the completed CSS Classes as a string.
    /// </summary>
    /// <returns>string</returns>
    public string? Build()
    {
        var allClasses = _userClasses == null
            ? _classes
            : _classes.Union(_userClasses);

        var result = string.Join(" ", allClasses);
        return string.IsNullOrWhiteSpace(result) ? null : result;
    }

    /// <summary>
    /// ToString should only and always call Build to finalize the rendered string.
    /// </summary>
    /// <returns>string</returns>
    public override string? ToString() => Build();

    /// <summary>
    /// Validates if the provided class name is valid.
    /// </summary>
    /// <param name="className">CSS class name to validate</param>
    /// <returns>True if valid, otherwise false</returns>
    private bool IsValidClassName(string className)
    {
        return _validateClassNames ? ValidClassNameRegex.IsMatch(className) : true;
    }

    /// <summary>
    /// Splits a space-separated string of class names and validates each one.
    /// </summary>
    /// <param name="input">Space-separated CSS Classes</param>
    /// <returns>Enumerable of valid class names</returns>
    private IEnumerable<string> SplitAndValidate(string input)
    {
        return input.Split(' ', StringSplitOptions.RemoveEmptyEntries).Where(IsValidClassName);
    }

    /// <summary>
    /// Generates the regex used to validate CSS class names.
    /// </summary>
    /// <returns>A compiled regex for validating CSS class names</returns>
    [GeneratedRegex(@"^-?[_a-zA-Z]+[_a-zA-Z0-9-]*$", RegexOptions.Compiled)]
    private static partial Regex GenerateValidClassNameRegex();
}
