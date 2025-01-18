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
 */

// System.Console.WriteLine(SpacingGenerator.Script);

[System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "Non Production Code")]
internal class SpacingGenerator
{
    private readonly System.Text.StringBuilder _script = new();

    public static string Script => new SpacingGenerator().Generate();

    public string Generate()
    {
        ApplySpacingPositiveNegative("");

        foreach (var breakpoint in BreakpointsCssUtilitiesOnly)
        {
            _script.AppendLine($"@media screen and (min-width:{breakpoint.Value}) {{");
            ApplySpacingPositiveNegative($"{breakpoint.Key}-");
            _script.AppendLine("}");
        }

        return _script.ToString();
    }

    readonly System.Collections.Generic.Dictionary<string, string> BreakpointsCssUtilitiesOnly = new(System.StringComparer.Ordinal)
    {
        { "sm", "600px" },
        { "md", "960px" },
        { "lg", "1280px" },
        { "xl", "1920px" },
        { "xxl", "2560px" },
    };

    readonly System.Collections.Generic.Dictionary<string, string> SpacingValues = new(System.StringComparer.Ordinal)
    {
        { "0",  "0" },
        { "1",  "var(--design-unit)" },
        { "2",  "calc(var(--design-unit) * 2)" },
        { "3",  "calc(var(--design-unit) * 3)" },
        { "4",  "calc(var(--design-unit) * 4)" },
        { "5",  "calc(var(--design-unit) * 5)" },
        { "6",  "calc(var(--design-unit) * 6)" },
        { "7",  "calc(var(--design-unit) * 7)" },
        { "8",  "calc(var(--design-unit) * 8)" },
        { "9",  "calc(var(--design-unit) * 9)" },
        { "10", "calc(var(--design-unit) * 10)" },
        { "11", "calc(var(--design-unit) * 11)" },
        { "12", "calc(var(--design-unit) * 12)" },
        { "13", "calc(var(--design-unit) * 13)" },
        { "14", "calc(var(--design-unit) * 14)" },
        { "15", "calc(var(--design-unit) * 15)" },
        { "16", "calc(var(--design-unit) * 16)" },
        { "17", "calc(var(--design-unit) * 17)" },
        { "18", "calc(var(--design-unit) * 18)" },
        { "19", "calc(var(--design-unit) * 19)" },
        { "20", "calc(var(--design-unit) * 20)" },
        { "auto", "auto" },
    };

    readonly System.Collections.Generic.Dictionary<string, string> SpacingNegativeValues = new(System.StringComparer.Ordinal)
    {
        { "n1",  "calc(-1 * var(--design-unit))" },
        { "n2",  "calc(-2 * var(--design-unit))" },
        { "n3",  "calc(-3 * var(--design-unit))" },
        { "n4",  "calc(-4 * var(--design-unit))" },
        { "n5",  "calc(-5 * var(--design-unit))" },
        { "n6",  "calc(-6 * var(--design-unit))" },
        { "n7",  "calc(-7 * var(--design-unit))" },
        { "n8",  "calc(-8 * var(--design-unit))" },
        { "n9",  "calc(-9 * var(--design-unit))" },
        { "n10", "calc(-10 * var(--design-unit))" },
        { "n11", "calc(-11 * var(--design-unit))" },
        { "n12", "calc(-12 * var(--design-unit))" },
        { "n13", "calc(-13 * var(--design-unit))" },
        { "n14", "calc(-14 * var(--design-unit))" },
        { "n15", "calc(-15 * var(--design-unit))" },
        { "n16", "calc(-16 * var(--design-unit))" },
        { "n17", "calc(-17 * var(--design-unit))" },
        { "n18", "calc(-18 * var(--design-unit))" },
        { "n19", "calc(-19 * var(--design-unit))" },
        { "n20", "calc(-20 * var(--design-unit))" },
    };

