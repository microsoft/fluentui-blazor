# ************************************************************
# PowerShell Script to extract all FluentUI Web Components
# from the web-components.d.ts file
#
# Usage:
#    1. Open the PowerShell terminal in the `Core.Scripts` directory.
#    2. Run this Script using the following command: `.\_ExtractWebComponents.ps1`
#    3. Copy the output and paste it in the `FluentUIWebComponents.ts` file.
# ************************************************************

# Read the content of the file
$fileContent = Get-Content -Path ".\node_modules\@fluentui\web-components\dist\web-components.d.ts"

# Use a regular expression to find matches
$matches = [regex]::Matches($fileContent, 'export declare const (\w+): FASTElementDefinition')

# Extract the matched item names
$itemNames = foreach ($match in $matches) { "FluentUIComponents.$($match.Groups[1].Value).define(registry);" }

# Output the item names
$itemNames
