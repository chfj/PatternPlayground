using Patterns.Singleton.Task;

namespace Tests;

public class SingletonTests
{
    [Fact]
    public void Instance_ReturnsTheSameReferenceEveryTime()
    {
        var first = SaveManager.Instance;
        var second = SaveManager.Instance;

        Assert.Same(first, second);
    }

    [Fact]
    public void WriteSave_ThenReadSave_ReturnsWhatWasWritten()
    {
        SaveManager.Instance.WriteSave("Level 5, 900 gold");

        Assert.Equal("Level 5, 900 gold", SaveManager.Instance.ReadSave());
    }

    [Fact]
    public void WriteSave_IncrementsWriteCountByOne()
    {
        var before = SaveManager.Instance.WriteCount;

        SaveManager.Instance.WriteSave("some save data");

        Assert.Equal(before + 1, SaveManager.Instance.WriteCount);
    }

    [Fact]
    public void WriteSave_FromOneReference_IsVisibleThroughAnotherReference()
    {
        var fromPauseMenu = SaveManager.Instance;
        var fromAutosave = SaveManager.Instance;

        fromPauseMenu.WriteSave("shared state check");

        Assert.Equal("shared state check", fromAutosave.ReadSave());
    }
}
