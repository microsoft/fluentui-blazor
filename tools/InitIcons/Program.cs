// See https://aka.ms/new-console-template for more information

Console.WriteLine("Renaming icons");
int counter = 0;

static void RenameFile(string folder, string file)
{
    string? fileName = Path.GetFileNameWithoutExtension(file);

    int index = fileName!.LastIndexOf("_", StringComparison.Ordinal);
    string size = fileName![(index - 2)..index];
    char variant = fileName![(index + 1)];

    string? newFileName = $"{size}_{variant}";

    string? newFilePath = Path.Combine(folder, newFileName + ".svg");

    File.Move(file, newFilePath, true);

}

const string relRootFolder = @"..\..\src\Microsoft.Fast.Components.FluentUI\Assets\icons";

string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
string sRoot = Path.Combine(sCurrentDirectory, @"..\..\..");

string iconsRootFolder = Path.GetFullPath(Path.Combine(sRoot, relRootFolder));

foreach (string folder in Directory.EnumerateDirectories(iconsRootFolder))
{
    foreach (string file in Directory.EnumerateFiles(folder))
    {
        RenameFile(folder, file);
        counter++;
    }

    foreach (string subfolder in Directory.EnumerateDirectories(folder))
    {

        foreach (string file in Directory.EnumerateFiles(subfolder))
        {
            RenameFile(subfolder, file);
            counter++;
        }
    }
}

Console.WriteLine($"{counter} icons renamed");
