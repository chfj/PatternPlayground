using System;
using System.Collections.Generic;

namespace Patterns.Command.Solution;

public sealed class Light
{
    public bool IsOn { get; private set; }

    public void TurnOn()
    {
        IsOn = true;
        Console.WriteLine("Light: ON");
    }

    public void TurnOff()
    {
        IsOn = false;
        Console.WriteLine("Light: OFF");
    }
}

public sealed class Thermostat
{
    public int TemperatureF { get; private set; } = 68;

    public void SetTemperature(int degreesF)
    {
        TemperatureF = degreesF;
        Console.WriteLine($"Thermostat: {TemperatureF}F");
    }
}

public interface ICommand
{
    void Execute();
    void Undo();
}

// Each command wraps a "receiver" (the Light/Thermostat) plus whatever
// parameters it needs, and knows how to both do and undo its own action.
// RemoteControl never needs an if/switch on "what kind of command is this?"
public sealed class LightOnCommand(Light light) : ICommand
{
    public void Execute() => light.TurnOn();
    public void Undo() => light.TurnOff();
}

public sealed class LightOffCommand(Light light) : ICommand
{
    public void Execute() => light.TurnOff();
    public void Undo() => light.TurnOn();
}

public sealed class SetTemperatureCommand(Thermostat thermostat, int newTemperatureF) : ICommand
{
    // A command that changes state (rather than toggling a boolean) has to
    // capture whatever it's about to overwrite BEFORE overwriting it - that
    // captured value is what makes Undo possible.
    private int _previousTemperatureF;

    public void Execute()
    {
        _previousTemperatureF = thermostat.TemperatureF;
        thermostat.SetTemperature(newTemperatureF);
    }

    public void Undo() => thermostat.SetTemperature(_previousTemperatureF);
}

public sealed class RemoteControl
{
    // A stack of executed commands is the "undo history" - undo just pops
    // the most recent one and asks IT to reverse itself.
    private readonly Stack<ICommand> _history = new();

    public void PressButton(ICommand command)
    {
        command.Execute();
        _history.Push(command);
    }

    public void PressUndo()
    {
        if (_history.Count == 0)
        {
            return;
        }
        var lastCommand = _history.Pop();
        lastCommand.Undo();
    }

    public static void Demo()
    {
        var light = new Light();
        var thermostat = new Thermostat();
        var remote = new RemoteControl();

        Console.WriteLine("Pressing: light on, set temp to 72, light off");
        remote.PressButton(new LightOnCommand(light));
        remote.PressButton(new SetTemperatureCommand(thermostat, 72));
        remote.PressButton(new LightOffCommand(light));

        Console.WriteLine($"\nState: light.IsOn={light.IsOn}, thermostat={thermostat.TemperatureF}F");

        Console.WriteLine("\nPressing undo twice:");
        remote.PressUndo();
        remote.PressUndo();

        Console.WriteLine($"\nState: light.IsOn={light.IsOn}, thermostat={thermostat.TemperatureF}F");
    }
}
