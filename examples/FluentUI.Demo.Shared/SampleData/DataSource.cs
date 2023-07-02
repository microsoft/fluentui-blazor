using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Shared.SampleData;

public class DataSource
{
    private readonly HttpClient _http;
    private readonly NavigationManager _navigationManager;

    public DataSource(HttpClient http, NavigationManager navigationManager)
    {
        _http = http;
        _navigationManager = navigationManager;
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "This is just being used for sample data")]
    public IQueryable<Person> People { get; } = _people.AsQueryable();

    public List<string> Names = _people.Select(p => $"{p.FirstName} {p.LastName}").ToList();

    public List<string> Hits { get; } = new()
    {
        "Please Please Me",
        "With The Beatles",
        "A Hard Day's Night",
        "Beatles for Sale",
        "Help!",
        "Rubber Soul",
        "Revolver",
        "Sgt. Pepper's Lonely Hearts Club Band",
        "Magical Mystery Tour",
        "The Beatles",
        "Yellow Submarine",
        "Abbey Road",
        "Let It Be",
    };

    public List<string> Sizes { get; } = new()
    {
        "Extra small",
        "Small",
        "Medium",
        "Large",
        "Extra Large"
    };



    public Task<Country[]> GetCountriesAsync()
    {
        // In a real application, you'd typically be querying an external data source here.
        // However, since this is a standalone docs site, we'll hardcode the data.
        // The API contract would be the same either way.
        return Task.FromResult(_countries);
    }

    private readonly static Person[] _people = new[]
    {
        new Person ( PersonId: 1, FirstName : "Jean", LastName : "Martin", CountryCode : "fr", BirthDate : new DateOnly(1985, 3, 16) ),
        new Person ( PersonId : 2, FirstName : "António", LastName : "Langa", CountryCode : "mz", BirthDate : new DateOnly(1991, 12, 1) ),
        new Person ( PersonId : 3, FirstName : "Julie", LastName : "Smith", CountryCode : "au", BirthDate : new DateOnly(1958, 10, 10) ),
        new Person ( PersonId : 4, FirstName : "Nur", LastName : "Sari", CountryCode : "id", BirthDate : new DateOnly(1922, 4, 27) ),
        new Person ( PersonId : 5, FirstName : "Jose", LastName : "Hernandez", CountryCode : "mx", BirthDate : new DateOnly(2011, 5, 3) ),
        new Person ( PersonId : 6, FirstName : "Bert", LastName : "de Vries", CountryCode : "nl", BirthDate : new DateOnly(1999, 6, 9) ),
        new Person ( PersonId : 7, FirstName : "Jaques", LastName : "Martin", CountryCode : "fr", BirthDate : new DateOnly(2002, 10, 20) ),
        new Person ( PersonId : 8, FirstName : "Elizabeth", LastName : "Johnson", CountryCode : "gb", BirthDate : new DateOnly(1971, 11, 24) ),
        new Person ( PersonId : 9, FirstName : "Jakob", LastName : "Berger", CountryCode : "de", BirthDate : new DateOnly(1971, 4, 21) )
    };

    public class MonthItem
    {
        public string Index { get; set; } = "00";
        public string Name { get; set; } = "";
        public override string ToString() => $"{Index:00} {Name}";
    }

    public static MonthItem[] AllMonths = Enumerable.Range(0, 12)
                                            .Select(i => new MonthItem
                                            {
                                                Index = $"{i + 1:00}",
                                                Name = GetMonthName(i)
                                            })
                                            .ToArray();

    private static string GetMonthName(int index)
    {
        return System.Globalization
                     .DateTimeFormatInfo
                     .CurrentInfo
                     .MonthNames
                     .ElementAt(index % 12);
    }

    // ToastContent derived from https://www.kaggle.com/datasets/arjunprasadsarkhel/2021-olympics-in-tokyo,
    // which is licensed as https://creativecommons.org/licenses/by-sa/4.0/
    private readonly static Country[] _countries = new[]
    {
        new Country("ar", "Argentina", new Medals { Gold = 0, Silver = 1, Bronze = 2 }),
        new Country("am", "Armenia", new Medals { Gold = 0, Silver = 2, Bronze = 2 }),
        new Country("au", "Australia", new Medals { Gold = 17, Silver = 7, Bronze = 22 }),
        new Country("at", "Austria", new Medals { Gold = 1, Silver = 1, Bronze = 5 }),
        new Country("az", "Azerbaijan", new Medals { Gold = 0, Silver = 3, Bronze = 4 }),
        new Country("bs", "Bahamas", new Medals { Gold = 2, Silver = 0, Bronze = 0 }),
        new Country("bh", "Bahrain", new Medals { Gold = 0, Silver = 1, Bronze = 0 }),
        new Country("by", "Belarus", new Medals { Gold = 1, Silver = 3, Bronze = 3 }),
        new Country("be", "Belgium", new Medals { Gold = 3, Silver = 1, Bronze = 3 }),
        new Country("bm", "Bermuda", new Medals { Gold = 1, Silver = 0, Bronze = 0 }),
        new Country("bw", "Botswana", new Medals { Gold = 0, Silver = 0, Bronze = 1 }),
        new Country("br", "Brazil", new Medals { Gold = 7, Silver = 6, Bronze = 8 }),
        new Country("bg", "Bulgaria", new Medals { Gold = 3, Silver = 1, Bronze = 2 }),
        new Country("bf", "Burkina Faso", new Medals { Gold = 0, Silver = 0, Bronze = 1 }),
        new Country("ca", "Canada", new Medals { Gold = 7, Silver = 6, Bronze = 11 }),
        new Country("tpe", "Chinese Taipei", new Medals { Gold = 2, Silver = 4, Bronze = 6 }),
        new Country("co", "Colombia", new Medals { Gold = 0, Silver = 4, Bronze = 1 }),
        new Country("ci", "Côte d'Ivoire", new Medals { Gold = 0, Silver = 0, Bronze = 1 }),
        new Country("hr", "Croatia", new Medals { Gold = 3, Silver = 3, Bronze = 2 }),
        new Country("cu", "Cuba", new Medals { Gold = 7, Silver = 3, Bronze = 5 }),
        new Country("cz", "Czech Republic", new Medals { Gold = 4, Silver = 4, Bronze = 3 }),
        new Country("dk", "Denmark", new Medals { Gold = 3, Silver = 4, Bronze = 4 }),
        new Country("do", "Dominican Republic", new Medals { Gold = 0, Silver = 3, Bronze = 2 }),
        new Country("ec", "Ecuador", new Medals { Gold = 2, Silver = 1, Bronze = 0 }),
        new Country("eg", "Egypt", new Medals { Gold = 1, Silver = 1, Bronze = 4 }),
        new Country("ee", "Estonia", new Medals { Gold = 1, Silver = 0, Bronze = 1 }),
        new Country("et", "Ethiopia", new Medals { Gold = 1, Silver = 1, Bronze = 2 }),
        new Country("fj", "Fiji", new Medals { Gold = 1, Silver = 0, Bronze = 1 }),
        new Country("fi", "Finland", new Medals { Gold = 0, Silver = 0, Bronze = 2 }),
        new Country("fr", "France", new Medals { Gold = 10, Silver = 12, Bronze = 11 }),
        new Country("ge", "Georgia", new Medals { Gold = 2, Silver = 5, Bronze = 1 }),
        new Country("de", "Germany", new Medals { Gold = 10, Silver = 11, Bronze = 16 }),
        new Country("gh", "Ghana", new Medals { Gold = 0, Silver = 0, Bronze = 1 }),
        new Country("gb", "Great Britain", new Medals { Gold = 22, Silver = 21, Bronze = 22 }),
        new Country("gr", "Greece", new Medals { Gold = 2, Silver = 1, Bronze = 1 }),
        new Country("gd", "Grenada", new Medals { Gold = 0, Silver = 0, Bronze = 1 }),
        new Country("hk", "Hong Kong, China", new Medals { Gold = 1, Silver = 2, Bronze = 3 }),
        new Country("hu", "Hungary", new Medals { Gold = 6, Silver = 7, Bronze = 7 }),
        new Country("in", "India", new Medals { Gold = 1, Silver = 2, Bronze = 4 }),
        new Country("id", "Indonesia", new Medals { Gold = 1, Silver = 1, Bronze = 3 }),
        new Country("ie", "Ireland", new Medals { Gold = 2, Silver = 0, Bronze = 2 }),
        new Country("ir", "Islamic Republic of Iran", new Medals { Gold = 3, Silver = 2, Bronze = 2 }),
        new Country("il", "Israel", new Medals { Gold = 2, Silver = 0, Bronze = 2 }),
        new Country("it", "Italy", new Medals { Gold = 10, Silver = 10, Bronze = 20 }),
        new Country("jm", "Jamaica", new Medals { Gold = 4, Silver = 1, Bronze = 4 }),
        new Country("jp", "Japan", new Medals { Gold = 27, Silver = 14, Bronze = 17 }),
        new Country("jo", "Jordan", new Medals { Gold = 0, Silver = 1, Bronze = 1 }),
        new Country("kz", "Kazakhstan", new Medals { Gold = 0, Silver = 0, Bronze = 8 }),
        new Country("ke", "Kenya", new Medals { Gold = 4, Silver = 4, Bronze = 2 }),
        new Country("xk", "Kosovo", new Medals { Gold = 2, Silver = 0, Bronze = 0 }),
        new Country("kw", "Kuwait", new Medals { Gold = 0, Silver = 0, Bronze = 1 }),
        new Country("kg", "Kyrgyzstan", new Medals { Gold = 0, Silver = 2, Bronze = 1 }),
        new Country("lv", "Latvia", new Medals { Gold = 1, Silver = 0, Bronze = 1 }),
        new Country("lt", "Lithuania", new Medals { Gold = 0, Silver = 1, Bronze = 0 }),
        new Country("my", "Malaysia", new Medals { Gold = 0, Silver = 1, Bronze = 1 }),
        new Country("mx", "Mexico", new Medals { Gold = 0, Silver = 0, Bronze = 4 }),
        new Country("mn", "Mongolia", new Medals { Gold = 0, Silver = 1, Bronze = 3 }),
        new Country("ma", "Morocco", new Medals { Gold = 1, Silver = 0, Bronze = 0 }),
        new Country("na", "Namibia", new Medals { Gold = 0, Silver = 1, Bronze = 0 }),
        new Country("nl", "Netherlands", new Medals { Gold = 10, Silver = 12, Bronze = 14 }),
        new Country("nz", "New Zealand", new Medals { Gold = 7, Silver = 6, Bronze = 7 }),
        new Country("ng", "Nigeria", new Medals { Gold = 0, Silver = 1, Bronze = 1 }),
        new Country("mk", "North Macedonia", new Medals { Gold = 0, Silver = 1, Bronze = 0 }),
        new Country("no", "Norway", new Medals { Gold = 4, Silver = 2, Bronze = 2 }),
        new Country("cn", "People's Republic of China", new Medals { Gold = 38, Silver = 32, Bronze = 18 }),
        new Country("ph", "Philippines", new Medals { Gold = 1, Silver = 2, Bronze = 1 }),
        new Country("pl", "Poland", new Medals { Gold = 4, Silver = 5, Bronze = 5 }),
        new Country("pt", "Portugal", new Medals { Gold = 1, Silver = 1, Bronze = 2 }),
        new Country("pr", "Puerto Rico", new Medals { Gold = 1, Silver = 0, Bronze = 0 }),
        new Country("qa", "Qatar", new Medals { Gold = 2, Silver = 0, Bronze = 1 }),
        new Country("kr", "Republic of Korea", new Medals { Gold = 6, Silver = 4, Bronze = 10 }),
        new Country("md", "Republic of Moldova", new Medals { Gold = 0, Silver = 0, Bronze = 1 }),
        new Country("roc", "ROC", new Medals { Gold = 20, Silver = 28, Bronze = 23 }),
        new Country("ro", "Romania", new Medals { Gold = 1, Silver = 3, Bronze = 0 }),
        new Country("sm", "San Marino", new Medals { Gold = 0, Silver = 1, Bronze = 2 }),
        new Country("sa", "Saudi Arabia", new Medals { Gold = 0, Silver = 1, Bronze = 0 }),
        new Country("rs", "Serbia", new Medals { Gold = 3, Silver = 1, Bronze = 5 }),
        new Country("sk", "Slovakia", new Medals { Gold = 1, Silver = 2, Bronze = 1 }),
        new Country("si", "Slovenia", new Medals { Gold = 3, Silver = 1, Bronze = 1 }),
        new Country("za", "South Africa", new Medals { Gold = 1, Silver = 2, Bronze = 0 }),
        new Country("es", "Spain", new Medals { Gold = 3, Silver = 8, Bronze = 6 }),
        new Country("se", "Sweden", new Medals { Gold = 3, Silver = 6, Bronze = 0 }),
        new Country("ch", "Switzerland", new Medals { Gold = 3, Silver = 4, Bronze = 6 }),
        new Country("sy", "Syrian Arab Republic", new Medals { Gold = 0, Silver = 0, Bronze = 1 }),
        new Country("th", "Thailand", new Medals { Gold = 1, Silver = 0, Bronze = 1 }),
        new Country("tn", "Tunisia", new Medals { Gold = 1, Silver = 1, Bronze = 0 }),
        new Country("tr", "Turkey", new Medals { Gold = 2, Silver = 2, Bronze = 9 }),
        new Country("tm", "Turkmenistan", new Medals { Gold = 0, Silver = 1, Bronze = 0 }),
        new Country("ug", "Uganda", new Medals { Gold = 2, Silver = 1, Bronze = 1 }),
        new Country("ua", "Ukraine", new Medals { Gold = 1, Silver = 6, Bronze = 12 }),
        new Country("us", "United States of America", new Medals { Gold = 39, Silver = 41, Bronze = 33 }),
        new Country("uz", "Uzbekistan", new Medals { Gold = 3, Silver = 0, Bronze = 2 }),
        new Country("ve", "Venezuela", new Medals { Gold = 1, Silver = 3, Bronze = 0 }),
    };
}
