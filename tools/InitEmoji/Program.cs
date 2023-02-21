using System.Text.Json;
using System.Text.RegularExpressions;


// First create the group folders
var groupFolders = new List<string>() {
    "Objects",
    "People_Body",
    "Smileys_Emotion",
    "Animals_Nature",
    "Food_Drink",
    "Symbols",
    "Travel_Places",
    "Activities",
    "Flags"
};

//const int maxnamelength = 15;
const string relDestFolder = @"..\Microsoft.Fast.Components.FluentUI\Assets\emojis";
const string relSourceFolder = @"..\..\..\fluentui-emoji\assets";


string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
string sRoot = Path.Combine(sCurrentDirectory, @"..\..\..");

string baseDestFolder = Path.GetFullPath(Path.Combine(sRoot, relDestFolder));
string baseSourceFolder = Path.GetFullPath(Path.Combine(sRoot, relSourceFolder));

string[] stylefolders = { "Color", "Flat", "High Contrast" };
string[] skinTones = { "Default", "Light", "Medium-Light", "Medium", "Medium-Dark", "Dark" };



foreach (var group in groupFolders)
{
    string? groupFolder = Path.Combine(baseDestFolder, group);
    Directory.CreateDirectory(groupFolder);
}

int i = 0;


// Then create the emoji folders
foreach (string folder in Directory.EnumerateDirectories(baseSourceFolder))
{
    string assetFolder = Path.GetFileName(folder);
    string baseFileName = assetFolder.Replace(" ", "_").ToLower();
    string destFolder = Name().Replace(assetFolder, m => m.Value.ToUpper()).Replace(" ", "").Replace("-", "");

    // Read metadata file from folder
    string metadataFile = Path.Combine(folder, "metadata.json");

    // Read and parse json
    Metadata? metadata = JsonSerializer.Deserialize<Metadata>(File.ReadAllText(metadataFile));

    //metadata!.basefilename = baseFileName;
    string groupFolder = metadata!.group!.Replace(" & ", "_");

    string destPath = Path.Combine(baseDestFolder, groupFolder, destFolder);
    Directory.CreateDirectory(destPath);

    // Write adjusted metadata file to folder
    //File.WriteAllText(Path.Combine(destPath, "metadata.json"), JsonSerializer.Serialize(metadata, new JsonSerializerOptions() { WriteIndented = true }));
    File.Copy(metadataFile, Path.Combine(destPath, "metadata.json"));

    i++;

    // Copy all files from subfolders to new folder
    foreach (string subfolder in Directory.EnumerateDirectories(folder))
    {
        string? subfolderName = Path.GetFileName(subfolder);

        if (stylefolders.Contains(subfolderName))
        {
            foreach (string file in Directory.EnumerateFiles(subfolder))
            {
                string? fileName = Path.GetFileName(file);
                //string composedName = $"{(assetFolder.Replace(" ", "_"))[0..Math.Min(assetFolder.Length, maxnamelength)]}_{subfolderName[0]}.svg".ToLower();
                string composedName = $"{subfolderName[0]}.svg".ToLower();
                Console.WriteLine($"{fileName} copied to {composedName}");

                File.Copy(file, Path.Combine(destPath, composedName), true);
                i++;
            }
        }
        if (skinTones.Contains(subfolderName))
        {
            string skinToneShort = subfolderName switch
            {
                "Default" => "de",
                "Light" => "li",
                "Medium-Light" => "ml",
                "Medium" => "me",
                "Medium-Dark" => "md",
                "Dark" => "da",
                _ => ""
            };
            foreach (string subsubfolder in Directory.EnumerateDirectories(subfolder))
            {
                string? subsubfolderName = Path.GetFileName(subsubfolder);

                if (stylefolders.Contains(subsubfolderName))
                {
                    foreach (string file in Directory.EnumerateFiles(subsubfolder))
                    {
                        string? fileName = Path.GetFileName(file);

                        //string composedName = $"{(assetFolder.Replace(" ", "_"))[0..Math.Min(assetFolder.Length, maxnamelength)]}_{skinToneShort}_{subsubfolderName[0]}.svg".ToLower();
                        string composedName = $"{skinToneShort}_{subsubfolderName[0]}.svg".ToLower();

                        Console.WriteLine($"{fileName} copied to {composedName}");

                        File.Copy(file, Path.Combine(destPath, composedName), true);
                        i++;
                    }
                }
            }
        }
    }
}
Console.WriteLine($"{i} files copied");

public class Metadata
{
    public string? basefilename { get; set; }
    public string? cldr { get; set; }
    public string? fromVersion { get; set; }
    public string? glyph { get; set; }
    public string[]? glyphAsUtfInEmoticons { get; set; }
    public string? group { get; set; }
    public string[]? keywords { get; set; }
    public string[]? mappedToEmoticons { get; set; }
    public string? tts { get; set; }
    public string? unicode { get; set; }
    public string[]? unicodeSkintones { get; set; }
}

partial class Program
{
    [GeneratedRegex("(?<=\\s|-)\\w")]
    private static partial Regex Name();
}