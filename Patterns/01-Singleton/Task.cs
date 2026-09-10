using System;
using System.Threading;

namespace Patterns.Singleton.Task;

// Scenario: a game has exactly one active save slot on disk. Different parts
// of the game (the pause menu, an autosave timer, a "quicksave" hotkey) all
// need to write to and read from that same save slot. If two independent
// SaveManager objects existed, they could race to write the file and corrupt
// it, or disagree about what the "current" save data is.
//
// Your job: make SaveManager a true singleton — exactly one instance for the
// whole process, created lazily the first time someone asks for it, and
// thread-safe to boot.

public sealed class SaveManager
{
    // TODO: this constructor needs to become unreachable from outside the
    // class (that's what actually enforces "only one instance can exist").
    public SaveManager()
    {
        Console.WriteLine("[SaveManager] new instance created");
    }

    // TODO: expose the single shared instance here. Requirements:
    //  - Lazily created on first access (not at class-load time via a
    //    field initializer, and not eagerly before it's needed).
    //  - Safe if two threads call this at the same time (only one
    //    SaveManager should ever get constructed, even under a race).
    // Hint: look at System.Lazy<T> — it gives you both for free.
    public static SaveManager Instance => throw new NotImplementedException();

    private string _saveData = "<empty save>";
    private int _writeCount;

    // TODO: update the shared save data and bump _writeCount.
    public void WriteSave(string data)
    {
        throw new NotImplementedException();
    }

    // TODO: return the current save data.
    public string ReadSave()
    {
        throw new NotImplementedException();
    }

    public int WriteCount => _writeCount;

    public static void Demo()
    {
        Console.WriteLine("Simulating the pause menu grabbing the save manager...");
        var fromPauseMenu = Instance;

        Console.WriteLine("Simulating an autosave timer grabbing it independently...");
        var fromAutosave = Instance;

        Console.WriteLine($"Same instance? {ReferenceEquals(fromPauseMenu, fromAutosave)}");

        fromPauseMenu.WriteSave("Level 3, 250 gold, boss defeated");
        Console.WriteLine($"Autosave sees: \"{fromAutosave.ReadSave()}\" (write count: {fromAutosave.WriteCount})");
    }
}
