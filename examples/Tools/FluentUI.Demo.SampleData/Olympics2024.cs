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
            yield return new Country("ALB", "Albania", new Medals(Gold: 0, Silver: 0, Bronze: 2));
            yield return new Country("ALG", "Algeria", new Medals(Gold: 2, Silver: 0, Bronze: 1));
            yield return new Country("ARG", "Argentina", new Medals(Gold: 1, Silver: 1, Bronze: 1));
            yield return new Country("ARM", "Armenia", new Medals(Gold: 0, Silver: 3, Bronze: 1));
            yield return new Country("AUS", "Australia", new Medals(Gold: 18, Silver: 19, Bronze: 16));
            yield return new Country("AUT", "Austria", new Medals(Gold: 2, Silver: 0, Bronze: 3));
            yield return new Country("AZE", "Azerbaijan", new Medals(Gold: 2, Silver: 2, Bronze: 3));
            yield return new Country("BHR", "Bahrain", new Medals(Gold: 2, Silver: 1, Bronze: 1));
            yield return new Country("BEL", "Belgium", new Medals(Gold: 3, Silver: 1, Bronze: 6));
            yield return new Country("BOT", "Botswana", new Medals(Gold: 1, Silver: 1, Bronze: 0));
            yield return new Country("BRZ", "Brazil", new Medals(Gold: 3, Silver: 7, Bronze: 10));
            yield return new Country("BUL", "Bulgaria", new Medals(Gold: 3, Silver: 1, Bronze: 3));
            yield return new Country("CAN", "Canada", new Medals(Gold: 9, Silver: 7, Bronze: 11));
            yield return new Country("CPV", "Cape Verde", new Medals(Gold: 0, Silver: 0, Bronze: 1));
            yield return new Country("CHI", "Chile", new Medals(Gold: 1, Silver: 1, Bronze: 0));
            yield return new Country("CHN", "China", new Medals(Gold: 40, Silver: 27, Bronze: 24));
            yield return new Country("TPE", "Chinese Taipei", new Medals(Gold: 2, Silver: 0, Bronze: 5));
            yield return new Country("COL", "Colombia", new Medals(Gold: 0, Silver: 3, Bronze: 1));
            yield return new Country("CRO", "Croatia", new Medals(Gold: 2, Silver: 2, Bronze: 3));
            yield return new Country("CUB", "Cuba", new Medals(Gold: 2, Silver: 1, Bronze: 6));
            yield return new Country("CYP", "Cyprus", new Medals(Gold: 0, Silver: 1, Bronze: 0));
            yield return new Country("CZE", "Czech Republic", new Medals(Gold: 3, Silver: 0, Bronze: 2));
            yield return new Country("DEN", "Denmark", new Medals(Gold: 2, Silver: 2, Bronze: 5));
            yield return new Country("DMA", "Dominica", new Medals(Gold: 1, Silver: 0, Bronze: 0));
            yield return new Country("DOM", "Dominican Republic", new Medals(Gold: 1, Silver: 0, Bronze: 2));
            yield return new Country("ECU", "Ecuador", new Medals(Gold: 1, Silver: 2, Bronze: 2));
            yield return new Country("EGY", "Egypt", new Medals(Gold: 1, Silver: 1, Bronze: 1));
            yield return new Country("ETH", "Ethiopia", new Medals(Gold: 1, Silver: 3, Bronze: 0));
            yield return new Country("FIJ", "Fiji", new Medals(Gold: 0, Silver: 1, Bronze: 0));
            yield return new Country("FRA", "France", new Medals(Gold: 16, Silver: 26, Bronze: 22));
            yield return new Country("GEO", "Georgia", new Medals(Gold: 3, Silver: 3, Bronze: 1));
            yield return new Country("GER", "Germany", new Medals(Gold: 12, Silver: 13, Bronze: 8));
            yield return new Country("GBG", "Great Britain", new Medals(Gold: 14, Silver: 22, Bronze: 29));
            yield return new Country("GRE", "Greece", new Medals(Gold: 1, Silver: 1, Bronze: 6));
            yield return new Country("GRN", "Grenada", new Medals(Gold: 0, Silver: 0, Bronze: 2));
            yield return new Country("GUA", "Guatemala", new Medals(Gold: 1, Silver: 0, Bronze: 1));
            yield return new Country("HK", "Hong Kong", new Medals(Gold: 2, Silver: 0, Bronze: 2));
            yield return new Country("HUN", "Hungary", new Medals(Gold: 6, Silver: 7, Bronze: 6));
            yield return new Country("IND", "India", new Medals(Gold: 0, Silver: 1, Bronze: 5));
            yield return new Country("IDN", "Indonesia", new Medals(Gold: 2, Silver: 0, Bronze: 1));
            yield return new Country("IRN", "Iran", new Medals(Gold: 3, Silver: 6, Bronze: 3));
            yield return new Country("IRE", "Ireland", new Medals(Gold: 4, Silver: 0, Bronze: 3));
            yield return new Country("ISR", "Israel", new Medals(Gold: 1, Silver: 5, Bronze: 1));
            yield return new Country("ITA", "Italy", new Medals(Gold: 12, Silver: 13, Bronze: 15));
            yield return new Country("CIV", "Ivory Coast", new Medals(Gold: 0, Silver: 0, Bronze: 1));
            yield return new Country("JAM", "Jamaica", new Medals(Gold: 1, Silver: 3, Bronze: 2));
            yield return new Country("JPN", "Japan", new Medals(Gold: 20, Silver: 12, Bronze: 13));
            yield return new Country("JOR", "Jordan", new Medals(Gold: 0, Silver: 1, Bronze: 0));
            yield return new Country("KAZ", "Kazakhstan", new Medals(Gold: 1, Silver: 3, Bronze: 3));
            yield return new Country("KEN", "Kenya", new Medals(Gold: 4, Silver: 2, Bronze: 5));
            yield return new Country("KOS", "Kosovo", new Medals(Gold: 0, Silver: 1, Bronze: 1));
            yield return new Country("KGZ", "Kyrgyzstan", new Medals(Gold: 0, Silver: 2, Bronze: 4));
            yield return new Country("LTU", "Lithuania", new Medals(Gold: 0, Silver: 2, Bronze: 2));
            yield return new Country("MAS", "Malaysia", new Medals(Gold: 0, Silver: 0, Bronze: 2));
            yield return new Country("MEX", "Mexico", new Medals(Gold: 0, Silver: 3, Bronze: 2));
            yield return new Country("MDA", "Moldova", new Medals(Gold: 0, Silver: 1, Bronze: 3));
            yield return new Country("MGL", "Mongolia", new Medals(Gold: 0, Silver: 1, Bronze: 0));
            yield return new Country("MOR", "Morocco", new Medals(Gold: 1, Silver: 0, Bronze: 1));
            yield return new Country("NED", "Netherlands", new Medals(Gold: 15, Silver: 7, Bronze: 12));
            yield return new Country("NZ", "New Zealand", new Medals(Gold: 10, Silver: 7, Bronze: 3));
            yield return new Country("PRK", "North Korea", new Medals(Gold: 0, Silver: 2, Bronze: 4));
            yield return new Country("NOR", "Norway", new Medals(Gold: 4, Silver: 1, Bronze: 3));
            yield return new Country("PKN", "Pakistan", new Medals(Gold: 1, Silver: 0, Bronze: 0));
            yield return new Country("PAN", "Panama", new Medals(Gold: 0, Silver: 1, Bronze: 0));
            yield return new Country("PER", "Peru", new Medals(Gold: 0, Silver: 0, Bronze: 1));
            yield return new Country("PHI", "Philippines", new Medals(Gold: 2, Silver: 0, Bronze: 2));
            yield return new Country("POL", "Poland", new Medals(Gold: 1, Silver: 4, Bronze: 5));
            yield return new Country("POR", "Portugal", new Medals(Gold: 1, Silver: 2, Bronze: 1));
            yield return new Country("PUR", "Puerto Rico", new Medals(Gold: 0, Silver: 0, Bronze: 2));
            yield return new Country("QAT", "Qatar", new Medals(Gold: 0, Silver: 0, Bronze: 1));
            yield return new Country("EOR", "Refugee Olympic Team", new Medals(Gold: 0, Silver: 0, Bronze: 1));
            yield return new Country("ROM", "Romania", new Medals(Gold: 3, Silver: 4, Bronze: 2));
            yield return new Country("SER", "Serbia", new Medals(Gold: 3, Silver: 1, Bronze: 1));
            yield return new Country("SIN", "Singapore", new Medals(Gold: 0, Silver: 0, Bronze: 1));
            yield return new Country("SVK", "Slovakia", new Medals(Gold: 0, Silver: 0, Bronze: 1));
            yield return new Country("SLO", "Slovenia", new Medals(Gold: 2, Silver: 1, Bronze: 0));
            yield return new Country("SA", "South Africa", new Medals(Gold: 1, Silver: 3, Bronze: 2));
            yield return new Country("KOR", "South Korea", new Medals(Gold: 13, Silver: 9, Bronze: 10));
            yield return new Country("SPA", "Spain", new Medals(Gold: 5, Silver: 4, Bronze: 9));
            yield return new Country("LCA", "St Lucia", new Medals(Gold: 1, Silver: 1, Bronze: 0));
            yield return new Country("SWE", "Sweden", new Medals(Gold: 4, Silver: 4, Bronze: 3));
            yield return new Country("SWI", "Switzerland", new Medals(Gold: 1, Silver: 2, Bronze: 5));
            yield return new Country("TJK", "Tajikistan", new Medals(Gold: 0, Silver: 0, Bronze: 3));
            yield return new Country("THA", "Thailand", new Medals(Gold: 1, Silver: 3, Bronze: 2));
            yield return new Country("TUN", "Tunisia", new Medals(Gold: 1, Silver: 1, Bronze: 1));
            yield return new Country("TUR", "Turkey", new Medals(Gold: 0, Silver: 3, Bronze: 5));
            yield return new Country("UGA", "Uganda", new Medals(Gold: 1, Silver: 1, Bronze: 0));
            yield return new Country("UKR", "Ukraine", new Medals(Gold: 3, Silver: 5, Bronze: 4));
            yield return new Country("US", "United States", new Medals(Gold: 40, Silver: 44, Bronze: 42));
            yield return new Country("UZB", "Uzbekistan", new Medals(Gold: 8, Silver: 2, Bronze: 3));
            yield return new Country("ZAM", "Zambia", new Medals(Gold: 0, Silver: 0, Bronze: 1));
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
}
