using System;
using System.Collections.Generic;
using YoutubeExplode.Videos.Streams;

namespace YoutubeToMpx
{
    public class YoutubeObject
    {
        public string? Url { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public TimeSpan Duration { get; set; }
        
        // Selected Stream Details
        public string? Quality { get; set; }
        public string? Format { get; set; }
        public long FileSize { get; set; }
        public string? Container { get; set; }
        public string? Resolution { get; set; }
        public int Bitrate { get; set; }

        // Data for selection
        public StreamManifest? Manifest { get; set; }
        public List<StreamOption> AvailableOptions { get; set; } = new List<StreamOption>();

        public YoutubeObject()
        {
        }

        public YoutubeObject(string url)
        {
            Url = url;
        }

        public override string ToString()
        {
            return $"Title: {Title}\n" +
                   $"Author: {Author}\n" +
                   $"Duration: {Duration}\n" +
                   $"Selected Quality: {Quality}\n" +
                   $"Selected Format: {Format}\n" +
                   $"Selected Container: {Container}\n";
        }
    }

    public class StreamOption
    {
        public int Id { get; set; }
        public string? QualityLabel { get; set; }
        public string? Container { get; set; }
        public string? Size { get; set; }
        public IStreamInfo? StreamInfo { get; set; }

        public override string ToString()
        {
            return $"{Id}. {QualityLabel} - {Container} ({Size})";
        }
    }
}
