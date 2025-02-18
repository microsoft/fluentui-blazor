// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/*
 *  To generate the `Spacing.razor.css` utilities, run the following code in a C# Console Application,
 *  and copy the output to the `Spacing.razor.css`, "Margin.cs" or "Padding.cs" files.
 *  
 *    System.Console.WriteLine(SpacingGenerator.Script);
 *    System.Console.WriteLine(SpacingGenerator.CSharpMargin);
 *    System.Console.WriteLine(SpacingGenerator.CSharpPadding);
 *    
 *  Example, using https://dotnetfiddle.net/
 *  
 *  We ran several tests to determine the number of elements (e.g. .ml-?) to generate, depending on requirements
 *  and the size of the CSS file generated. We determined that 8 elements, where Spacing can then be set up to 32px
 *  (either positive or negative), gives a good compromise between options and size of the CSS file.
 *
 *  1. To generate this CSS content, you can run this command in a C# Console Application:
 *
 *       System.Console.WriteLine(SpacingGenerator.Script);    // Default `Count` is 8
 *
 *  2. To find what file size corresponds to what count, the following code can be run in a C# Console Application.
 *
 *       for (int i = 1; i < 25; i++)
 *       {
 *           var script = SpacingGenerator.GenerateScript(i);
 *           System.Console.WriteLine($"   Count = {i:00}   =>   Max spacing size: {i*4}px - File size:  {script.Length / 1024} kb.");
 *       }
 *   
 *  Results:
 *   
 *    Count = 01   =>   Max spacing size:  4px - File size:  29 kb.
 *    Count = 02   =>   Max spacing size:  8px - File size:  43 kb.
 *    Count = 03   =>   Max spacing size: 12px - File size:  57 kb.
 *    Count = 04   =>   Max spacing size: 16px - File size:  71 kb.
 *    Count = 05   =>   Max spacing size: 20px - File size:  85 kb.
 *    Count = 06   =>   Max spacing size: 24px - File size: 100 kb.
 *    Count = 07   =>   Max spacing size: 28px - File size: 114 kb.
 * => Count = 08   =>   Max spacing size: 32px - File size: 129 kb. <==
 *    Count = 09   =>   Max spacing size: 36px - File size: 144 kb.
 *    Count = 10   =>   Max spacing size: 40px - File size: 159 kb.
 *    Count = 11   =>   Max spacing size: 44px - File size: 175 kb.
 *    Count = 12   =>   Max spacing size: 48px - File size: 191 kb.
 *    Count = 13   =>   Max spacing size: 52px - File size: 207 kb.
 *    Count = 14   =>   Max spacing size: 56px - File size: 223 kb.
 *    Count = 15   =>   Max spacing size: 60px - File size: 239 kb.
 *    Count = 16   =>   Max spacing size: 64px - File size: 256 kb.
 *    Count = 17   =>   Max spacing size: 68px - File size: 273 kb.
 *    Count = 18   =>   Max spacing size: 72px - File size: 290 kb.
 *    Count = 19   =>   Max spacing size: 76px - File size: 307 kb.
 *    Count = 20   =>   Max spacing size: 80px - File size: 325 kb.
 *    Count = 21   =>   Max spacing size: 84px - File size: 342 kb.
 *    Count = 22   =>   Max spacing size: 88px - File size: 360 kb.
 *    Count = 23   =>   Max spacing size: 92px - File size: 378 kb.
 *    Count = 24   =>   Max spacing size: 96px - File size: 397 kb.
 *    
 */

[System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "Non Production Code")]
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "Non Production Code")]
internal class SpacingGenerator
{
    private int _spaceCount = 8;
    private readonly System.Text.StringBuilder _script = new();
    private readonly System.Text.StringBuilder _csharpMargin = new();
    private readonly System.Text.StringBuilder _csharpPadding = new();

    public static string Script => new SpacingGenerator().Generate(8).Styles;

    public static string CSharpMargin => new SpacingGenerator().Generate(8).Margin;

    public static string CSharpPadding => new SpacingGenerator().Generate(8).Padding;

    public static string GenerateScript(int count) => new SpacingGenerator().Generate(count).Styles;

