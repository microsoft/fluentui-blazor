// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.SampleData.Resources;

namespace FluentUI.Demo.SampleData;

/// <summary>
/// Paris 2024 Olympics Medals
/// </summary>
/// <remarks>
/// Source: <see href="https://www.kaggle.com/datasets/berkayalan/paris-2024-olympics-medals" />, licensed as <see href="https://www.mit.edu/~amini/LICENSE.md" />
/// </remarks>
public partial class Olympics2024
{
    private const string IMG_PREFIX_SVG = "data:image/png;base64,";

    /// <summary>
    /// Gets an array of entities (reusing <see cref="Country"/> type) representing each continent, with medal counts initialized to zero.
    /// </summary>
    /// <remarks>Each element in the array is a Country object corresponding to a continent, identified by its
    /// code and name. The Medals property for each continent is initialized with zero gold, silver, and bronze
    /// medals.</remarks>
    public static IEnumerable<Country> Continents
    {
        get
        {
            yield return new Country("AF", "Africa", null, new Medals(Gold: 0, Silver: 0, Bronze: 0));
            yield return new Country("AS", "Asia", null, new Medals(Gold: 0, Silver: 0, Bronze: 0));
            yield return new Country("EU", "Europe", null, new Medals(Gold: 0, Silver: 0, Bronze: 0));
            yield return new Country("NA", "North America", null, new Medals(Gold: 0, Silver: 0, Bronze: 0));
            yield return new Country("OC", "Oceania", null, new Medals(Gold: 0, Silver: 0, Bronze: 0));
            yield return new Country("SA", "South America", null, new Medals(Gold: 0, Silver: 0, Bronze: 0));
        }
    }

