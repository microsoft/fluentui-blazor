$e = "D:\Source\FluentUI\fluentui-blazor\users\vnbaaij\dev-v5\charts\examples"

# Use regex replacements so we can anchor to word boundary: match " Color = " not "CustomColor = "
# Pattern: match Color property assignment preceded by whitespace, comma, or opening brace
$rs = @(
  @{ F = '(?<=[,\s{])Color = "#637cef"'; T = 'Color = DataVizPalette.Color1' }
  @{ F = '(?<=[,\s{])Color = "#e3008c"'; T = 'Color = DataVizPalette.Color2' }
  @{ F = '(?<=[,\s{])Color = "#2aa0a4"'; T = 'Color = DataVizPalette.Color3' }
  @{ F = '(?<=[,\s{])Color = "#9373c0"'; T = 'Color = DataVizPalette.Color4' }
  @{ F = '(?<=[,\s{])Color = "#13a10e"'; T = 'Color = DataVizPalette.Color5' }
  @{ F = '(?<=[,\s{])Color = "#3a96dd"'; T = 'Color = DataVizPalette.Color6' }
  @{ F = '(?<=[,\s{])Color = "#ca5010"'; T = 'Color = DataVizPalette.Color7' }
  @{ F = '(?<=[,\s{])Color = "#57811b"'; T = 'Color = DataVizPalette.Color8' }
  @{ F = '(?<=[,\s{])Color = "#4F68ED"'; T = 'Color = DataVizPalette.Color21' }
  @{ F = '(?<=[,\s{])Color = "#AE8C00"'; T = 'Color = DataVizPalette.Color10' }
  @{ F = '(?<=[,\s{])Color = "#ae8c00"'; T = 'Color = DataVizPalette.Color10' }
  @{ F = '(?<=[,\s{])Color = "#0099BC"'; T = 'Color = DataVizPalette.Custom, CustomColor = "#0099BC"' }
  @{ F = '(?<=[,\s{])Color = "#77004D"'; T = 'Color = DataVizPalette.Custom, CustomColor = "#77004D"' }
  @{ F = '(?<=[,\s{])Color = "#004E8C"'; T = 'Color = DataVizPalette.Custom, CustomColor = "#004E8C"' }
  @{ F = '(?<=[,\s{])Color = "#881798"'; T = 'Color = DataVizPalette.Custom, CustomColor = "#881798"' }
  @{ F = '(?<=[,\s{])Color = "#570078"'; T = 'Color = DataVizPalette.Custom, CustomColor = "#570078"' }
)
$n = 0
Get-ChildItem $e -Filter "*.razor" -Recurse | ForEach-Object {
  $p = $_.FullName
  $c = [IO.File]::ReadAllText($p)
  $orig = $c
  foreach ($r in $rs) { $c = [regex]::Replace($c, $r.F, $r.T) }
  if ($c -ne $orig) {
    [IO.File]::WriteAllText($p, $c)
    $n++
    Write-Output "Updated: $($_.Name)"
  }
}
Write-Output "Done: $n files updated"

