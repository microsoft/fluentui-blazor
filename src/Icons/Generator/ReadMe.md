# FluentUI Icons Generator

## How to generate FluentUI Icons classes

This application generates **FluentUI Icons** from SVG images
from the official website https://github.com/microsoft/fluentui-system-icons

To generate the icons, you need:

1. Download SVG images locally from the [official website](https://github.com/microsoft/fluentui-system-icons).
2. Run the `FluentUIIconsGenerator.exe` application, passing the **Assets** path to the folder containing the SVG images as a parameter.
   
   ```cmd
   FluentUIIconsGenerator.exe --Assets=C:\Temp\fluentui-system-icons\main\assets
   ```
   
   Or run the project, passing parameters to the `Properties/launchSettings.json` file:

   Example:
   ```json
   {
	  "profiles": {
		"Microsoft.Fast.Components.FluentUI.IconsGenerator": {
		  "commandName": "Project",
		  "commandLineArgs": "--Assets=C:/Temp/fluentui-system-icons/main/assets --Target=./Samples"
		}
	  }
	}
   ```

3. All generated C# files are located in the **Target** folder, or in the current folder if this attribute has not been defined.


## Help

Run the application with the `--help` parameter to see all available parameters.

```cmd
> FluentUIIconsGenerator.exe --help



Microsoft FluentUI Icons Generator

  FluentIconsGenerator --folder:<Icons_Folder_Directory>

  --Assets    | -a   The root directory containing all SVG icons,
                     downloaded from https://github.com/microsoft/fluentui-system-icons.
                     If not specified, the current working directory will be used.

  --Namespace | -ns  The namespace used for generated classes.
                     If not specified, "Microsoft.Fast.Components.FluentUI" will be used.

  --Sizes     | -s   The list of icon sizes to generate, separated by coma.
                     By default: 16,24,32

  --Target    | -t   The target directory where C# classes will be created.
                     If not specified, the current working directory will be used.

  --Help      | -h   Display this documentation.
```

## Technical information

This application stores the SVG data of the icons in RESX files,
because, due to the large number of icons, it's not possible to place such a large 
quantity of character strings without receiving a compilation error.

```
Error CS8103: Combined length of user strings used by the program exceeds allowed limit
```
