Add-Type -AssemblyName System.Drawing

$baseDir = "D:\App Development Projects\Sim Gamepad WPF\Sim Gamepad\assets"
$pngPath = Join-Path $baseDir "icon.png"

$img = [System.Drawing.Image]::FromFile($pngPath)

# 1) Generate the installer Wizard images (BMP)
$bmpSmall = New-Object System.Drawing.Bitmap 55, 55
$gSmall = [System.Drawing.Graphics]::FromImage($bmpSmall)
$gSmall.Clear([System.Drawing.Color]::White)
# Center icon in 55x55
$gSmall.DrawImage($img, 5, 5, 45, 45)
$bmpSmall.Save((Join-Path $baseDir "wizard_small.bmp"), [System.Drawing.Imaging.ImageFormat]::Bmp)

$bmpLarge = New-Object System.Drawing.Bitmap 164, 314
$gLarge = [System.Drawing.Graphics]::FromImage($bmpLarge)
$gLarge.Clear([System.Drawing.Color]::White)
# Center icon in the middle
$gLarge.DrawImage($img, 10, 85, 144, 144)
$bmpLarge.Save((Join-Path $baseDir "wizard_large.bmp"), [System.Drawing.Imaging.ImageFormat]::Bmp)

# 2) Generate a purely standard Windows ICO via unmanaged DIB conversion
# Resize to 256x256 first for best compatibility
$icoBitmap = New-Object System.Drawing.Bitmap 256, 256
$gIco = [System.Drawing.Graphics]::FromImage($icoBitmap)
$gIco.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
$gIco.DrawImage($img, 0, 0, 256, 256)

$hIcon = $icoBitmap.GetHicon()
$ico = [System.Drawing.Icon]::FromHandle($hIcon)
$fs = New-Object System.IO.FileStream (Join-Path $baseDir "icon.ico"), Create
$ico.Save($fs)
$fs.Close()

# Clean up unmanaged handle
[System.Runtime.InteropServices.Marshal]::DestroyIcon($hIcon) | Out-Null

$gSmall.Dispose(); $bmpSmall.Dispose()
$gLarge.Dispose(); $bmpLarge.Dispose()
$gIco.Dispose(); $icoBitmap.Dispose()
$img.Dispose()

Write-Host "Successfully generated valid ICO and Wizard BMPs."
