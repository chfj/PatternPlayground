using System;
using System.Collections.Generic;

namespace Patterns.Command.Task;

// Scenario: a smart-home remote control issues commands to devices (a
// light, a thermostat) and needs to support undo - pressing "undo" reverses
// whatever the last button press did. The remote itself shouldn't need to
// know how to reverse a light vs. a thermostat change; each command knows
// how to undo itself.

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

public sealed class LightOnCommand(Light light) : ICommand
{
    // TODO: turn the light on. Undo should turn it back off.
    public void Execute() => throw new NotImplementedException();
    public void Undo() => throw new NotImplementedException();
}

public sealed class LightOffCommand(Light light) : ICommand
{
    // TODO: turn the light off. Undo should turn it back on.
    public void Execute() => throw new NotImplementedException();
    public void Undo() => throw new NotImplementedException();
}

public sealed class SetTemperatureCommand(Thermostat thermostat, int newTemperatureF) : ICommand
{
    private int _previousTemperatureF;

    // TODO: remember the CURRENT temperature (before the change) in
    // _previousTemperatureF, then set the thermostat to newTemperatureF.
    // Undo should restore _previousTemperatureF.
    public void Execute() => throw new NotImplementedException();
    public void Undo() => throw new NotImplementedException();
}

public sealed class RemoteControl
{
    private readonly Stack<ICommand> _history = new();

    // TODO: run the command, then push it onto _history so it can be undone later.
    public void PressButton(ICommand command)
    {
        throw new NotImplementedException();
    }

    // TODO: pop the most recent command off _history and call its Undo().
    // If _history is empty, do nothing.
    public void PressUndo()
    {
        throw new NotImplementedException();
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
        remote.PressUndo(); // undoes "light off" -> light back on
        remote.PressUndo(); // undoes "set temp to 72" -> back to 68

        Console.WriteLine($"\nState: light.IsOn={light.IsOn}, thermostat={thermostat.TemperatureF}F");
    }
}
