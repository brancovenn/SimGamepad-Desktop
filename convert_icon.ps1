Add-Type -AssemblyName System.Drawing
$pngPath = "$PSScriptRoot\assets\icon.png"
$icoPath = "$PSScriptRoot\assets\icon.ico"

$img = [System.Drawing.Image]::FromFile($pngPath)
$w = $img.Width; if ($w -ge 256) { $w = 0 }
$h = $img.Height; if ($h -ge 256) { $h = 0 }
$img.Dispose()

$pngBytes = [System.IO.File]::ReadAllBytes($pngPath)
$fs = [System.IO.File]::Open($icoPath, [System.IO.FileMode]::Create)
$ws = [System.IO.BinaryWriter]::new($fs)

$ws.Write([System.Int16]0)
$ws.Write([System.Int16]1)
$ws.Write([System.Int16]1)

$ws.Write([byte]$w)
$ws.Write([byte]$h)
$ws.Write([byte]0)
$ws.Write([byte]0)
$ws.Write([System.Int16]1)
$ws.Write([System.Int16]32)
$ws.Write([System.Int32]$pngBytes.Length)
$ws.Write([System.Int32]22)

$ws.Write($pngBytes)
$ws.Close()
$fs.Close()
Write-Host "Successfully generated valid $icoPath"
