# FluentUI Icons Generator

## How to generate FluentUI Icons classes

This application generates **FluentUI Icons** from SVG images
from the official website https://github.com/microsoft/fluentui-system-icons

To generate the icons, you need:

1. Download SVG images in a local folder (ex. C:\Temp) from the [official NPM repo](https://www.npmjs.com/package/@fluentui/svg-icons), using this command.

   ```cmd
   npm install @fluentui/svg-icons
   ```

2. a) Compile the project and run the `FluentUIIconsGenerator.exe` application,
   passing the **Assets** path to this local folder containing the SVG images as a parameter.
   
	   ```cmd
	   FluentUIIconsGenerator.exe --Assets=C:\Temp\Icons --Target=./Samples --Library=Icon
	   ```
   
   b) Or run the project, passing parameters to the `Properties/launchSettings.json` file:

	   ```json
	   {
		  "profiles": {
			"Microsoft.Fast.Components.FluentUI.IconsGenerator": {
			  "commandName": "Project",
			  "commandLineArgs": "--Assets=C:/Temp/Icons --Target=./Samples --Library=Icon"
			}
		  }
		}
	   ```

3. All generated C# files are located in the **Target** folder, or in the current folder if this attribute has not been defined.

## How to generate FluentUI Emoji classes

1. Download the [repository](https://github.com/microsoft/fluentui-emoji).

	```cmd
	git clone https://github.com/microsoft/fluentui-emoji
	```

2. a) Compile the project and run the `FluentUIIconsGenerator.exe` application,
   passing the **Assets** path to this local folder containing the SVG images as a parameter.
   
	   ```cmd
	   FluentUIIconsGenerator.exe --Assets=C:\Temp\Emojis --Target=./Samples --Library=Emoji
	   ```
   
   b) Or run the project, passing parameters to the `Properties/launchSettings.json` file:

	   ```json
	   {
		  "profiles": {
			"Microsoft.Fast.Components.FluentUI.IconsGenerator": {
			  "commandName": "Project",
			  "commandLineArgs": "--Assets=C:/Temp/Icons --Target=./Samples --Library=Emoji"
			}
		  }
		}
	   ```

## Help

Run the application with the `--help` parameter to see all available parameters.

```cmd
> FluentUIIconsGenerator.exe --help


  FluentIconsGenerator --folder:<Icons_Folder_Directory>

  --Assets    | -a   The root directory containing all SVG icons,
                     downloaded from https://github.com/microsoft/fluentui-system-icons.
                     If not specified, the current working directory will be used.

  --Library   | -l   The type of library to generate: icon or emoji.
                     If not specified, "icon" will be used.

  --Namespace | -ns  The namespace used for generated classes.
                     If not specified, "Microsoft.Fast.Components.FluentUI" will be used.

  --Names     | -n   The list of icon names to generate, separated by coma.
                     Example of icons: accessibility_32_filled,add_circle_20_filled
                     Example of emojis: accordion_flat,ambulance_high_contrast
                     By default: all icons

  --Sizes     | -s   The list of icon sizes to generate, separated by coma.
                     Example: 12,24. By default: all sizes
                     (Not available for emoji library)

  --Target    | -t   The target directory where C# classes will be created.
                     If not specified, the current working directory will be used.

  --Help      | -h   Display this documentation.
```
