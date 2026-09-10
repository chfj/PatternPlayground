using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Patterns.Iterator.Task;

// Scenario: a music Playlist needs to support two traversal orders -
// sequential (track 1, 2, 3...) and shuffled - without exposing its
// backing List<Song> to callers. Each traversal order is its own iterator;
// Playlist just hands one out.

public sealed record Song(string Title, string Artist);

public sealed class SequentialIterator(IReadOnlyList<Song> songs) : IEnumerator<Song>
{
    private int _index = -1;

    // TODO: return the song at the current position.
    public Song Current => throw new NotImplementedException();

    object IEnumerator.Current => Current;

    // TODO: advance to the next position; return true if there is a song
    // there, false if we've walked off the end.
    public bool MoveNext()
    {
        throw new NotImplementedException();
    }

    public void Reset() => _index = -1;
    public void Dispose() { }
}

public sealed class ShuffledIterator(IReadOnlyList<Song> songs, int seed) : IEnumerator<Song>
{
    // Precomputed once, so every MoveNext() just walks this fixed order.
    private readonly List<int> _order = ComputeShuffledOrder(songs.Count, seed);
    private int _position = -1;

    // TODO: return the song at _order[_position] (i.e. look up the shuffled
    // index, then index into `songs`).
    public Song Current => throw new NotImplementedException();

    object IEnumerator.Current => Current;

    // TODO: same idea as SequentialIterator.MoveNext, but walking _order
    // instead of `songs` directly.
    public bool MoveNext()
    {
        throw new NotImplementedException();
    }

    public void Reset() => _position = -1;
    public void Dispose() { }

    private static List<int> ComputeShuffledOrder(int count, int seed)
    {
        var order = Enumerable.Range(0, count).ToList();
        var rng = new Random(seed); // fixed seed -> reproducible "shuffle" for the demo
        for (var i = order.Count - 1; i > 0; i--)
        {
            var j = rng.Next(i + 1);
            (order[i], order[j]) = (order[j], order[i]);
        }
        return order;
    }
}

// A tiny adapter so `foreach (var song in playlist.InShuffledOrder(seed))`
// works - foreach needs something with a GetEnumerator(), and
// ShuffledIterator alone is only the enumerator, not the enumerable.
public sealed class ShuffledView(IReadOnlyList<Song> songs, int seed) : IEnumerable<Song>
{
    public IEnumerator<Song> GetEnumerator() => new ShuffledIterator(songs, seed);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public sealed class Playlist : IEnumerable<Song>
{
    private readonly List<Song> _songs = [];

    public void Add(Song song) => _songs.Add(song);

    // TODO: return a SequentialIterator over _songs. This is what makes
    // `foreach (var song in playlist)` walk tracks in order.
    public IEnumerator<Song> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // TODO: return a ShuffledView over _songs using the given seed.
    public IEnumerable<Song> InShuffledOrder(int seed)
    {
        throw new NotImplementedException();
    }

    public static void Demo()
    {
        var playlist = new Playlist();
        playlist.Add(new Song("Song A", "Artist 1"));
        playlist.Add(new Song("Song B", "Artist 2"));
        playlist.Add(new Song("Song C", "Artist 3"));
        playlist.Add(new Song("Song D", "Artist 4"));

        Console.WriteLine("Sequential playback:");
        foreach (var song in playlist)
        {
            Console.WriteLine($"  {song.Title} - {song.Artist}");
        }

        Console.WriteLine("\nShuffled playback (seed 42):");
        foreach (var song in playlist.InShuffledOrder(42))
        {
            Console.WriteLine($"  {song.Title} - {song.Artist}");
        }
    }
}
