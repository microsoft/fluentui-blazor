# Introduction
This document describes the step needed to update the `wwwroot\icons` folder when a new version of the [fluentui-system-icons](https://github.com/microsoft/fluentui-system-icons) is released

## Setup steps
To work with the `fluentui-system-icons` repo, you will need a Linux environment. This is due to the way the `npm` scripts are handling paths.

If you are working on Windows, we recommend you setup a WSL2 environment. We have validated the steps to work from an Ubuntu 22.04 installation. If you are wotking on a Mac, you are probably good to go but this has not been verified.

Once your Linux environment is setup clone the [fluentui-system-icons](https://github.com/microsoft/fluentui-system-icons) repository to your Linux environment. 

```
git clone https://github.com/microsoft/fluentui-system-icons.git
```

You do not need to create a fork of the repo as all the necessary work has been integrated into the repo already

Once cloned, navigate to the directory `/packages/svg-icons` and install the needed dependencies: `npm install --only=dev`
You might need to repeat this command on the root folder of the repo as well.

## Regular update steps

 - navigate to the `/packages/svg-icons` directory in your Linux environment that the repo was cloned to (as described above).
	For example:
	```
	cd /source/fluent-ui-systemicons/packages/svg-icons
	```

- Clean out any previous work 
	```
	npm run clean
	```
- Run the build command
	```
	npm run buildforblazor
	```
- Navigate to the created icons folder (under svg-icons)
	```
	cd icons
	```
- Copy the result to your local fast-blazor clone/fork
	```
	rsync -r -v  .  /mnt/c/Source/Blazor/fast-blazor/src/Microsoft.Fast.Components.FluentUI/wwwroot/icons
	```
	The `/mnt/..` path is a WSL2 thing. Adjust accordingly if you are working  on a Mac. Make sure the path in the mnt (c/Source/Blazor/) points to your actual folder where the repo is in.
	
	The `rsync` command should only copy new ad/or update files. 