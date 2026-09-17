# Define the number of images to download
$numImages = 50

# Define the URL and the desktop path
$url = "https://thispersondoesnotexist.com"
$desktopPath = ".\"

# Create the directory if it doesn't exist
if (-not (Test-Path -Path $desktopPath)) {
    New-Item -ItemType Directory -Path $desktopPath
}

# Function to download and resize images
function Download-And-Resize-Image {
    param (
        [int]$index
    )

    $imagePath = [System.IO.Path]::Combine($desktopPath, "FaceAI-Initial-$index.jpg")
    $resizedImagePath = [System.IO.Path]::Combine($desktopPath, "FaceAI-${index}.jpg")

    # Download the image
    Invoke-WebRequest -Uri $url -OutFile $imagePath

    # Resize the image to 120x120 pixels
    $image = [System.Drawing.Image]::FromFile($imagePath)
    $resizedImage = New-Object System.Drawing.Bitmap(120, 120)
    $graphics = [System.Drawing.Graphics]::FromImage($resizedImage)
    $graphics.DrawImage($image, 0, 0, 120, 120)
    $resizedImage.Save($resizedImagePath, [System.Drawing.Imaging.ImageFormat]::Jpeg)

    # Clean up
    $graphics.Dispose()
    $image.Dispose()
    $resizedImage.Dispose()
}

# Download and resize images
for ($i = 1; $i -le $numImages; $i++) {
    Download-And-Resize-Image -index $i
}
