using Patterns.Proxy.Task;

namespace Tests;

public class ProxyTests
{
    [Fact]
    public void GetMetadata_CalledTwiceForSameVideo_OnlyHitsRealServiceOnce()
    {
        var remote = new RemoteVideoService();
        IVideoService proxy = new CachingVideoServiceProxy(remote);

        proxy.GetMetadata("abc123");
        proxy.GetMetadata("abc123");

        Assert.Equal(1, remote.CallCount);
    }

    [Fact]
    public void GetMetadata_ReturnsTheSameDataOnACacheHit()
    {
        var remote = new RemoteVideoService();
        IVideoService proxy = new CachingVideoServiceProxy(remote);

        var first = proxy.GetMetadata("abc123");
        var second = proxy.GetMetadata("abc123");

        Assert.Equal(first, second);
    }

    [Fact]
    public void GetMetadata_ForDifferentVideos_HitsRealServiceOncePerDistinctVideo()
    {
        var remote = new RemoteVideoService();
        IVideoService proxy = new CachingVideoServiceProxy(remote);

        proxy.GetMetadata("abc123");
        proxy.GetMetadata("xyz789");
        proxy.GetMetadata("abc123");

        Assert.Equal(2, remote.CallCount);
    }

    [Fact]
    public void GetMetadata_ReturnsCorrectDataForEachDistinctVideo()
    {
        var remote = new RemoteVideoService();
        IVideoService proxy = new CachingVideoServiceProxy(remote);

        var abc = proxy.GetMetadata("abc123");
        var xyz = proxy.GetMetadata("xyz789");

        Assert.Equal("abc123", abc.VideoId);
        Assert.Equal("xyz789", xyz.VideoId);
    }
}
