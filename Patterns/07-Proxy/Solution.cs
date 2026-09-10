using System;
using System.Collections.Generic;

namespace Patterns.Proxy.Solution;

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
        return new VideoMetadata(videoId, Title: $"Video {videoId}", Duration: TimeSpan.FromMinutes(4));
    }
}

// Implements the SAME interface as the real service, so any code written
// against IVideoService can't tell the difference — the proxy just adds a
// cache check in front of the real call.
public sealed class CachingVideoServiceProxy(IVideoService realService) : IVideoService
{
    private readonly Dictionary<string, VideoMetadata> _cache = [];

    public VideoMetadata GetMetadata(string videoId)
    {
        if (_cache.TryGetValue(videoId, out var cached))
        {
            Console.WriteLine($"[CachingVideoServiceProxy] cache hit for '{videoId}'");
            return cached;
        }

        var fresh = realService.GetMetadata(videoId);
        _cache[videoId] = fresh;
        return fresh;
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

        // Three lookups for abc123 + one for xyz789 = 4 calls through the
        // proxy, but only 2 ever reached the "expensive" real service.
        Console.WriteLine($"\nTotal real network calls made: {remote.CallCount} (should be 2 - one per distinct video)");
    }
}
