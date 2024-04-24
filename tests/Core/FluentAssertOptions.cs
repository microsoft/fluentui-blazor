using AngleSharp.Diffing.Core;
using AngleSharp.Dom;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests;

public class FluentAssertOptions
{
    /// <summary>
    /// Gets or sets default file extension used to identify an expected HTML file,
    /// generated from CS tests.
    /// </summary>
    public string VerifiedCSharpExtension { get; set; } = ".verified.html";

    /// <summary>
    /// Gets or sets default file extension used to save a Card generated HTML file,
    /// generated from CS tests.
    /// </summary>
    public string ReceivedCSharpExtension { get; set; } = ".received.html";

    /// <summary>
    /// Gets or sets default file extension used to identify an expected HTML file,
    /// generated from RAZOR tests.
    /// </summary>
    public string VerifiedRazorExtension { get; set; } = ".verified.razor.html";

    /// <summary>
    /// Gets or sets default file extension used to save a Card generated JSON file,
    /// generated from RAZOR tests.
    /// </summary>
    public string ReceivedRazorExtension { get; set; } = ".received.razor.html";

    /// <summary>
    /// True to update the verified files with the received files.
    /// No exceptions will be thrown.
    /// </summary>
    public bool UpdateVerifiedFiles => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("UPDATE_VERIFIED_FILES"));

    /// <summary>
    /// Scrub lines with an optional replace.
    /// Can return the input to ignore the line, or return a a different string to replace it.
    /// </summary>
    public string ScrubLinesWithReplace(string content)
    {
        return content.ReplaceAttribute("id", "xxx")
                      .ReplaceAttribute("name", "xxx")
                      .ReplaceAttribute("for", "xxx")
                      .ReplaceAttribute("ForId", "xxx")
                      .ReplaceAttribute("blazor:elementreference", "xxx")
                      .ReplaceAttribute("anchor", "xxx");
    }

    /// <summary>
    /// Check if the <paramref name="item"/> must be excluded from comparison errors.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool IsExcluded(IDiff item)
    {
        // Exclude Id attribute
        switch (item)
        {
            case AttrDiff attrDiff:
                return (attrDiff.Control.Attribute as Attr)?.IsId == true;

            case UnexpectedAttrDiff unexpectedAttrDiff:
                return (unexpectedAttrDiff.Test.Attribute as Attr)?.IsId == true;
        }

        return false;
    }
}
