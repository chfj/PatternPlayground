using System;
using System.Collections.Generic;

namespace Patterns.Proxy.Task;

// Scenario: fetching a video's metadata from RemoteVideoService is
// expensive (pretend it's a network call). The UI asks for the same
// video's metadata repeatedly (once for the thumbnail, once for the title
// bar, once for the "duration" label). Build a caching proxy that looks
// exactly like IVideoService to callers, but only hits the real service
// once per video.

public sealed record VideoMetadata(string VideoId, string Title, TimeSpan Duration);

public interface IVideoService
{
    VideoMetadata GetMetadata(string videoId);
}

public sealed class RemoteVideoService : IVideoService
{
    public int CallCount { get; private set; }

    public VideoMetadata GetMetadata(string videoId)
    {
        CallCount++;
        Console.WriteLine($"[RemoteVideoService] hitting the network for '{videoId}' (call #{CallCount})...");
        // Pretend this took a real network round-trip.
        return new VideoMetadata(videoId, Title: $"Video {videoId}", Duration: TimeSpan.FromMinutes(4));
    }
}

public sealed class CachingVideoServiceProxy(IVideoService realService) : IVideoService
{
    private readonly Dictionary<string, VideoMetadata> _cache = [];

    // TODO: if `videoId` is already in _cache, return the cached value
    // without calling realService. Otherwise call realService.GetMetadata,
    // store the result in _cache, and return it.
    public VideoMetadata GetMetadata(string videoId)
    {
        throw new NotImplementedException();
    }

    public static void Demo()
    {
        var remote = new RemoteVideoService();
        IVideoService proxy = new CachingVideoServiceProxy(remote);

        Console.WriteLine("UI asks for video 'abc123' three times (thumbnail, title, duration label):");
        _ = proxy.GetMetadata("abc123");
        _ = proxy.GetMetadata("abc123");
        var third = proxy.GetMetadata("abc123");
        Console.WriteLine($"Got: {third.Title}, {third.Duration}");

        Console.WriteLine("\nUI asks for a different video 'xyz789':");
        var other = proxy.GetMetadata("xyz789");
        Console.WriteLine($"Got: {other.Title}");

        Console.WriteLine($"\nTotal real network calls made: {remote.CallCount} (should be 2 - one per distinct video)");
    }
}
