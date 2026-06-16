# FluentUI Icons & Emoji's Generator

## How to generate FluentUI Icons classes

This application generates **FluentUI Icons** from SVG images
from the official website https://github.com/microsoft/fluentui-system-icons

To generate the icons, you need:

1. To work with the `fluentui-system-icons` repo, you will need a Linux environment.
   This is due to the way the `npm` scripts are handling paths.

   If you are working on Windows, we recommend you setup a [WSL2 environment](https://learn.microsoft.com/en-us/windows/wsl/install).
   We have validated the steps to work from an Ubuntu 22.04 installation.
   If you are wotking on a Mac, you are probably good to go but this has not been verified.

   ```cmd
   wsl --install -d Ubuntu-22.04
   ```

2. Once your Linux environment is setup clone the [fluentui-system-icons](https://github.com/microsoft/fluentui-system-icons) repository to your Linux environment. 

   ```cmd
   git clone https://github.com/microsoft/fluentui-system-icons.git
   ```

   You do not need to create a fork of the repo as all the necessary work has been integrated into the repo already

3. Once cloned, navigate to the directory `/packages/svg-icons` and install the needed dependencies:
 
   ```cmd
   npm install --only=dev
   ```

   You might need to repeat this command on the root folder of the repo as well.

   > If you have an issue with the NPM command, try to install NVM and to re-install the latest version of NPM on your Linux environment.
   > ```cmd
   > curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/master/install.sh | bash
   > ```
   > ```cmd
   > nvm install --lts
   > ```


4. Clean out any previous work and run the build command.

   ```
   npm run clean
   npm run buildforblazor
   ```

5. Navigate to the created icons folder (under `svg-icons`)

   ```
   cd icons
   ```

6. Copy the result to your local fluentui-blazor clone/fork.

   ```
   rsync -r -v  .  /mnt/c/Temp/Icons
   ```

   The `/mnt/..` path is a WSL2 thing. Adjust accordingly if you are working  on a Mac.
   Make sure the path in the mnt (c/Temp/FluentIcons) points to a temporary folder will use in next steps.
	
   The `rsync` command should only copy new ad/or update files. 

7. a) Compile the project and run the `FluentAssetsGenerator.exe` application,
   passing the **Assets** path to this local folder containing the SVG images as a parameter.
   
	   ```cmd
	   FluentAssetsGenerator.exe --Assets=C:\Temp\Icons --Target=./Samples --Library=Icon
	   ```
   
   b) Or run the project, passing parameters to the `Properties/launchSettings.json` file:

	   ```json
	   {
		  "profiles": {
			"Microsoft.FluentUI.AspNetCore.Components.AssetsGenerator": {
			  "commandName": "Project",
			  "commandLineArgs": "--Assets=C:/Temp/Icons --Target=./Samples --Library=Icon"
			}
		  }
		}
	   ```

3. All generated C# files are located in the **Target** folder, or in the current folder if this attribute has not been defined.

## How to generate FluentUI Emoji classes

1. Download SVG images in a local folder (ex. C:\Temp) from the [repository](https://github.com/microsoft/fluentui-emoji).

	```cmd
	git clone https://github.com/microsoft/fluentui-emoji
	```

2. a) Compile the project and run the `FluentAssetsGenerator.exe` application,
   passing the **Assets** path to this local folder containing the SVG images as a parameter.
   
	   ```cmd
	   FluentAssetsGenerator.exe --Assets=C:\Temp\Emojis --Target=./Samples --Library=Emoji
	   ```
   
   b) Or run the project, passing parameters to the `Properties/launchSettings.json` file:

	   ```json
	   {
		  "profiles": {
			"Microsoft.FLuentUI.AspNetCore.Components.AssetsGenerator": {
			  "commandName": "Project",
			  "commandLineArgs": "--Assets=C:/Temp/Icons --Target=./Samples --Library=Emoji"
			}
		  }
		}
	   ```

## Help

Run the application with the `--help` parameter to see all available parameters.

```cmd
> FluentAssetsGenerator.exe --help


  FluentAssetsGenerator --folder:<Icons_Folder_Directory>

  --Assets    | -a   The root directory containing all SVG icons,
                     downloaded from https://github.com/microsoft/fluentui-system-icons.
                     If not specified, the current working directory will be used.

  --Library   | -l   The type of library to generate: icon or emoji.
                     If not specified, "icon" will be used.

  --Namespace | -ns  The namespace used for generated classes.
                     If not specified, "Microsoft.FluentUI.AspNetCore.Components" will be used.

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
