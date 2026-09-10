using System;

namespace Patterns.Singleton.Solution;

public sealed class SaveManager
{
    // Private constructor: the only place a SaveManager can be built is
    // inside this class itself, which is what actually prevents a second
    // instance from ever existing.
    private SaveManager()
    {
        Console.WriteLine("[SaveManager] new instance created");
    }

    // System.Lazy<T> defers construction until the first .Value access and
    // guarantees (by default, LazyThreadSafetyMode.ExecutionAndPublication)
    // that concurrent callers can't race each other into building two
    // instances — one thread builds it, the rest just observe the result.
    private static readonly Lazy<SaveManager> _lazyInstance = new(() => new SaveManager());

    public static SaveManager Instance => _lazyInstance.Value;

    private string _saveData = "<empty save>";
    private int _writeCount;

    public void WriteSave(string data)
    {
        // Every caller mutates the SAME field on the SAME object, so the
        // pause menu and the autosave timer are always looking at one
        // source of truth instead of racing two copies of it.
        _saveData = data;
        _writeCount++;
        Console.WriteLine($"[SaveManager] wrote save #{_writeCount}");
    }

    public string ReadSave() => _saveData;

    public int WriteCount => _writeCount;

    public static void Demo()
    {
        Console.WriteLine("Simulating the pause menu grabbing the save manager...");
        var fromPauseMenu = Instance;

        Console.WriteLine("Simulating an autosave timer grabbing it independently...");
        var fromAutosave = Instance;

        // Only ONE "new instance created" line printed above proves the
        // second Instance access reused the same object rather than
        // constructing a fresh one.
        Console.WriteLine($"Same instance? {ReferenceEquals(fromPauseMenu, fromAutosave)}");

        fromPauseMenu.WriteSave("Level 3, 250 gold, boss defeated");
        Console.WriteLine($"Autosave sees: \"{fromAutosave.ReadSave()}\" (write count: {fromAutosave.WriteCount})");
    }
}
