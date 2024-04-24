namespace FluentUI.Demo.Shared.SampleData;

public static class DataSourceExtensions
{
    public static IQueryable<Person> WithVeryLongName(this IQueryable<Person> values)
    {
        var longName = new Person(PersonId: 91,
                                  FirstName: "Jean",
                                  LastName: "With a very long name to validate components",
                                  CountryCode: "fr",
                                  BirthDate: new DateOnly(1984, 4, 27),
                                  Picture: DataSource.ImageFaces[0]);
        return values.Concat(new[] { longName });
    }
}
