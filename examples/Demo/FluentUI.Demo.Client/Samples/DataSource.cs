// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.Client.Samples;

public static class DataSource
{
    public static readonly string[] NavigationMenu =
    [
        "Home", "About Us", "Services", "Products", "Portfolio", "Blog", "News", "Events", "Careers", "Contact Us",
        "FAQ",  "Testimonials", "Case Studies", "Resources", "Pricing", "Support", "Privacy Policy", "Terms of Service",
        "Case Studies", "Sitemap", "Login/Register",
    ];

    public static readonly string[] ImageFaces =
    [
        Data.Faces.Face1,
        Data.Faces.Face2,
        Data.Faces.Face3,
        Data.Faces.Face4,
        Data.Faces.Face5,
        Data.Faces.Face6,
        Data.Faces.Face7,
        Data.Faces.Face8,
    ];

    public static readonly string[] LoremIpsum =
    [
        Data.LoremIpsum.Lorem1,
        Data.LoremIpsum.Lorem2,
        Data.LoremIpsum.Lorem3,
        Data.LoremIpsum.Lorem4,
        Data.LoremIpsum.Lorem5,
        Data.LoremIpsum.Lorem6,
        Data.LoremIpsum.Lorem7,
        Data.LoremIpsum.Lorem8,
        Data.LoremIpsum.Lorem9,
        Data.LoremIpsum.Lorem10,
    ];