    /// <summary>
    /// Gets a list of Countries and their medals
    /// </summary>
    public static IEnumerable<Country> Countries
    {
        get
        {
            yield return new Country("al", "Albania", "EU", new Medals(Gold: 0, Silver: 0, Bronze: 2));
            yield return new Country("dz", "Algeria", "AF", new Medals(Gold: 2, Silver: 0, Bronze: 1));
            yield return new Country("ar", "Argentina", "SA", new Medals(Gold: 1, Silver: 1, Bronze: 1));
            yield return new Country("am", "Armenia", "EU", new Medals(Gold: 0, Silver: 3, Bronze: 1));
            yield return new Country("au", "Australia", "OC", new Medals(Gold: 18, Silver: 19, Bronze: 16));
            yield return new Country("at", "Austria", "EU", new Medals(Gold: 2, Silver: 0, Bronze: 3));
            yield return new Country("az", "Azerbaijan", "EU", new Medals(Gold: 2, Silver: 2, Bronze: 3));
            yield return new Country("be", "Belgium", "EU", new Medals(Gold: 3, Silver: 1, Bronze: 6));
            yield return new Country("bh", "Bahrain", "AS", new Medals(Gold: 2, Silver: 1, Bronze: 1));
            yield return new Country("bw", "Botswana", "AF", new Medals(Gold: 1, Silver: 1, Bronze: 0));
            yield return new Country("br", "Brazil", "SA", new Medals(Gold: 3, Silver: 7, Bronze: 10));
            yield return new Country("bg", "Bulgaria", "EU", new Medals(Gold: 3, Silver: 1, Bronze: 3));
            yield return new Country("ca", "Canada", "NA", new Medals(Gold: 9, Silver: 7, Bronze: 11));
            yield return new Country("cl", "Chile", "SA", new Medals(Gold: 1, Silver: 1, Bronze: 0));
            yield return new Country("cn", "People's Republic of China", "AS", new Medals(Gold: 40, Silver: 27, Bronze: 24));
            yield return new Country("ci", "Ivory Coast", "AF", new Medals(Gold: 0, Silver: 0, Bronze: 1));
            yield return new Country("co", "Colombia", "SA", new Medals(Gold: 0, Silver: 3, Bronze: 1));
            yield return new Country("cv", "Cape Verde", "AF", new Medals(Gold: 0, Silver: 0, Bronze: 1));
            yield return new Country("hr", "Croatia", "EU", new Medals(Gold: 2, Silver: 2, Bronze: 3));
            yield return new Country("cu", "Cuba", "NA", new Medals(Gold: 2, Silver: 1, Bronze: 6));
            yield return new Country("cy", "Cyprus", "EU", new Medals(Gold: 0, Silver: 1, Bronze: 0));
            yield return new Country("cz", "Czech Republic", "EU", new Medals(Gold: 3, Silver: 0, Bronze: 2));
            yield return new Country("dk", "Denmark", "EU", new Medals(Gold: 2, Silver: 2, Bronze: 5));
            yield return new Country("dm", "Dominica", "NA", new Medals(Gold: 1, Silver: 0, Bronze: 0));
            yield return new Country("do", "Dominican Republic", "NA", new Medals(Gold: 1, Silver: 0, Bronze: 2));
            yield return new Country("ec", "Ecuador", "SA", new Medals(Gold: 1, Silver: 2, Bronze: 2));
            yield return new Country("eg", "Egypt", "AF", new Medals(Gold: 1, Silver: 1, Bronze: 1));
            yield return new Country("xx", "Refugee Olympic Team", "XX", new Medals(Gold: 0, Silver: 0, Bronze: 1));
            yield return new Country("et", "Ethiopia", "AF", new Medals(Gold: 1, Silver: 3, Bronze: 0));
            yield return new Country("fj", "Fiji", "OC", new Medals(Gold: 0, Silver: 1, Bronze: 0));
            yield return new Country("fr", "France", "EU", new Medals(Gold: 16, Silver: 26, Bronze: 22));
            yield return new Country("gb", "Great Britain", "EU", new Medals(Gold: 14, Silver: 22, Bronze: 29));
            yield return new Country("ge", "Georgia", "EU", new Medals(Gold: 3, Silver: 3, Bronze: 1));
            yield return new Country("de", "Germany", "EU", new Medals(Gold: 12, Silver: 13, Bronze: 8));
            yield return new Country("gr", "Greece", "EU", new Medals(Gold: 1, Silver: 1, Bronze: 6));
            yield return new Country("gd", "Grenada", "NA", new Medals(Gold: 0, Silver: 0, Bronze: 2));
            yield return new Country("gt", "Guatemala", "NA", new Medals(Gold: 1, Silver: 0, Bronze: 1));
            yield return new Country("hk", "Hong Kong", "AS", new Medals(Gold: 2, Silver: 0, Bronze: 2));
            yield return new Country("hu", "Hungary", "EU", new Medals(Gold: 6, Silver: 7, Bronze: 6));
            yield return new Country("id", "Indonesia", "AS", new Medals(Gold: 2, Silver: 0, Bronze: 1));
            yield return new Country("in", "India", "AS", new Medals(Gold: 0, Silver: 1, Bronze: 5));
            yield return new Country("ie", "Ireland", "EU", new Medals(Gold: 4, Silver: 0, Bronze: 3));
            yield return new Country("ir", "Iran", "AS", new Medals(Gold: 3, Silver: 6, Bronze: 3));
            yield return new Country("il", "Israel", "AS", new Medals(Gold: 1, Silver: 5, Bronze: 1));
            yield return new Country("it", "Italy", "EU", new Medals(Gold: 12, Silver: 13, Bronze: 15));
            yield return new Country("jm", "Jamaica", "NA", new Medals(Gold: 1, Silver: 3, Bronze: 2));
            yield return new Country("jo", "Jordan", "AS", new Medals(Gold: 0, Silver: 1, Bronze: 0));
            yield return new Country("jp", "Japan", "AS", new Medals(Gold: 20, Silver: 12, Bronze: 13));
            yield return new Country("kz", "Kazakhstan", "AS", new Medals(Gold: 1, Silver: 3, Bronze: 3));
            yield return new Country("ke", "Kenya", "AF", new Medals(Gold: 4, Silver: 2, Bronze: 5));
            yield return new Country("kg", "Kyrgyzstan", "AS", new Medals(Gold: 0, Silver: 2, Bronze: 4));
            yield return new Country("kr", "South Korea", "AS", new Medals(Gold: 13, Silver: 9, Bronze: 10));
            yield return new Country("xk", "Kosovo", "EU", new Medals(Gold: 0, Silver: 1, Bronze: 1));
            yield return new Country("lc", "St Lucia", "NA", new Medals(Gold: 1, Silver: 1, Bronze: 0));
            yield return new Country("lt", "Lithuania", "EU", new Medals(Gold: 0, Silver: 2, Bronze: 2));
            yield return new Country("my", "Malaysia", "AS", new Medals(Gold: 0, Silver: 0, Bronze: 2));
            yield return new Country("md", "Moldova", "EU", new Medals(Gold: 0, Silver: 1, Bronze: 3));
            yield return new Country("mx", "Mexico", "NA", new Medals(Gold: 0, Silver: 3, Bronze: 2));
            yield return new Country("mn", "Mongolia", "AS", new Medals(Gold: 0, Silver: 1, Bronze: 0));
            yield return new Country("ma", "Morocco", "AF", new Medals(Gold: 1, Silver: 0, Bronze: 1));
            yield return new Country("nl", "Netherlands", "EU", new Medals(Gold: 15, Silver: 7, Bronze: 12));
            yield return new Country("no", "Norway", "EU", new Medals(Gold: 4, Silver: 1, Bronze: 3));
            yield return new Country("nz", "New Zealand", "OC", new Medals(Gold: 10, Silver: 7, Bronze: 3));
            yield return new Country("pa", "Panama", "NA", new Medals(Gold: 0, Silver: 1, Bronze: 0));
            yield return new Country("pe", "Peru", "SA", new Medals(Gold: 0, Silver: 0, Bronze: 1));
            yield return new Country("ph", "Philippines", "AS", new Medals(Gold: 2, Silver: 0, Bronze: 2));
            yield return new Country("pk", "Pakistan", "AS", new Medals(Gold: 1, Silver: 0, Bronze: 0));
            yield return new Country("pl", "Poland", "EU", new Medals(Gold: 1, Silver: 4, Bronze: 5));
            yield return new Country("pt", "Portugal", "EU", new Medals(Gold: 1, Silver: 2, Bronze: 1));
            yield return new Country("kp", "North Korea", "AS", new Medals(Gold: 0, Silver: 2, Bronze: 4));
            yield return new Country("pr", "Puerto Rico", "NA", new Medals(Gold: 0, Silver: 0, Bronze: 2));
            yield return new Country("qa", "Qatar", "AS", new Medals(Gold: 0, Silver: 0, Bronze: 1));
            yield return new Country("ro", "Romania", "EU", new Medals(Gold: 3, Silver: 4, Bronze: 2));
            yield return new Country("za", "South Africa", "AF", new Medals(Gold: 1, Silver: 3, Bronze: 2));
            yield return new Country("rs", "Serbia", "EU", new Medals(Gold: 3, Silver: 1, Bronze: 1));
            yield return new Country("sg", "Singapore", "AS", new Medals(Gold: 0, Silver: 0, Bronze: 1));
            yield return new Country("si", "Slovenia", "EU", new Medals(Gold: 2, Silver: 1, Bronze: 0));
            yield return new Country("es", "Spain", "EU", new Medals(Gold: 5, Silver: 4, Bronze: 9));
            yield return new Country("sk", "Slovakia", "EU", new Medals(Gold: 0, Silver: 0, Bronze: 1));
            yield return new Country("se", "Sweden", "EU", new Medals(Gold: 4, Silver: 4, Bronze: 3));
            yield return new Country("ch", "Switzerland", "EU", new Medals(Gold: 1, Silver: 2, Bronze: 5));
            yield return new Country("th", "Thailand", "AS", new Medals(Gold: 1, Silver: 3, Bronze: 2));
            yield return new Country("tj", "Tajikistan", "AS", new Medals(Gold: 0, Silver: 0, Bronze: 3));
            yield return new Country("tw", "Chinese Taipei", "AS", new Medals(Gold: 2, Silver: 0, Bronze: 5));
            yield return new Country("tn", "Tunisia", "AF", new Medals(Gold: 1, Silver: 1, Bronze: 1));
            yield return new Country("tr", "Turkey", "EU", new Medals(Gold: 0, Silver: 3, Bronze: 5));
            yield return new Country("ug", "Uganda", "AF", new Medals(Gold: 1, Silver: 1, Bronze: 0));
            yield return new Country("ua", "Ukraine", "EU", new Medals(Gold: 3, Silver: 5, Bronze: 4));
            yield return new Country("us", "United States of America", "NA", new Medals(Gold: 40, Silver: 44, Bronze: 42));
            yield return new Country("uz", "Uzbekistan", "AS", new Medals(Gold: 8, Silver: 2, Bronze: 3));
            yield return new Country("zm", "Zambia", "AF", new Medals(Gold: 0, Silver: 0, Bronze: 1));
        }
    }

