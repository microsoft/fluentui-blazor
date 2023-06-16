# FluentUI Icons Generator

## How to generate FluentUI Icons classes

This application generates **FluentUI Icons** from SVG images
from the official website https://github.com/microsoft/fluentui-system-icons

To generate the icons, you need:

1. Download SVG images in a local folder (ex. C:\Temp) from the [official NPM repo](https://www.npmjs.com/package/@fluentui/svg-icons), using this command.

   ```cmd
   npm install @fluentui/svg-icons
   ```

2. Compile the project and run the `FluentUIIconsGenerator.exe` application,
   passing the **Assets** path to this local folder containing the SVG images as a parameter.
   
   ```cmd
   FluentUIIconsGenerator.exe --Assets=C:\Temp\Icons --Target=./Samples
   ```
   
   Or run the project, passing parameters to the `Properties/launchSettings.json` file:

   Example:
   ```json
   {
	  "profiles": {
		"Microsoft.Fast.Components.FluentUI.IconsGenerator": {
		  "commandName": "Project",
		  "commandLineArgs": "--Assets=C:/Temp/Icons --Target=./Samples"
		}
	  }
	}
   ```

3. All generated C# files are located in the **Target** folder, or in the current folder if this attribute has not been defined.


## Help

Run the application with the `--help` parameter to see all available parameters.

```cmd
> FluentUIIconsGenerator.exe --help



  FluentIconsGenerator --folder:<Icons_Folder_Directory>
  
  --Assets    | -a   The root directory containing all SVG icons,
                     downloaded from https://github.com/microsoft/fluentui-system-icons.
                     If not specified, the current working directory will be used.
  
  --Namespace | -ns  The namespace used for generated classes.
                     If not specified, "Microsoft.Fast.Components.FluentUI\ will be used.
  
  --Names     | -n   The list of icon names to generate, separated by coma.
                     Example: accessibility_32_filled,add_circle_20_filled"
                     By default: all icons

  --Sizes     | -s   The list of icon sizes to generate, separated by coma.
                     Example: 12,24. By default: all sizes
  
  --Target    | -t   The target directory where C# classes will be created.
                     If not specified, the current working directory will be used.
  
  --Help      | -h   Display this documentation.
```
