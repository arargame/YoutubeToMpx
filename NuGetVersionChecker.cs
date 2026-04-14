using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Reflection;
using YoutubeExplode;

namespace YoutubeToMpx
{
    public static class NuGetVersionChecker
    {
        private const string PackageName = "YoutubeExplode";
        private const string NuGetUrl = "https://api.nuget.org/v3-flatcontainer/youtubeexplode/index.json";

        public static async Task<(bool UpdateAvailable, string LatestVersion)> CheckForUpdatesAsync()
        {
            try
            {
                using var client = new HttpClient();
                // Set timeout to avoid hanging the app startup
                client.Timeout = TimeSpan.FromSeconds(5);
                
                var response = await client.GetStringAsync(NuGetUrl);
                var doc = JsonDocument.Parse(response);
                
                var versions = doc.RootElement.GetProperty("versions").EnumerateArray()
                    .Select(v => v.GetString())
                    .Where(v => !string.IsNullOrEmpty(v))
                    .ToList();

                string? latestVersion = versions.LastOrDefault();
                if (string.IsNullOrEmpty(latestVersion)) return (false, "");

                // Get current version from the assembly
                var currentVersion = typeof(YoutubeClient).Assembly.GetName().Version?.ToString(3);
                
                if (currentVersion != null && IsNewer(latestVersion, currentVersion))
                {
                    return (true, latestVersion);
                }
            }
            catch
            {
                // Silently fail if no internet or NuGet is down
            }

            return (false, "");
        }

        private static bool IsNewer(string latest, string current)
        {
            if (Version.TryParse(latest, out var vLatest) && Version.TryParse(current, out var vCurrent))
            {
                return vLatest > vCurrent;
            }
            return false;
        }
    }
}