    private (string Styles, string Margin, string Padding) Generate(int count)
    {
        AddClassName(_csharpMargin, "Margin");
        AddClassName(_csharpPadding, "Padding");

        _spaceCount = count;
        SpacingValues = CreateSpacingValues(_spaceCount, negative: false, withZero: true, withAuto: true);
        SpacingNegativeValues = CreateSpacingValues(_spaceCount, negative: true, withZero: false, withAuto: false);
        SpacingHorizontalVariables = CreateSpacingVariables("Horizontal");
        SpacingVerticalVariables = CreateSpacingVariables("Vertical");

        ApplySpacingPositiveNegative("");

        foreach (var breakpoint in BreakpointsCssUtilitiesOnly)
        {
            _script.AppendLine($"@media screen and (min-width:{breakpoint.Value}) {{");
            ApplySpacingPositiveNegative($"{breakpoint.Key}-");
            _script.AppendLine("}");
        }

        _csharpMargin.AppendLine("}");
        _csharpPadding.AppendLine("}");

        return (_script.ToString(), _csharpMargin.ToString(), _csharpPadding.ToString());
    }

    private static void AddClassName(System.Text.StringBuilder builder, string name)
    {
        builder.AppendLine("// ------------------------------------------------------------------------");
        builder.AppendLine("// MIT License - Copyright (c) Microsoft Corporation. All rights reserved. ");
        builder.AppendLine("// ------------------------------------------------------------------------");
        builder.AppendLine();
        builder.AppendLine("namespace Microsoft.FluentUI.AspNetCore.Components;");
        builder.AppendLine();
        builder.AppendLine("/// <summary>");
        builder.AppendLine("/// List of all available " + name + " styles.");
        builder.AppendLine("/// </summary>");
        builder.AppendLine("public static class " + name);
        builder.AppendLine("{");
    }

    private static readonly System.Collections.Generic.Dictionary<string, string> BreakpointsCssUtilitiesOnly = new(System.StringComparer.Ordinal)
    {
        { "sm", "600px" },
        { "md", "960px" },
        { "lg", "1280px" },
        { "xl", "1920px" },
        { "xxl", "2560px" },
    };

    private static System.Collections.Generic.Dictionary<int, string> SpacingHorizontalVariables = [];

    private static System.Collections.Generic.Dictionary<int, string> SpacingVerticalVariables = [];

    private System.Collections.Generic.Dictionary<string, string> SpacingValues = new(System.StringComparer.Ordinal);

    private System.Collections.Generic.Dictionary<string, string> SpacingNegativeValues = new(System.StringComparer.Ordinal);

    private static System.Collections.Generic.Dictionary<string, string> CreateSpacingValues(int count, bool negative, bool withZero, bool withAuto)
    {
        var values = new System.Collections.Generic.Dictionary<string, string>(System.StringComparer.Ordinal);

        if (withZero)
        {
            values.Add("0", "0");
        }

        for (var i = 1; i <= count; i++)
        {
            var key = $"{(negative ? "n" : "")}{i}";
            //var value = negative || i > 1
            //          ? $"calc({(negative ? -i : i)} * var(--design-unit))"
            //          : $"var(--design-unit)";
            var value = $"{(negative ? -i : i)}";   // Updated to use --spacingVerticalM and --spacingHorizontalM instead of --design-unit
            values.Add(key, value);
        }

        if (withAuto)
        {
            values.Add("auto", "auto");
        }

        return values;
    }

    private static string ConvertSpacingValueToVariable(string value, char direction)
    {
        if (string.Equals(value, "auto", System.StringComparison.OrdinalIgnoreCase))
        {
            return "auto";
        }

        var index = System.Convert.ToInt32(value, System.Globalization.CultureInfo.InvariantCulture);
        var negative = index < 0;

        if (direction == 'h')
        {
            return negative
                ? $"calc(-1 * var({SpacingHorizontalVariables[System.Math.Abs(index)]}))"
                : $"var({SpacingHorizontalVariables[System.Math.Abs(index)]})";
        }

        return negative
                ? $"calc(-1 * var({SpacingVerticalVariables[System.Math.Abs(index)]}))"
                : $"var({SpacingVerticalVariables[System.Math.Abs(index)]})";
    }

