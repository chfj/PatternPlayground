using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Patterns.Iterator.Solution;

public sealed record Song(string Title, string Artist);

// A hand-rolled IEnumerator: it holds its own position (_index) separate
// from the Playlist itself, so multiple iterators (or multiple foreach
// loops) over the same playlist never interfere with each other.
public sealed class SequentialIterator(IReadOnlyList<Song> songs) : IEnumerator<Song>
{
    private int _index = -1;

    public Song Current => songs[_index];
    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        _index++;
        return _index < songs.Count;
    }

    public void Reset() => _index = -1;
    public void Dispose() { }
}

// A second, independent iterator over the SAME underlying data, using a
// different traversal order. Neither iterator needs to know the other
// exists, and Playlist doesn't need branching logic to support both.
public sealed class ShuffledIterator(IReadOnlyList<Song> songs, int seed) : IEnumerator<Song>
{
    private readonly List<int> _order = ComputeShuffledOrder(songs.Count, seed);
    private int _position = -1;

    public Song Current => songs[_order[_position]];
    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        _position++;
        return _position < _order.Count;
    }

    public void Reset() => _position = -1;
    public void Dispose() { }

    private static List<int> ComputeShuffledOrder(int count, int seed)
    {
        var order = Enumerable.Range(0, count).ToList();
        var rng = new Random(seed);
        for (var i = order.Count - 1; i > 0; i--)
        {
            var j = rng.Next(i + 1);
            (order[i], order[j]) = (order[j], order[i]);
        }
        return order;
    }
}

public sealed class ShuffledView(IReadOnlyList<Song> songs, int seed) : IEnumerable<Song>
{
    public IEnumerator<Song> GetEnumerator() => new ShuffledIterator(songs, seed);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

// Playlist implements IEnumerable<Song> itself (default = sequential order)
// so `foreach` works directly on it, while InShuffledOrder hands out a
// completely different iterator for the same private _songs list - callers
// never see that list directly either way.
public sealed class Playlist : IEnumerable<Song>
{
    private readonly List<Song> _songs = [];

    public void Add(Song song) => _songs.Add(song);

    public IEnumerator<Song> GetEnumerator() => new SequentialIterator(_songs);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerable<Song> InShuffledOrder(int seed) => new ShuffledView(_songs, seed);

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
