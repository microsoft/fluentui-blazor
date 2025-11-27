param(
    [Parameter(Mandatory=$true)]
    [string]$newTarget
)

Write-Host "Updating TargetFrameworks to $newTarget..."

Get-ChildItem -Recurse -Filter *.csproj | ForEach-Object {
    $content = Get-Content $_.FullName -Raw
    if ($content -match '<TargetFrameworks>.*?</TargetFrameworks>') {
        $content = $content -replace '<TargetFrameworks>.*?</TargetFrameworks>', "<TargetFrameworks>$newTarget</TargetFrameworks>"
        Set-Content -Path $_.FullName -Value $content -NoNewline
        Write-Host "Updated $($_.FullName)"
    }
}

Write-Host "Done updating TargetFrameworks."
