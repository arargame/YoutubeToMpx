using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using YoutubeExplode.Converter;

namespace YoutubeToMpx
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Background update check
            var updateCheckTask = NuGetVersionChecker.CheckForUpdatesAsync();

            Console.WriteLine("Please enter the YouTube video URL:");
            string videoUrl = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(videoUrl))
            {
                Console.WriteLine("Invalid URL. Please try again:");
                videoUrl = Console.ReadLine();
            }

            Console.WriteLine("Enter the output path (press Enter for Desktop as default):");
            string inputPath = Console.ReadLine();
            string outputDirectory = string.IsNullOrWhiteSpace(inputPath) ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop) : inputPath;

            if (!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not create directory: {outputDirectory}. Error: {ex.Message}");
                    return;
                }
            }

            // Check for FFmpeg
            string ffmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg.exe");
            bool hasFfmpeg = File.Exists(ffmpegPath);
            if (!hasFfmpeg)
            {
                try
                {
                    using var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "ffmpeg",
                        Arguments = "-version",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    });
                    process?.WaitForExit();
                    if (process?.ExitCode == 0) hasFfmpeg = true;
                }
                catch { /* ignore */ }
            }
            if (!hasFfmpeg)
            {
                Console.WriteLine("\n[WARNING] FFmpeg not found!");
                Console.WriteLine("To download high quality videos (1080p+) or MP3, you MUST have ffmpeg.exe in the application folder.");
                Console.WriteLine("Please download ffmpeg.exe and place it here: " + AppDomain.CurrentDomain.BaseDirectory);
                Console.WriteLine("Press Enter to continue anyway (functionality will be limited)...");
                Console.ReadLine();
            }

            try
            {
                var youtube = new YoutubeClient();
                var youtubeObject = new YoutubeObject(videoUrl);

                Console.WriteLine("Fetching video metadata...");
                var video = await youtube.Videos.GetAsync(videoUrl);
                youtubeObject.Title = video.Title;
                youtubeObject.Author = video.Author.ChannelTitle;
                youtubeObject.Duration = video.Duration ?? TimeSpan.Zero;

                Console.WriteLine($"\nVideo Found: {youtubeObject.Title}");
                Console.WriteLine($"Author: {youtubeObject.Author}");
                Console.WriteLine($"Duration: {youtubeObject.Duration}");

                // Check update check results
                var updateResult = await updateCheckTask;
                if (updateResult.UpdateAvailable)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\n[HINT] A newer version of YoutubeExplode is available: v{updateResult.LatestVersion}");
                    Console.WriteLine("If you experience errors, please run 'UpdateDeps.ps1' to update.");
                    Console.ResetColor();
                }

                Console.WriteLine("\nGetting available streams...");
                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
                youtubeObject.Manifest = streamManifest;

                int index = 0;
                // Audio only (MP3)
                var audioStream = streamManifest.GetAudioStreams().GetWithHighestBitrate();
                if (audioStream != null)
                {
                    var mp3Option = new StreamOption
                    {
                        Id = index++,
                        QualityLabel = "Audio Only",
                        Container = "mp3",
                        Size = $"{audioStream.Size.MegaBytes:F2} MB",
                        StreamInfo = audioStream
                    };
                    youtubeObject.AvailableOptions.Add(mp3Option);
                }
                // Video streams
                var videoStreams = streamManifest.GetVideoStreams()
                    .OrderBy(s => s.VideoQuality)
                    .ToList();
                foreach (var stream in videoStreams)
                {
                    var option = new StreamOption
                    {
                        Id = index++,
                        QualityLabel = stream.VideoQuality.Label,
                        Container = stream.Container.Name,
                        Size = $"{stream.Size.MegaBytes:F2} MB",
                        StreamInfo = stream
                    };
                    youtubeObject.AvailableOptions.Add(option);
                }

                if (!youtubeObject.AvailableOptions.Any())
                {
                    Console.Error.WriteLine("No suitable streams found.");
                    return;
                }

                Console.WriteLine("\nAvailable Options:");
                foreach (var opt in youtubeObject.AvailableOptions)
                {
                    Console.WriteLine(opt.ToString());
                }

                Console.WriteLine("\nPlease select an option by entering the number:");
                string selectionInput = Console.ReadLine();
                int selectedIndex;
                while (!int.TryParse(selectionInput, out selectedIndex) || selectedIndex < 0 || selectedIndex >= youtubeObject.AvailableOptions.Count)
                {
                    Console.WriteLine("Invalid selection. Please enter a valid number from the list:");
                    selectionInput = Console.ReadLine();
                }
                var selectedOption = youtubeObject.AvailableOptions[selectedIndex];

                // Perform download via service
                var youtubeService = new YoutubeService(youtube);
                await youtubeService.DownloadAsync(videoUrl, outputDirectory, selectedOption);
            }
            catch (Exception ex) when (ex.Message.Contains("403"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine("\n[ERROR] YouTube access was forbidden (403).");
                Console.Error.WriteLine("This usually means YouTube has updated its security measures.");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n>>> SOLUTION: Please run 'UpdateDeps.ps1' in the project folder to attempt an automatic update.");
                Console.ResetColor();
                Console.Error.WriteLine($"\nTechnical Details: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"\nAn error occurred: {ex.Message}");
                if (ex.Message.Contains("ffmpeg"))
                {
                    Console.WriteLine("Make sure ffmpeg.exe is in the application folder!");
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
