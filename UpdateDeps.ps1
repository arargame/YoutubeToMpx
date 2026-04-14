# YoutubeToMpx Bağımlılık Güncelleyici
# Bu betik YoutubeExplode kütüphanesini en son sürüme günceller ve projeyi yeniden derler.

$ErrorActionPreference = "Stop"

Write-Host "`n>>> YouTube Paketleri Güncelleniyor..." -ForegroundColor Cyan

try {
    # NuGet paketlerini en son sürüme güncelle
    dotnet add package YoutubeExplode
    dotnet add package YoutubeExplode.Converter

    Write-Host ">>> Geçici dosyalar temizleniyor..." -ForegroundColor Cyan
    if (Test-Path "bin") { Remove-Item -Recurse -Force "bin" }
    if (Test-Path "obj") { Remove-Item -Recurse -Force "obj" }

    Write-Host ">>> Proje yeniden derleniyor..." -ForegroundColor Cyan
    dotnet build

    Write-Host "`n[BAŞARILI] Güncelleme tamamlandı! Uygulamayı yeniden başlatabilirsiniz.`n" -ForegroundColor Green
}
catch {
    Write-Host "`n[HATA] Güncelleme sırasında bir sorun oluştu: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "Çıkmak için bir tuşa basın..."
$null = [Console]::ReadKey()