    /// <summary>
    /// Represents a country with its code, name, continent code, and medals.
    /// </summary>
    /// <param name="Code">The country's code.</param>
    /// <param name="Name">The country's name.</param>
    /// <param name="ContinentCode">The country's continent code.</param>
    /// <param name="Medals">The medals won by the country.</param>
    public record Country(string Code, string Name, string? ContinentCode, Medals Medals)
    {
        /// <summary>
        /// Gets a the embedded flag for a country
        /// </summary>
        public string Flag() => IMG_PREFIX_SVG + ResourceReader.EmbeddedPicture($"Flags.{Code}.png").ToBase64();
    }

    /// <summary>
    /// Represents a record of medals.
    /// </summary>
    /// <param name="Gold">The number of gold medals.</param>
    /// <param name="Silver">The number of silver medals.</param>
    /// <param name="Bronze">The number of bronze medals.</param>
    public record Medals(int Gold, int Silver, int Bronze)
    {
        /// <summary>
        /// Gets the total number of medals.
        /// </summary>
        public int Total => Gold + Silver + Bronze;
    }

    /// <summary>
    /// Compares countries by the number of gold medals.
    /// </summary>
    public class GoldMedalComparer : IComparer<Country>
    {
        /// <summary>
        /// Compares two countries by the number of gold medals.
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public int Compare(Country? c1, Country? c2)
        {
            if (c1 == null && c2 == null)
            {
                return 0;
            }

            if (c1 == null)
            {
                return -1;
            }

            if (c2 == null)
            {
                return 1;
            }

            return c1.Medals.Silver.CompareTo(c2.Medals.Silver);
        }
    }
}
