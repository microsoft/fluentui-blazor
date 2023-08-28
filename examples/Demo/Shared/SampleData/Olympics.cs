namespace FluentUI.Demo.Shared.SampleData;

public record Medals
{
    public int Gold { get; init; }
    public int Silver { get; init; }
    public int Bronze { get; init; }

    public int Total => Gold + Silver + Bronze;
}

public record Country(string Code, string Name, Medals Medals);
