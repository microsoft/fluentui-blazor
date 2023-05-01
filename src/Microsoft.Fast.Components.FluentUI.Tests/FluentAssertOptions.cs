using AngleSharp.Diffing.Core;
using AngleSharp.Dom;

namespace Microsoft.Fast.Components.FluentUI.Tests;

public class FluentAssertOptions
{
    /// <summary>
    /// Gets or sets default file extension used to identify an expected JSON file.
    /// </summary>
    public string VerifiedExtension { get; set; } = ".verified.html";

    /// <summary>
    /// Gets or sets default file extension used to save a Card generated JSON file.
    /// </summary>
    public string ReceivedExtension { get; set; } = ".received.html";

    /// <summary>
    /// Check if the <paramref name="item"/> must be excluded from comparison errors.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool IsExcluded(IDiff item)
    {
        // Exclude Id attribute
        if (item.Target == DiffTarget.Attribute)
        {
            var attr = (AttrDiff)item;
            return (attr.Control.Attribute as Attr)?.IsId == true;
        }

        return false;
    }
}