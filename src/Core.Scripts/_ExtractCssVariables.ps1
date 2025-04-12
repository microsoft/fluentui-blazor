# ************************************************************
# PowerShell Script to extract all FluentUI CSS Variables
# from the web-components.d.ts file
#
# Usage:
#    1. Set the lastest `@fluentui/web-components` package version in the `package.json`.
#    2. Run `npm install` to install the latest package.
#    3. Open the PowerShell terminal in the `Core.Scripts` directory.
#    4. Run this Script using the following command: `.\_ExtractCssVariables.ps1`
#    5. Copy the output to the `Styles.cs` file in the `Microsoft.FluentUI.AspNetCore.Components` project.
# ************************************************************

# Read the content of the file
$fileContent = Get-Content -Path ".\node_modules\@fluentui\web-components\dist\web-components.d.ts"

# Use a regular expression to find matches
# Group 1 & 2: Categories, Group 3: Name, Group 4: Variable value
$matches = [regex]::Matches($fileContent, 'export declare const\s+([a-z]+)([A-Z]?[a-z]+)?(\w+)?\s*=\s*"([^"]+)"')

# Dictionary to sort by Group1 and Group2
$result = @{}

foreach ($match in $matches) {
        $g1 = $match.Groups[1].Value + "s"
        $g2 = $match.Groups[2].Value
        $g3 = $match.Groups[3].Value
        $g4 = $match.Groups[4].Value

        # Capitalize the first letter, keep the rest
        $g1 = $g1.Substring(0,1).ToUpper() + $g1.Substring(1)

        if (-not $result.ContainsKey($g1)) {
            $result[$g1] = @{}
        }

        if (-not $result[$g1].ContainsKey($g2)) {
            $result[$g1][$g2] = @()
        }

        $result[$g1][$g2] += [PSCustomObject]@{
            Groupe3 = $g3
            Groupe4 = $g4
        }
}

# Display the result
Write-Output "// ------------------------------------------------------------------------"
Write-Output "// MIT License - Copyright (c) Microsoft Corporation. All rights reserved."
Write-Output "// ------------------------------------------------------------------------"
Write-Output ""
Write-Output "namespace Microsoft.FluentUI.AspNetCore.Components;"
Write-Output ""
Write-Output "/// <summary />"
Write-Output "public class Styles"
Write-Output "{"

foreach ($g1 in $result.Keys) {
    Write-Output "    /// <summary />"
    Write-Output "    public partial class $g1"
    Write-Output "    {"

    # Direct constants
    if ($g1 -eq "Durations" -or $g1 -eq "Shadows") {

        if ($g1 -eq "Shadows") {
            $prefix = $g1
        }
        else {
            $prefix = ""
        }

        foreach ($g2 in $result[$g1].Keys) {
            foreach ($entry in $result[$g1][$g2]) {
                Write-Output "        /// <summary />"
                Write-Output "        public const string $prefix$g2$($entry.Groupe3) = ""$($entry.Groupe4)"";"
                Write-Output ""
            }
        }
    }

     # Sub category: Group2
    else {
        foreach ($g2 in $result[$g1].Keys) {
            Write-Output "        /// <summary />"
            Write-Output "        public partial class $g2"
            Write-Output "        {"
            foreach ($entry in $result[$g1][$g2]) {
                if ($entry.Groupe3 -eq "") {
                    $entry.Groupe3 = "Default"
                }

                Write-Output "            /// <summary />"
                Write-Output "            public const string $($entry.Groupe3) = ""$($entry.Groupe4)"";"
                Write-Output ""
            }
            Write-Output "        }"
        }
    }
    Write-Output "    }"
}
Write-Output "}"

# Extract the matched item names
# $itemNames = foreach ($match in $matches) {
#     $name = $match.Groups[1].Value
#     $value = $match.Groups[2].Value
#     if (-not $name.StartsWith("zIndex")) {
#         # Capitalize the first letter, keep the rest
#         $capitalized  = $name.Substring(0,1).ToUpper() + $name.Substring(1)
#         "/// <summary>"
#         "/// $value"
#         "/// </summary>"
#         "public const string " + $capitalized + " = """ + $value + """;"
#         ""  # Empty line
#     }
# }

# Output the item names
$itemNames
