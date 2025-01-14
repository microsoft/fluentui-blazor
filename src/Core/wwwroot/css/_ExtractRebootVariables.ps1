# Path to your file
$filePath = "C:\VSO\Perso\fluentui-blazor-v5\src\Core\wwwroot\css\reboot.css"

# Read the file content
$content = Get-Content -Path $filePath -Raw

# Use regular expression to find all variable names starting with '--'
$variables = [regex]::Matches($content, "--[\w-]+")

# Extract the variable names and remove duplicates
$uniqueVariables = $variables | Select-Object -Unique -Property Value

# Format the output as a JavaScript array
$jsArray = $uniqueVariables.Value -join "', '"
$jsArray = "['$jsArray']"

# Output the JavaScript array
$jsArray

################################################################################
#
# Add the generate jsArray to this JavaScript code and run it to display
# the variables defined in the reboot.css file.
#
# <script>
#     window.onload = function() {
#         setTimeout(function() {
#             console.log('👉 Checking if Reboot CSS variables are defined...');
#             console.log();
# 
#             const root = document.documentElement;
#             const variables = [<REBOOT_VARIABLES>];   // 👈 Replace <REBOOT_VARIABLES> with the generated $jsArray
# 
#             variables.forEach(variable => {
#                 const isDefined = getComputedStyle(root).getPropertyValue(variable).trim() !== '';
#                 console.log(`${isDefined ? '✅' : '❌'} ${variable}`);
#             });
#         }, 1000);
#     }
# </script>
#
################################################################################
