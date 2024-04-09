namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Hide the <see cref="FluentGridItem"/> when a size condition is met.
/// </summary>
[Flags]
public enum GridItemHidden
{
    /// <summary>
    /// Never
    /// </summary>
    None = 0,

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

    /// <summary>
    /// Browser size less than 599px
    /// </summary>
    XsAndDown = Xs,

    /// <summary>
    /// Browser size less than 959px
    /// </summary>
    SmAndDown = Xs | Sm,

    /// <summary>
    /// Browser size less than 1279px
    /// </summary>
    MdAndDown = Xs | Sm | Md,

    /// <summary>
    /// Browser size less than 1919px
    /// </summary>
    LgAndDown = Xs | Sm | Md | Lg,

    /// <summary>
    /// Browser size less than 2559px
    /// </summary>
    XlAndDown = Xs | Sm | Md | Lg | Xl,

    /// <summary>
    /// All sizes
    /// </summary>
    XxlAndDown = Xs | Sm | Md | Lg | Xl | Xxl,

    /// <summary>
    /// All sizes
    /// </summary>
    XsAndUp = Xs | Sm | Md | Lg | Xl | Xxl,

    /// <summary>
    /// Browser size greater than 600px
    /// </summary>
    SmAndUp = Sm | Md | Lg | Xl | Xxl,

    /// <summary>
    /// Browser size greater than 960px
    /// </summary>
    MdAndUp = Md | Lg | Xl | Xxl,

    /// <summary>
    /// Browser size greater than 1280px
    /// </summary>
    LgAndUp = Lg | Xl | Xxl,

    /// <summary>
    /// Browser size greater than 1920px
    /// </summary>
    XlAndUp = Xl | Xxl,

    /// <summary>
    /// Browser size greater than 2560px
    /// </summary>
    XxlAndUp = Xxl,
}
