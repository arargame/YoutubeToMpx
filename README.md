# YoutubeToMpx 🎥🎵

**YoutubeToMpx**, YouTube videolarını yüksek kalitede indirmek veya MP3 formatına dönüştürmek için geliştirilmiş, kullanıcı dostu bir .NET konsol uygulamasıdır.

## 🚀 Özellikler

*   **Video İndirme:** YouTube videolarını mevcut en yüksek kalitede (görüntü ve ses birleştirilmiş olarak) indirir.
*   **Ses Dönüştürme:** Videoları sadece ses olarak (MP3 formatında) indirebilirsiniz.
*   **Kalite Seçimi:** Video için mevcut çözünürlük seçeneklerini (1080p, 720p, 480p vb.) listeler ve seçim yapmanıza olanak tanır.
*   **Otomatik Birleştirme:** Yüksek kaliteli video akışları (video-only) ile ses akışlarını (audio-only) otomatik olarak FFmpeg kullanarak birleştirir.
*   **Kullanıcı Dostu Arayüz:** Basit ve anlaşılır konsol arayüzü ile adım adım yönlendirme.

## 🛠️ Teknolojiler ve Kütüphaneler

Bu proje aşağıdaki teknolojiler ve açık kaynak kütüphaneler kullanılarak geliştirilmiştir:

*   **[C# / .NET](https://dotnet.microsoft.com/):** Uygulamanın geliştirildiği ana programlama dili ve platform.
*   **[YoutubeExplode](https://github.com/Tyrrrz/YoutubeExplode):** YouTube verilerini çekmek, video akışlarını çözümlemek ve indirme işlemlerini yönetmek için kullanılan güçlü bir kütüphane.
*   **[YoutubeExplode.Converter](https://github.com/Tyrrrz/YoutubeExplode/tree/master/YoutubeExplode.Converter):** Video ve ses dosyalarını işlemek ve dönüştürmek (Muxing) için kullanılan eklenti.
*   **[FFmpeg](https://ffmpeg.org/):** Video ve ses dosyalarını birleştirmek ve format dönüştürmek için arka planda çalışan medya işleme aracı.

## 📋 Gereksinimler

*   .NET SDK (Proje sürümüne uygun)
*   **ffmpeg.exe:** Uygulamanın çalışabilmesi için `ffmpeg.exe` dosyasının uygulamanın çalıştığı dizinde (veya sistem yolunda) bulunması **ZORUNLUDUR**.

## 💻 Kurulum ve Kullanım

1.  **Projeyi Klonlayın:**
    ```bash
    git clone https://github.com/kullaniciadi/YoutubeToMpx.git
    cd YoutubeToMpx
    ```

2.  **FFmpeg'i Hazırlayın:**
    *   [FFmpeg resmi sitesinden](https://ffmpeg.org/download.html) işletim sisteminize uygun sürümü indirin.
    *   İndirdiğiniz arşivin içindeki `ffmpeg.exe` dosyasını projenin `bin/Debug/netX.X/` (veya yayınladıysanız publish) klasörüne kopyalayın.

3.  **Uygulamayı Derleyin ve Çalıştırın:**
    ```bash
    dotnet build
    dotnet run
    ```

4.  **Adım Adım Kullanım:**
    *   Uygulama açıldığında sizden bir **YouTube Video Linki** isteyecektir. Linki yapıştırın ve `Enter`'a basın.
    *   İndirme konumu sorulacaktır. Varsayılan olarak Masaüstü'ne indirmek için `Enter`'a basabilirsiniz veya farklı bir yol girebilirsiniz.
    *   Uygulama videoyu analiz edecek ve size **İndirme Seçeneklerini** sunacaktır (Örn: Audio Only, 1080p, 720p...).
    *   İstediğiniz seçeneğin yanındaki **numarayı** girin ve `Enter`'a basın.
    *   İndirme işlemi başlayacak ve ilerleme durumu (%) gösterilecektir. İşlem tamamlandığında dosya belirtilen klasörde hazır olacaktır.

## ⚠️ Yasal Uyarı

Bu yazılım sadece eğitim ve kişisel kullanım amaçlıdır. Telif hakkı ile korunan materyallerin izinsiz indirilmesi YouTube Hizmet Koşullarına ve yerel yasalarımıza aykırı olabilir. Kullanıcılar, bu aracı kullanırken ilgili yasalara uymaktan sorumludur.
