using Patterns.Iterator.Task;

namespace Tests;

public class IteratorTests
{
    private static Playlist CreateSamplePlaylist()
    {
        var playlist = new Playlist();
        playlist.Add(new Song("Song A", "Artist 1"));
        playlist.Add(new Song("Song B", "Artist 2"));
        playlist.Add(new Song("Song C", "Artist 3"));
        return playlist;
    }

    [Fact]
    public void Foreach_WalksSongsInSequentialOrder()
    {
        var playlist = CreateSamplePlaylist();

        var titles = playlist.Select(s => s.Title).ToList();

        Assert.Equal(["Song A", "Song B", "Song C"], titles);
    }

    [Fact]
    public void SequentialIterator_MoveNext_ReturnsFalseAfterTheLastSong()
    {
        var playlist = CreateSamplePlaylist();
        using var enumerator = playlist.GetEnumerator();

        while (enumerator.MoveNext())
        {
        }

        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    public void InShuffledOrder_VisitsEverySongExactlyOnce()
    {
        var playlist = CreateSamplePlaylist();

        var shuffledTitles = playlist.InShuffledOrder(42).Select(s => s.Title).ToList();

        Assert.Equal(3, shuffledTitles.Count);
        Assert.Equal(new[] { "Song A", "Song B", "Song C" }, shuffledTitles.OrderBy(t => t));
    }

    [Fact]
    public void InShuffledOrder_WithSameSeed_ProducesTheSameOrderEveryTime()
    {
        var playlist = CreateSamplePlaylist();

        var first = playlist.InShuffledOrder(42).Select(s => s.Title).ToList();
        var second = playlist.InShuffledOrder(42).Select(s => s.Title).ToList();

        Assert.Equal(first, second);
    }
}
