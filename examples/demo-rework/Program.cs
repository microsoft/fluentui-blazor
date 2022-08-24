// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string dir = @"C:\Source\Blazor\fast-blazor\examples\FluentUI.Demo.Shared\Pages";

foreach (string d in Directory.GetDirectories(dir))
{
    Directory.CreateDirectory(Path.Combine(d, "Examples"));
}