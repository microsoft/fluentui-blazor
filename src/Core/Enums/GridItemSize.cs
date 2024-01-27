namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Size available for <see cref="FluentGrid"/> when a Resize event raised.
/// </summary>
[Flags]
public enum GridItemSize
{
    /// <summary>
    /// Browser size less than 599px
    /// </summary>
    Xs = 1,

    /// <summary>
    /// Browser size between 600px and 959px
    /// </summary>
    Sm = 2,

    /// <summary>
    /// Browser size between 960px and 1279px
    /// </summary>
    Md = 4,

    /// <summary>
    /// Browser size between 1280px and 1919px
    /// </summary>
    Lg = 8,

    /// <summary>
    /// Browser size between 1920px and 2559px
    /// </summary>
    Xl = 16,

    /// <summary>
    /// Browser size greater  than 2560px
    /// </summary>
    Xxl = 32,
}
