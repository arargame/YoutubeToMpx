using System;
using System.IO;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using YoutubeExplode.Converter;

namespace YoutubeToMpx
{
    public class YoutubeService
    {
        private readonly YoutubeClient _client;

        public YoutubeService()
        {
            _client = new YoutubeClient();
        }

        /// <summary>
        /// Downloads the selected stream (video+audio or audio only) to the given directory.
        /// </summary>
        /// <param name="videoUrl">Full YouTube video URL.</param>
        /// <param name="outputDirectory">Directory where the file will be saved.</param>
        /// <param name="option">The StreamOption selected by the user.</param>
        public async Task DownloadAsync(string videoUrl, string outputDirectory, StreamOption option)
        {
            // Ensure output directory exists (caller already ensures)
            var video = await _client.Videos.GetAsync(videoUrl);
            var manifest = await _client.Videos.Streams.GetManifestAsync(video.Id);

            // Build safe file name
            string safeTitle = string.Join("_", video.Title.Split(Path.GetInvalidFileNameChars()));
            string fileName = $"{safeTitle} ({option.QualityLabel}).{option.Container}";
            string filePath = Path.Combine(outputDirectory, fileName);

            var progress = new Progress<double>(p => Console.Write($"\rProgress: {p:P0}"));

            if (option.Container.Equals("mp3", StringComparison.OrdinalIgnoreCase))
            {
                // Audio only – download and convert to mp3
                await _client.Videos.DownloadAsync(new[] { option.StreamInfo },
                    new ConversionRequestBuilder(filePath).SetContainer("mp3").Build(),
                    progress);
            }
            else
            {
                // Video download – merge with highest‑bitrate audio stream
                var videoInfo = (IVideoStreamInfo)option.StreamInfo!;
                var audioInfo = manifest.GetAudioStreams().GetWithHighestBitrate();
                var streams = audioInfo != null ? new IStreamInfo[] { videoInfo, audioInfo } : new IStreamInfo[] { videoInfo };
                await _client.Videos.DownloadAsync(streams,
                    new ConversionRequestBuilder(filePath).Build(),
                    progress);
            }

            Console.WriteLine("\nDownload complete!");
        }
    }
}
