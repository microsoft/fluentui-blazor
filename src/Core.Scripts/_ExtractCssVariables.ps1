# ************************************************************
# PowerShell Script to extract all FluentUI CSS Variables
# from the web-components.d.ts file
#
# Usage:
#    1. Set the lastest `@fluentui/web-components` package version in the `package.json`.
#    2. Run `npm install` to install the latest package.
#    3. Open the PowerShell terminal in the `Core.Scripts` directory.
#    4. Run this Script using the following command: `.\_ExtractCssVariables.ps1`
# ************************************************************

# Read the content of the file
$fileContent = Get-Content -Path ".\node_modules\@fluentui\web-components\dist\web-components.d.ts"

# Use a regular expression to find matches
$matches = [regex]::Matches($fileContent, 'export declare const\s+(\w+)\s*=\s*"([^"]+)"')

# Extract the matched item names
$itemNames = foreach ($match in $matches) {
    $name = $match.Groups[1].Value
    $value = $match.Groups[2].Value
    if (-not $name.StartsWith("zIndex")) {
        # Capitalize the first letter, keep the rest
        $capitalized  = $name.Substring(0,1).ToUpper() + $name.Substring(1)
        "/// <summary>"
        "/// $value"
        "/// </summary>"
        "public const string " + $capitalized + " = """ + $value + """;"
        ""  # Empty line
    }
}

# Output the item names
$itemNames
