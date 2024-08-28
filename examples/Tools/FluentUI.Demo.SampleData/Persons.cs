// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Demo.SampleData.Resources;

namespace FluentUI.Demo.SampleData;

/// <summary>
/// 
/// </summary>
public partial class Persons
{
    private const string IMG_PREFIX_JPG = "data:image/jpeg;base64,";

    /// <summary>
    /// Gets a list of 8 Face pictures, embedded in the library.
    /// </summary>
    public static IEnumerable<string> FacesForImg
    {
        get
        {
            yield return IMG_PREFIX_JPG + ResourceReader.EmbeddedPicture("Face1.jpg").ToBase64();
            yield return IMG_PREFIX_JPG + ResourceReader.EmbeddedPicture("Face2.jpg").ToBase64();
            yield return IMG_PREFIX_JPG + ResourceReader.EmbeddedPicture("Face3.jpg").ToBase64();
            yield return IMG_PREFIX_JPG + ResourceReader.EmbeddedPicture("Face4.jpg").ToBase64();
            yield return IMG_PREFIX_JPG + ResourceReader.EmbeddedPicture("Face5.jpg").ToBase64();
            yield return IMG_PREFIX_JPG + ResourceReader.EmbeddedPicture("Face6.jpg").ToBase64();
            yield return IMG_PREFIX_JPG + ResourceReader.EmbeddedPicture("Face7.jpg").ToBase64();
            yield return IMG_PREFIX_JPG + ResourceReader.EmbeddedPicture("Face8.jpg").ToBase64();
        }
    }
}
