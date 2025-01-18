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
        { "0", "0" },
        { "1", "4px" },
        { "2", "8px" },
        { "3", "12px" },
        { "4", "16px" },
        { "5", "20px" },
        { "6", "24px" },
        { "7", "28px" },
        { "8", "32px" },
        { "9", "36px" },
        { "10", "40px" },
        { "11", "44px" },
        { "12", "48px" },
        { "13", "52px" },
        { "14", "56px" },
        { "15", "60px" },
        { "16", "64px" },
        { "17", "68px" },
        { "18", "72px" },
        { "19", "76px" },
        { "20", "80px" },
        { "auto", "auto" },
    };

    readonly System.Collections.Generic.Dictionary<string, string> SpacingNegativeValues = new(System.StringComparer.Ordinal)
    {
        { "n1", "-4px" },
        { "n2", "-8px" },
        { "n3", "-12px" },
        { "n4", "-16px" },
        { "n5", "-20px" },
        { "n6", "-24px" },
        { "n7", "-28px" },
        { "n8", "-32px" },
        { "n9", "-36px" },
        { "n10", "-40px" },
        { "n11", "-44px" },
        { "n12", "-48px" },
        { "n13", "-52px" },
        { "n14", "-56px" },
        { "n15", "-60px" },
        { "n16", "-64px" },
        { "n17", "-68px" },
        { "n18", "-72px" },
        { "n19", "-76px" },
        { "n20", "-80px" },
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
