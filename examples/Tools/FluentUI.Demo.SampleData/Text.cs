// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Text;

namespace FluentUI.Demo.SampleData;

/// <summary>
/// Returns a list of pages with random sections and Lorem Ipsum text.
/// </summary>
public partial class Text
{
    private static readonly string[] LoremIpsumWords = Resources.ResourceReader.EmbeddedText("LoremIpsum.txt").Split(';');
    private static readonly string[] SectionItems = Resources.ResourceReader.EmbeddedText("Sections.txt").Split(';');

    /// <summary>
    /// Gets a list of pages with random sections and Lorem Ipsum text.
    /// </summary>
    public static IEnumerable<Page> Pages
    {
        get
        {
            var random = new Random();

            foreach (var section in SectionItems)
            {
                yield return new Page(section, GenerateLoremIpsum(random.Next(1, 5), random.Next(30, 200)));
            }
        }
    }

    /// <summary>
    /// Gets a list of section titles.
    /// </summary>
    public static IEnumerable<string> Titles => SectionItems;

    /// <summary>
    /// Gets a list of 15 Lorem Ipsum texts.
    /// </summary>
    public static IEnumerable<string> LoremIpsum
    {
        get
        {
            yield return "Lorem Ipsum " + GenerateLoremIpsum(1, 30);
            yield return GenerateLoremIpsum(1, 110);
            yield return GenerateLoremIpsum(1, 250);
            yield return GenerateLoremIpsum(1, 35);
            yield return GenerateLoremIpsum(1, 20);
            yield return GenerateLoremIpsum(1, 170);
            yield return GenerateLoremIpsum(1, 12);
            yield return GenerateLoremIpsum(1, 24);
            yield return GenerateLoremIpsum(1, 31);
            yield return GenerateLoremIpsum(1, 27);
            yield return GenerateLoremIpsum(1, 150);
            yield return GenerateLoremIpsum(1, 35);
            yield return GenerateLoremIpsum(1, 20);
            yield return GenerateLoremIpsum(1, 17);
            yield return GenerateLoremIpsum(1, 12);
        }
    }

    /// <summary>
    /// Generates a Lorem Ipsum text with the specified number of paragraphs and words per paragraph.
    /// </summary>
    /// <param name="paragraphCount"></param>
    /// <param name="wordsPerParagraph"></param>
    /// <returns></returns>
    public static string GenerateLoremIpsum(int paragraphCount = 3, int wordsPerParagraph = 50)
    {
        var loremIpsumText = new StringBuilder();
        var random = new Random();
        var LoremIpsumLength = LoremIpsumWords.Length;

        for (var i = 0; i < paragraphCount; i++)
        {
            for (var j = 0; j < wordsPerParagraph; j++)
            {
                var word = LoremIpsumWords[random.Next(LoremIpsumLength)];
                loremIpsumText.Append(word + " ");
            }

            loremIpsumText.AppendLine();
        }

        return loremIpsumText.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Title"></param>
    /// <param name="Content"></param>
    public record Page(string Title, string Content);
}
