// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/*
 *  To generate the `Spacing.razor.css` utilities, run the following code in a C# Console Application,
 *  and copy the output to the `Spacing.razor.css` file.
 *  
 *    System.Console.WriteLine(SpacingGenerator.Script);
 *    
 *  Example, using https://dotnetfiddle.net/
 *  
 * We ran several tests to determine the number of elements (e.g. `.ml-?`) to generate, depending on requirements 
 * and the size of the CSS file generated. We determined that 8 elements is a good compromise
 * Spacing can be up to 32px (positive or negative).
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
 *    for (int i = 1; i < 25; i++)
 *    {
 *        var script = SpacingGenerator.GenerateScript(i);
 *        System.Console.WriteLine($"   Count = {i:00}   =>   Max spacing size: {i*4}px - File size:  {script.Length / 1024} kb.");
 *    }
 */

// System.Console.WriteLine(SpacingGenerator.Script);

[System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "Non Production Code")]
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "Non Production Code")]
internal class SpacingGenerator
{
    private int _spaceCount = 8;
    private readonly System.Text.StringBuilder _script = new();

    public static string Script => new SpacingGenerator().Generate(8);

    public static string GenerateScript(int count) => new SpacingGenerator().Generate(count);

    private string Generate(int count)
    {
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

        return _script.ToString();
    }

    private readonly System.Collections.Generic.Dictionary<string, string> BreakpointsCssUtilitiesOnly = new(System.StringComparer.Ordinal)
    {
        { "sm", "600px" },
        { "md", "960px" },
        { "lg", "1280px" },
        { "xl", "1920px" },
        { "xxl", "2560px" },
    };

    private static System.Collections.Generic.Dictionary<int, string> SpacingHorizontalVariables = new();

    private static System.Collections.Generic.Dictionary<int, string> SpacingVerticalVariables = new();

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
        if (string.Equals(value, "auto", StringComparison.OrdinalIgnoreCase))
        {
            return "auto";
        }

        var index = System.Convert.ToInt32(value, System.Globalization.CultureInfo.InvariantCulture);
        var negative = index < 0;

        if (direction == 'h')
        {
            return negative
                ? $"calc(-1 * var({SpacingHorizontalVariables[Math.Abs(index)]}))"
                : $"var({SpacingHorizontalVariables[Math.Abs(index)]})";
        }

        return negative
                ? $"calc(-1 * var({SpacingVerticalVariables[Math.Abs(index)]}))"
                : $"var({SpacingVerticalVariables[Math.Abs(index)]})";
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
            }
        }
    }
}