    // Content derived from https://www.kaggle.com/datasets/arjunprasadsarkhel/2021-olympics-in-tokyo,
    // which is licensed as https://creativecommons.org/licenses/by-sa/4.0/
    public readonly static Country[] Countries =
    [
        new Country("ar", "Argentina", new Medals(Gold: 0, Silver: 1, Bronze: 2)),
        new Country("am", "Armenia", new Medals (Gold: 0, Silver: 2, Bronze: 2)),
        new Country("au", "Australia", new Medals (Gold: 17, Silver: 7, Bronze: 22)),
        new Country("at", "Austria", new Medals (Gold: 1, Silver: 1, Bronze: 5)),
        new Country("az", "Azerbaijan", new Medals (Gold: 0, Silver: 3, Bronze: 4)),
        new Country("bs", "Bahamas", new Medals (Gold: 2, Silver: 0, Bronze: 0)),
        new Country("bh", "Bahrain", new Medals (Gold: 0, Silver: 1, Bronze: 0)),
        new Country("by", "Belarus", new Medals (Gold: 1, Silver: 3, Bronze: 3)),
        new Country("be", "Belgium", new Medals (Gold: 3, Silver: 1, Bronze: 3)),
        new Country("bm", "Bermuda", new Medals (Gold: 1, Silver: 0, Bronze: 0)),
        new Country("bw", "Botswana", new Medals (Gold: 0, Silver: 0, Bronze: 1)),
        new Country("br", "Brazil", new Medals (Gold: 7, Silver: 6, Bronze: 8)),
        new Country("bg", "Bulgaria", new Medals (Gold: 3, Silver: 1, Bronze: 2)),
        new Country("bf", "Burkina Faso", new Medals (Gold: 0, Silver: 0, Bronze: 1)),
        new Country("ca", "Canada", new Medals (Gold: 7, Silver: 6, Bronze: 11)),
        new Country("tpe", "Chinese Taipei", new Medals (Gold: 2, Silver: 4, Bronze: 6)),
        new Country("co", "Colombia", new Medals (Gold: 0, Silver: 4, Bronze: 1)),
        new Country("ci", "Côte d'Ivoire", new Medals (Gold: 0, Silver: 0, Bronze: 1)),
        new Country("hr", "Croatia", new Medals (Gold: 3, Silver: 3, Bronze: 2)),
        new Country("cu", "Cuba", new Medals (Gold: 7, Silver: 3, Bronze: 5)),
        new Country("cz", "Czech Republic", new Medals (Gold: 4, Silver: 4, Bronze: 3)),
        new Country("dk", "Denmark", new Medals (Gold: 3, Silver: 4, Bronze: 4)),
        new Country("do", "Dominican Republic", new Medals (Gold: 0, Silver: 3, Bronze: 2)),
        new Country("ec", "Ecuador", new Medals (Gold: 2, Silver: 1, Bronze: 0)),
        new Country("eg", "Egypt", new Medals (Gold: 1, Silver: 1, Bronze: 4)),
        new Country("ee", "Estonia", new Medals (Gold: 1, Silver: 0, Bronze: 1)),
        new Country("et", "Ethiopia", new Medals (Gold: 1, Silver: 1, Bronze: 2)),
        new Country("fj", "Fiji", new Medals (Gold: 1, Silver: 0, Bronze: 1)),
        new Country("fi", "Finland", new Medals (Gold: 0, Silver: 0, Bronze: 2)),
        new Country("fr", "France", new Medals (Gold: 10, Silver: 12, Bronze: 11)),
        new Country("ge", "Georgia", new Medals (Gold: 2, Silver: 5, Bronze: 1)),
        new Country("de", "Germany", new Medals (Gold: 10, Silver: 11, Bronze: 16)),
        new Country("gh", "Ghana", new Medals (Gold: 0, Silver: 0, Bronze: 1)),
        new Country("gb", "Great Britain", new Medals (Gold: 22, Silver: 21, Bronze: 22)),
        new Country("gr", "Greece", new Medals (Gold: 2, Silver: 1, Bronze: 1)),
        new Country("gd", "Grenada", new Medals (Gold: 0, Silver: 0, Bronze: 1)),
        new Country("hk", "Hong Kong, China", new Medals (Gold: 1, Silver: 2, Bronze: 3)),
        new Country("hu", "Hungary", new Medals (Gold: 6, Silver: 7, Bronze: 7)),
        new Country("in", "India", new Medals (Gold: 1, Silver: 2, Bronze: 4)),
        new Country("id", "Indonesia", new Medals (Gold: 1, Silver: 1, Bronze: 3)),
        new Country("ie", "Ireland", new Medals (Gold: 2, Silver: 0, Bronze: 2)),
        new Country("ir", "Islamic Republic of Iran", new Medals (Gold: 3, Silver: 2, Bronze: 2)),
        new Country("il", "Israel", new Medals (Gold: 2, Silver: 0, Bronze: 2)),
        new Country("it", "Italy", new Medals (Gold: 10, Silver: 10, Bronze: 20)),
        new Country("jm", "Jamaica", new Medals (Gold: 4, Silver: 1, Bronze: 4)),
        new Country("jp", "Japan", new Medals (Gold: 27, Silver: 14, Bronze: 17)),
        new Country("jo", "Jordan", new Medals (Gold: 0, Silver: 1, Bronze: 1)),
        new Country("kz", "Kazakhstan", new Medals (Gold: 0, Silver: 0, Bronze: 8)),
        new Country("ke", "Kenya", new Medals (Gold: 4, Silver: 4, Bronze: 2)),
        new Country("xk", "Kosovo", new Medals (Gold: 2, Silver: 0, Bronze: 0)),
        new Country("kw", "Kuwait", new Medals (Gold: 0, Silver: 0, Bronze: 1)),
        new Country("kg", "Kyrgyzstan", new Medals (Gold: 0, Silver: 2, Bronze: 1)),
        new Country("lv", "Latvia", new Medals (Gold: 1, Silver: 0, Bronze: 1)),
        new Country("lt", "Lithuania", new Medals (Gold: 0, Silver: 1, Bronze: 0)),
        new Country("my", "Malaysia", new Medals (Gold: 0, Silver: 1, Bronze: 1)),
        new Country("mx", "Mexico", new Medals (Gold: 0, Silver: 0, Bronze: 4)),
        new Country("mn", "Mongolia", new Medals (Gold: 0, Silver: 1, Bronze: 3)),
        new Country("ma", "Morocco", new Medals (Gold: 1, Silver: 0, Bronze: 0)),
        new Country("na", "Namibia", new Medals (Gold: 0, Silver: 1, Bronze: 0)),
        new Country("nl", "Netherlands", new Medals (Gold: 10, Silver: 12, Bronze: 14)),
        new Country("nz", "New Zealand", new Medals (Gold: 7, Silver: 6, Bronze: 7)),
        new Country("ng", "Nigeria", new Medals (Gold: 0, Silver: 1, Bronze: 1)),
        new Country("mk", "North Macedonia", new Medals (Gold: 0, Silver: 1, Bronze: 0)),
        new Country("no", "Norway", new Medals (Gold: 4, Silver: 2, Bronze: 2)),
        new Country("cn", "People's Republic of China", new Medals (Gold: 38, Silver: 32, Bronze: 18)),
        new Country("ph", "Philippines", new Medals (Gold: 1, Silver: 2, Bronze: 1)),
        new Country("pl", "Poland", new Medals (Gold: 4, Silver: 5, Bronze: 5)),
        new Country("pt", "Portugal", new Medals (Gold: 1, Silver: 1, Bronze: 2)),
        new Country("pr", "Puerto Rico", new Medals (Gold: 1, Silver: 0, Bronze: 0)),
        new Country("qa", "Qatar", new Medals (Gold: 2, Silver: 0, Bronze: 1)),
        new Country("kr", "Republic of Korea", new Medals (Gold: 6, Silver: 4, Bronze: 10)),
        new Country("md", "Republic of Moldova", new Medals (Gold: 0, Silver: 0, Bronze: 1)),
        new Country("roc", "ROC", new Medals (Gold: 20, Silver: 28, Bronze: 23)),
        new Country("ro", "Romania", new Medals (Gold: 1, Silver: 3, Bronze: 0)),
        new Country("sm", "San Marino", new Medals (Gold: 0, Silver: 1, Bronze: 2)),
        new Country("sa", "Saudi Arabia", new Medals (Gold: 0, Silver: 1, Bronze: 0)),
        new Country("rs", "Serbia", new Medals (Gold: 3, Silver: 1, Bronze: 5)),
        new Country("sk", "Slovakia", new Medals (Gold: 1, Silver: 2, Bronze: 1)),
        new Country("si", "Slovenia", new Medals (Gold: 3, Silver: 1, Bronze: 1)),
        new Country("za", "South Africa", new Medals (Gold: 1, Silver: 2, Bronze: 0)),
        new Country("es", "Spain", new Medals (Gold: 3, Silver: 8, Bronze: 6)),
        new Country("se", "Sweden", new Medals (Gold: 3, Silver: 6, Bronze: 0)),
        new Country("ch", "Switzerland", new Medals (Gold: 3, Silver: 4, Bronze: 6)),
        new Country("sy", "Syrian Arab Republic", new Medals (Gold: 0, Silver: 0, Bronze: 1)),
        new Country("th", "Thailand", new Medals (Gold: 1, Silver: 0, Bronze: 1)),
        new Country("tn", "Tunisia", new Medals (Gold: 1, Silver: 1, Bronze: 0)),
        new Country("tr", "Turkey", new Medals (Gold: 2, Silver: 2, Bronze: 9)),
        new Country("tm", "Turkmenistan", new Medals (Gold: 0, Silver: 1, Bronze: 0)),
        new Country("ug", "Uganda", new Medals (Gold: 2, Silver: 1, Bronze: 1)),
        new Country("ua", "Ukraine", new Medals (Gold: 1, Silver: 6, Bronze: 12)),
        new Country("us", "United States of America", new Medals (Gold: 39, Silver: 41, Bronze: 33)),
        new Country("uz", "Uzbekistan", new Medals (Gold: 3, Silver: 0, Bronze: 2)),
        new Country("ve", "Venezuela", new Medals (Gold: 1, Silver: 3, Bronze: 0)),
    ];
}

public record Country(string Code, string Name, Medals Medals);

public record Medals(int Gold, int Silver, int Bronze) { public int Total => Gold + Silver + Bronze; }