    private void ApplySpacingPositiveNegative(string breakpoint)
    {
        foreach (var prop in new System.Collections.Generic.Dictionary<string, string>(System.StringComparer.Ordinal) { { "margin", "m" }, { "padding", "p" } })
        {
            foreach (var spacing in SpacingValues)
            {
                _script.AppendLine($".{prop.Value}t-{breakpoint}{spacing.Key} {{ {prop.Key}-top: {spacing.Value} !important; }}");
                _script.AppendLine($".{prop.Value}y-{breakpoint}{spacing.Key} {{ {prop.Key}-top: {spacing.Value} !important; }}");
                _script.AppendLine($".{prop.Value}r-{breakpoint}{spacing.Key} {{ {prop.Key}-right: {spacing.Value} !important; }}");
                _script.AppendLine($".{prop.Value}x-{breakpoint}{spacing.Key} {{ {prop.Key}-right: {spacing.Value} !important; }}");
                _script.AppendLine($".{prop.Value}l-{breakpoint}{spacing.Key} {{ {prop.Key}-left: {spacing.Value} !important; }}");
                _script.AppendLine($".{prop.Value}x-{breakpoint}{spacing.Key} {{ {prop.Key}-left: {spacing.Value} !important; }}");
                _script.AppendLine($".{prop.Value}b-{breakpoint}{spacing.Key} {{ {prop.Key}-bottom: {spacing.Value} !important; }}");
                _script.AppendLine($".{prop.Value}y-{breakpoint}{spacing.Key} {{ {prop.Key}-bottom: {spacing.Value} !important; }}");
                _script.AppendLine($".{prop.Value}s-{breakpoint}{spacing.Key} {{ {prop.Key}-inline-start: {spacing.Value} !important; }}");
                _script.AppendLine($".{prop.Value}e-{breakpoint}{spacing.Key} {{ {prop.Key}-inline-end: {spacing.Value} !important; }}");
                _script.AppendLine($".{prop.Value}a-{breakpoint}{spacing.Key} {{ {prop.Key}: {spacing.Value} !important; }}");
            }
        }

        foreach (var prop in new System.Collections.Generic.Dictionary<string, string>(System.StringComparer.Ordinal) { { "margin", "m" } })
        {
            foreach (var spacing in SpacingNegativeValues)
            {
                _script.AppendLine($".{prop.Value}t-{breakpoint}{spacing.Key} {{ {prop.Key}-top: {spacing.Value} !important; }}");
                _script.AppendLine($".{prop.Value}y-{breakpoint}{spacing.Key} {{ {prop.Key}-top: {spacing.Value} !important; }}");
                _script.AppendLine($".{prop.Value}r-{breakpoint}{spacing.Key} {{ {prop.Key}-right: {spacing.Value} !important; }}");
                _script.AppendLine($".{prop.Value}x-{breakpoint}{spacing.Key} {{ {prop.Key}-right: {spacing.Value} !important; }}");
                _script.AppendLine($".{prop.Value}l-{breakpoint}{spacing.Key} {{ {prop.Key}-left: {spacing.Value} !important; }}");
                _script.AppendLine($".{prop.Value}x-{breakpoint}{spacing.Key} {{ {prop.Key}-left: {spacing.Value} !important; }}");
                _script.AppendLine($".{prop.Value}b-{breakpoint}{spacing.Key} {{ {prop.Key}-bottom: {spacing.Value} !important; }}");
                _script.AppendLine($".{prop.Value}y-{breakpoint}{spacing.Key} {{ {prop.Key}-bottom: {spacing.Value} !important; }}");
                _script.AppendLine($".{prop.Value}s-{breakpoint}{spacing.Key} {{ {prop.Key}-inline-start: {spacing.Value} !important; }}");
                _script.AppendLine($".{prop.Value}e-{breakpoint}{spacing.Key} {{ {prop.Key}-inline-end: {spacing.Value} !important; }}");
                _script.AppendLine($".{prop.Value}a-{breakpoint}{spacing.Key} {{ {prop.Key}: {spacing.Value} !important; }}");
            }
        }
    }
}
