// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.SampleData;

/// <summary>
/// Paris 2024 Olympics Medals
/// </summary>
/// <remarks>
/// Source: <see href="https://www.kaggle.com/datasets/berkayalan/paris-2024-olympics-medals" />, licensed as <see href="https://www.mit.edu/~amini/LICENSE.md" />
/// </remarks>
public partial class Olympics2024
{
    /// <summary>
    /// Gets a list of Countries and their medals
    /// </summary>
    public static IEnumerable<Country> Countries
    {
        get
        {
            yield return new Country("al", "Albania", new Medals(Gold: 0, Silver: 0, Bronze: 2));
            yield return new Country("dz", "Algeria", new Medals(Gold: 2, Silver: 0, Bronze: 1));
            yield return new Country("ar", "Argentina", new Medals(Gold: 1, Silver: 1, Bronze: 1));
            yield return new Country("am", "Armenia", new Medals(Gold: 0, Silver: 3, Bronze: 1));
            yield return new Country("au", "Australia", new Medals(Gold: 18, Silver: 19, Bronze: 16));
            yield return new Country("at", "Austria", new Medals(Gold: 2, Silver: 0, Bronze: 3));
            yield return new Country("az", "Azerbaijan", new Medals(Gold: 2, Silver: 2, Bronze: 3));
            yield return new Country("be", "Belgium", new Medals(Gold: 3, Silver: 1, Bronze: 6));
            yield return new Country("bh", "Bahrain", new Medals(Gold: 2, Silver: 1, Bronze: 1));
            yield return new Country("bw", "Botswana", new Medals(Gold: 1, Silver: 1, Bronze: 0));
            yield return new Country("br", "Brazil", new Medals(Gold: 3, Silver: 7, Bronze: 10));
            yield return new Country("bg", "Bulgaria", new Medals(Gold: 3, Silver: 1, Bronze: 3));
            yield return new Country("ca", "Canada", new Medals(Gold: 9, Silver: 7, Bronze: 11));
            yield return new Country("cl", "Chile", new Medals(Gold: 1, Silver: 1, Bronze: 0));
            yield return new Country("cn", "People's Republic of China", new Medals(Gold: 40, Silver: 27, Bronze: 24));
            yield return new Country("ci", "Ivory Coast", new Medals(Gold: 0, Silver: 0, Bronze: 1));
            yield return new Country("co", "Colombia", new Medals(Gold: 0, Silver: 3, Bronze: 1));
            yield return new Country("cv", "Cape Verde", new Medals(Gold: 0, Silver: 0, Bronze: 1));
            yield return new Country("hr", "Croatia", new Medals(Gold: 2, Silver: 2, Bronze: 3));
            yield return new Country("cu", "Cuba", new Medals(Gold: 2, Silver: 1, Bronze: 6));
            yield return new Country("cy", "Cyprus", new Medals(Gold: 0, Silver: 1, Bronze: 0));
            yield return new Country("cz", "Czech Republic", new Medals(Gold: 3, Silver: 0, Bronze: 2));
            yield return new Country("dk", "Denmark", new Medals(Gold: 2, Silver: 2, Bronze: 5));
            yield return new Country("dm", "Dominica", new Medals(Gold: 1, Silver: 0, Bronze: 0));
            yield return new Country("do", "Dominican Republic", new Medals(Gold: 1, Silver: 0, Bronze: 2));
            yield return new Country("ec", "Ecuador", new Medals(Gold: 1, Silver: 2, Bronze: 2));
            yield return new Country("eg", "Egypt", new Medals(Gold: 1, Silver: 1, Bronze: 1));
            yield return new Country("xx", "Refugee Olympic Team", new Medals(Gold: 0, Silver: 0, Bronze: 1));
            yield return new Country("et", "Ethiopia", new Medals(Gold: 1, Silver: 3, Bronze: 0));
            yield return new Country("fj", "Fiji", new Medals(Gold: 0, Silver: 1, Bronze: 0));
            yield return new Country("fr", "France", new Medals(Gold: 16, Silver: 26, Bronze: 22));
            yield return new Country("gb", "Great Britain", new Medals(Gold: 14, Silver: 22, Bronze: 29));
            yield return new Country("ge", "Georgia", new Medals(Gold: 3, Silver: 3, Bronze: 1));
            yield return new Country("de", "Germany", new Medals(Gold: 12, Silver: 13, Bronze: 8));
            yield return new Country("gr", "Greece", new Medals(Gold: 1, Silver: 1, Bronze: 6));
            yield return new Country("gd", "Grenada", new Medals(Gold: 0, Silver: 0, Bronze: 2));
            yield return new Country("gt", "Guatemala", new Medals(Gold: 1, Silver: 0, Bronze: 1));
            yield return new Country("hk", "Hong Kong", new Medals(Gold: 2, Silver: 0, Bronze: 2));
            yield return new Country("hu", "Hungary", new Medals(Gold: 6, Silver: 7, Bronze: 6));
            yield return new Country("id", "Indonesia", new Medals(Gold: 2, Silver: 0, Bronze: 1));
            yield return new Country("in", "India", new Medals(Gold: 0, Silver: 1, Bronze: 5));
            yield return new Country("ie", "Ireland", new Medals(Gold: 4, Silver: 0, Bronze: 3));
            yield return new Country("ir", "Iran", new Medals(Gold: 3, Silver: 6, Bronze: 3));
            yield return new Country("il", "Israel", new Medals(Gold: 1, Silver: 5, Bronze: 1));
            yield return new Country("it", "Italy", new Medals(Gold: 12, Silver: 13, Bronze: 15));
            yield return new Country("jm", "Jamaica", new Medals(Gold: 1, Silver: 3, Bronze: 2));
            yield return new Country("jo", "Jordan", new Medals(Gold: 0, Silver: 1, Bronze: 0));
            yield return new Country("jp", "Japan", new Medals(Gold: 20, Silver: 12, Bronze: 13));
            yield return new Country("kz", "Kazakhstan", new Medals(Gold: 1, Silver: 3, Bronze: 3));
            yield return new Country("ke", "Kenya", new Medals(Gold: 4, Silver: 2, Bronze: 5));
            yield return new Country("kg", "Kyrgyzstan", new Medals(Gold: 0, Silver: 2, Bronze: 4));
            yield return new Country("kr", "South Korea", new Medals(Gold: 13, Silver: 9, Bronze: 10));
            yield return new Country("xk", "Kosovo", new Medals(Gold: 0, Silver: 1, Bronze: 1));
            yield return new Country("lc", "St Lucia", new Medals(Gold: 1, Silver: 1, Bronze: 0));
            yield return new Country("lt", "Lithuania", new Medals(Gold: 0, Silver: 2, Bronze: 2));
            yield return new Country("my", "Malaysia", new Medals(Gold: 0, Silver: 0, Bronze: 2));
            yield return new Country("md", "Moldova", new Medals(Gold: 0, Silver: 1, Bronze: 3));
            yield return new Country("mx", "Mexico", new Medals(Gold: 0, Silver: 3, Bronze: 2));
            yield return new Country("mn", "Mongolia", new Medals(Gold: 0, Silver: 1, Bronze: 0));
            yield return new Country("ma", "Morocco", new Medals(Gold: 1, Silver: 0, Bronze: 1));
            yield return new Country("nl", "Netherlands", new Medals(Gold: 15, Silver: 7, Bronze: 12));
            yield return new Country("no", "Norway", new Medals(Gold: 4, Silver: 1, Bronze: 3));
            yield return new Country("nz", "New Zealand", new Medals(Gold: 10, Silver: 7, Bronze: 3));
            yield return new Country("pa", "Panama", new Medals(Gold: 0, Silver: 1, Bronze: 0));
            yield return new Country("pe", "Peru", new Medals(Gold: 0, Silver: 0, Bronze: 1));
            yield return new Country("ph", "Philippines", new Medals(Gold: 2, Silver: 0, Bronze: 2));
            yield return new Country("pk", "Pakistan", new Medals(Gold: 1, Silver: 0, Bronze: 0));
            yield return new Country("pl", "Poland", new Medals(Gold: 1, Silver: 4, Bronze: 5));
            yield return new Country("pt", "Portugal", new Medals(Gold: 1, Silver: 2, Bronze: 1));
            yield return new Country("kp", "North Korea", new Medals(Gold: 0, Silver: 2, Bronze: 4));
            yield return new Country("pr", "Puerto Rico", new Medals(Gold: 0, Silver: 0, Bronze: 2));
            yield return new Country("qa", "Qatar", new Medals(Gold: 0, Silver: 0, Bronze: 1));
            yield return new Country("ro", "Romania", new Medals(Gold: 3, Silver: 4, Bronze: 2));
            yield return new Country("za", "South Africa", new Medals(Gold: 1, Silver: 3, Bronze: 2));
            yield return new Country("rs", "Serbia", new Medals(Gold: 3, Silver: 1, Bronze: 1));
            yield return new Country("sg", "Singapore", new Medals(Gold: 0, Silver: 0, Bronze: 1));
            yield return new Country("si", "Slovenia", new Medals(Gold: 2, Silver: 1, Bronze: 0));
            yield return new Country("es", "Spain", new Medals(Gold: 5, Silver: 4, Bronze: 9));
            yield return new Country("sk", "Slovakia", new Medals(Gold: 0, Silver: 0, Bronze: 1));
            yield return new Country("se", "Sweden", new Medals(Gold: 4, Silver: 4, Bronze: 3));
            yield return new Country("ch", "Switzerland", new Medals(Gold: 1, Silver: 2, Bronze: 5));
            yield return new Country("th", "Thailand", new Medals(Gold: 1, Silver: 3, Bronze: 2));
            yield return new Country("tj", "Tajikistan", new Medals(Gold: 0, Silver: 0, Bronze: 3));
            yield return new Country("tpe", "Chinese Taipei", new Medals(Gold: 2, Silver: 0, Bronze: 5));
            yield return new Country("tn", "Tunisia", new Medals(Gold: 1, Silver: 1, Bronze: 1));
            yield return new Country("tr", "Turkey", new Medals(Gold: 0, Silver: 3, Bronze: 5));
            yield return new Country("ug", "Uganda", new Medals(Gold: 1, Silver: 1, Bronze: 0));
            yield return new Country("ua", "Ukraine", new Medals(Gold: 3, Silver: 5, Bronze: 4));
            yield return new Country("us", "United States of America", new Medals(Gold: 40, Silver: 44, Bronze: 42));
            yield return new Country("uz", "Uzbekistan", new Medals(Gold: 8, Silver: 2, Bronze: 3));
            yield return new Country("zm", "Zambia", new Medals(Gold: 0, Silver: 0, Bronze: 1));
        }
    }

    /// <summary>
    /// Represents a country with its code, name, and medals.
    /// </summary>
    /// <param name="Code">The country's code.</param>
    /// <param name="Name">The country's name.</param>
    /// <param name="Medals">The medals won by the country.</param>
    public record Country(string Code, string Name, Medals Medals);

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
