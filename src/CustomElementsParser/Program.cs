using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace CustomElementsParser;

public class Program
{
    public static void Main()
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,  // set camelCase       
            WriteIndented = true                                // write pretty json
        };

        string fileName = "C:\\Source\\Blazor\\fast-blazor\\src\\CustomElementsParser\\custom-elements.json";
        string jsonString = File.ReadAllText(fileName);
        CustomElements customElements = JsonSerializer.Deserialize<CustomElements>(jsonString, options)!;

        StringBuilder classBuilder = new();
        StringBuilder elementBuilder = new();

        foreach (Module m in customElements.modules)
        {
            foreach (Declaration d in m.declarations)
            {
                if (d.kind != "variable" && d.superclass != null && d.superclass.name == "FASTElement")
                {

                    string name;
                    if (d.name.StartsWith("FAST"))
                        name = d.name[4..];
                    else
                        name = d.name;
                    classBuilder.AppendLine($"public partial class Fluent{name}");
                    classBuilder.AppendLine("{");

                    name = Regex.Replace(name, @"([A-Z])", "-$1");

                    elementBuilder.AppendLine($"<fluent{name.ToLower()} ");

                    if (d.attributes is not null)
                    {
                        foreach (Attribute a in d.attributes)
                        {
                            if (!string.IsNullOrEmpty(a.name))
                            {


                                string composedName = a.name[0].ToString().ToUpper() + a.name[1..];

                                composedName = Regex.Replace(composedName, @"(-[a-z])", m => m.ToString()[1].ToString().ToUpper());


                                classBuilder.AppendLine("\t[Parameter]");
                                classBuilder.AppendLine($"\tpublic {a.type.text}? {composedName} {{ get; set; }}\n");

                                elementBuilder.AppendLine($"\t{a.name}=@{composedName}");
                            }
                        }
                    }
                    classBuilder.AppendLine("}\n");
                    elementBuilder.AppendLine($"></fluent{name.ToLower()}>\n");

                }

            }

        }
        Console.WriteLine("Generated Classes");
        Console.WriteLine(classBuilder.ToString());
        Console.WriteLine("\n-----------\n");
        Console.WriteLine("Generated Elements");
        Console.WriteLine(elementBuilder.ToString());
    }
}