    private static System.Collections.Generic.Dictionary<int, string> CreateSpacingVariables(string direction)
    {
        var requiredElements = new System.Collections.Generic.Dictionary<int, string>()
        {
            { 0, $"--spacing{direction}None" },
            { 1, $"--spacing{direction}XS" },
            { 2, $"--spacing{direction}S" },
            { 3, $"--spacing{direction}M" },
            { 4, $"--spacing{direction}L" },
        };

        for (var i = 5; i <= 50; i++)
        {
            var xElements = new string('X', i - 4);
            requiredElements.Add(i, $"--spacing{direction}{xElements}L");
        }

        return requiredElements;
    }

    private void ApplySpacingPositiveNegative(string breakpoint)
    {
        foreach (var prop in new System.Collections.Generic.Dictionary<string, string>(System.StringComparer.Ordinal) { { "margin", "m" }, { "padding", "p" } })
        {
            foreach (var spacing in SpacingValues)
            {
                var hValue = ConvertSpacingValueToVariable(spacing.Value, 'h');
                var vValue = ConvertSpacingValueToVariable(spacing.Value, 'v');

                _script.AppendLine($".{prop.Value}t-{breakpoint}{spacing.Key} {{ {prop.Key}-top: {vValue} !important; }}");
                _script.AppendLine($".{prop.Value}y-{breakpoint}{spacing.Key} {{ {prop.Key}-top: {vValue} !important; }}");
                _script.AppendLine($".{prop.Value}r-{breakpoint}{spacing.Key} {{ {prop.Key}-right: {hValue} !important; }}");
                _script.AppendLine($".{prop.Value}x-{breakpoint}{spacing.Key} {{ {prop.Key}-right: {hValue} !important; }}");
                _script.AppendLine($".{prop.Value}l-{breakpoint}{spacing.Key} {{ {prop.Key}-left: {hValue} !important; }}");
                _script.AppendLine($".{prop.Value}x-{breakpoint}{spacing.Key} {{ {prop.Key}-left: {hValue} !important; }}");
                _script.AppendLine($".{prop.Value}b-{breakpoint}{spacing.Key} {{ {prop.Key}-bottom: {vValue} !important; }}");
                _script.AppendLine($".{prop.Value}y-{breakpoint}{spacing.Key} {{ {prop.Key}-bottom: {vValue} !important; }}");
                _script.AppendLine($".{prop.Value}s-{breakpoint}{spacing.Key} {{ {prop.Key}-inline-start: {hValue} !important; }}");
                _script.AppendLine($".{prop.Value}e-{breakpoint}{spacing.Key} {{ {prop.Key}-inline-end: {hValue} !important; }}");
                _script.AppendLine($".{prop.Value}a-{breakpoint}{spacing.Key} {{ {prop.Key}: {vValue} {hValue} !important; }}");

                // Convert these 11 lines to C# constants
                var code = ConvertCssToCSharp(_script, numberOfLines: 11);
                (string.Equals(prop.Value, "m", System.StringComparison.Ordinal) ? _csharpMargin : _csharpPadding).Append(code);
            }
        }

        foreach (var prop in new System.Collections.Generic.Dictionary<string, string>(System.StringComparer.Ordinal) { { "margin", "m" } })
        {
            foreach (var spacing in SpacingNegativeValues)
            {
                var hValue = ConvertSpacingValueToVariable(spacing.Value, 'h');
                var vValue = ConvertSpacingValueToVariable(spacing.Value, 'v');

                _script.AppendLine($".{prop.Value}t-{breakpoint}{spacing.Key} {{ {prop.Key}-top: {vValue} !important; }}");
                _script.AppendLine($".{prop.Value}y-{breakpoint}{spacing.Key} {{ {prop.Key}-top: {vValue} !important; }}");
                _script.AppendLine($".{prop.Value}r-{breakpoint}{spacing.Key} {{ {prop.Key}-right: {hValue} !important; }}");
                _script.AppendLine($".{prop.Value}x-{breakpoint}{spacing.Key} {{ {prop.Key}-right: {hValue} !important; }}");
                _script.AppendLine($".{prop.Value}l-{breakpoint}{spacing.Key} {{ {prop.Key}-left: {hValue} !important; }}");
                _script.AppendLine($".{prop.Value}x-{breakpoint}{spacing.Key} {{ {prop.Key}-left: {hValue} !important; }}");
                _script.AppendLine($".{prop.Value}b-{breakpoint}{spacing.Key} {{ {prop.Key}-bottom: {vValue} !important; }}");
                _script.AppendLine($".{prop.Value}y-{breakpoint}{spacing.Key} {{ {prop.Key}-bottom: {vValue} !important; }}");
                _script.AppendLine($".{prop.Value}s-{breakpoint}{spacing.Key} {{ {prop.Key}-inline-start: {hValue} !important; }}");
                _script.AppendLine($".{prop.Value}e-{breakpoint}{spacing.Key} {{ {prop.Key}-inline-end: {hValue} !important; }}");
                _script.AppendLine($".{prop.Value}a-{breakpoint}{spacing.Key} {{ {prop.Key}: {vValue} {hValue} !important; }}");

                // Convert these 11 lines to C# constants
                var code = ConvertCssToCSharp(_script, numberOfLines: 11);
                (string.Equals(prop.Value, "m", System.StringComparison.Ordinal) ? _csharpMargin : _csharpPadding).Append(code);
            }
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "MA0001:StringComparison is missing", Justification = "")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0051:Method is too long", Justification = "")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "MA0009:Add regex evaluation timeout", Justification = "<Pending>")]
    public static string ConvertCssToCSharp(System.Text.StringBuilder script, int numberOfLines)
    {
        var code = new System.Text.StringBuilder();

        // Extract the last line
        var lines = System.Linq.Enumerable.TakeLast(script.ToString()
                                                          .Split(["\r\n", "\n", System.Environment.NewLine], System.StringSplitOptions.RemoveEmptyEntries)
                                                  , numberOfLines);

        foreach (var cssLine in lines)
        {
            var cssName = cssLine.Substring(cssLine.IndexOf('.') + 1, cssLine.IndexOf(' ') - cssLine.IndexOf('.') - 1);

            if (code.ToString().Contains($"\"{cssName} \"")) // Already exists
            {
                continue;
            }

            // Extract class name
            var match = System.Text.RegularExpressions.Regex.Match(cssLine, "\\.(m|p)(\\w+)(-([a-z]+))?-(n?)([0-9]+|auto)");
            if (!match.Success)
            {
                throw new System.InvalidOperationException($"Invalid CSS class name: {cssLine}");
            }

            // Mapping for prefixes
            System.Collections.Generic.Dictionary<string, string> prefixMap = new(System.StringComparer.Ordinal)
            {
                { "t", "Top" },
                { "b", "Bottom" },
                { "l", "Left" },
                { "r", "Right" },
                { "s", "Start" },
                { "e", "End" },
                { "x", "Horizontal" },
                { "y", "Vertical" },
                { "m", "Margin" },
                { "a", "All" },
            };

            var prefix = match.Groups[2].Value;
            var size = match.Groups[4].Value;
            var negative = string.Equals(match.Groups[5].Value, "n", System.StringComparison.Ordinal) ? "Negative" : "";
            var number = match.Groups[6].Value;
            var numberInPixels = number switch
            {
                "auto" => "",
                _ => $" ({prefixMap[prefix]}: {(negative == "" ? "" : "-")}{(int.Parse(number, System.Globalization.CultureInfo.InvariantCulture) * 4)}px)"
            };
            var breakpoint = string.IsNullOrEmpty(size) ? "" : $" where `min-width: {BreakpointsCssUtilitiesOnly[size]}`";

            size = size switch
            {
                "sm" => "_ForSmall",
                "md" => "_ForMedium",
                "lg" => "_ForLarge",
                "xl" => "_ForExtraLarge",
                "xxl" => "_ForExtraExtraLarge",
                _ => ""
            };

            // Convert to proper format
            // public const string TopXxl0 = "mt-xxl-0"
            code.AppendLine($"    /// <summary>CSS `{cssName}`{numberInPixels}{breakpoint}.</summary>");
            code.AppendLine($"    public const string {prefixMap[prefix]}{negative}{Capitalize(number)}{Capitalize(size)} = \"{cssName} \";");
            code.AppendLine();
        }

        return code.ToString();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "MA0011:IFormatProvider is missing", Justification = "<Pending>")]
    private static string Capitalize(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        return char.ToUpper(input[0]) + input[1..];
    }
}
