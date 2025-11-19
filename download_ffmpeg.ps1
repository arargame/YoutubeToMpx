$ErrorActionPreference = "Stop"
$url = "https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip"
$zipPath = "ffmpeg.zip"
$extractPath = "ffmpeg_temp"

Write-Host "Downloading FFmpeg from $url..."
Invoke-WebRequest -Uri $url -OutFile $zipPath

Write-Host "Extracting..."
Expand-Archive -Path $zipPath -DestinationPath $extractPath -Force

Write-Host "Locating ffmpeg.exe..."
$ffmpegBinary = Get-ChildItem -Path $extractPath -Recurse -Filter "ffmpeg.exe" | Select-Object -First 1

if ($ffmpegBinary) {
    Write-Host "Found ffmpeg.exe at $($ffmpegBinary.FullName)"
    Copy-Item -Path $ffmpegBinary.FullName -Destination ".\ffmpeg.exe" -Force
    Write-Host "ffmpeg.exe copied to project root."
} else {
    Write-Error "ffmpeg.exe not found in the downloaded zip."
}

Write-Host "Cleaning up..."
Remove-Item -Path $zipPath -Force
Remove-Item -Path $extractPath -Recurse -Force

Write-Host "Done! FFmpeg is ready."
