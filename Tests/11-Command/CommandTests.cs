using Patterns.Command.Task;

namespace Tests;

public class CommandTests
{
    [Fact]
    public void LightOnCommand_Execute_TurnsLightOn()
    {
        var light = new Light();
        var command = new LightOnCommand(light);

        command.Execute();

        Assert.True(light.IsOn);
    }

    [Fact]
    public void LightOnCommand_Undo_TurnsLightBackOff()
    {
        var light = new Light();
        var command = new LightOnCommand(light);
        command.Execute();

        command.Undo();

        Assert.False(light.IsOn);
    }

    [Fact]
    public void LightOffCommand_Undo_TurnsLightBackOn()
    {
        var light = new Light();
        light.TurnOn();
        var command = new LightOffCommand(light);
        command.Execute();

        command.Undo();

        Assert.True(light.IsOn);
    }

    [Fact]
    public void SetTemperatureCommand_Execute_SetsTheNewTemperature()
    {
        var thermostat = new Thermostat();
        var command = new SetTemperatureCommand(thermostat, 72);

        command.Execute();

        Assert.Equal(72, thermostat.TemperatureF);
    }

    [Fact]
    public void SetTemperatureCommand_Undo_RestoresThePreviousTemperature()
    {
        var thermostat = new Thermostat(); // starts at 68
        var command = new SetTemperatureCommand(thermostat, 72);
        command.Execute();

        command.Undo();

        Assert.Equal(68, thermostat.TemperatureF);
    }

    [Fact]
    public void RemoteControl_PressButton_ExecutesTheCommand()
    {
        var light = new Light();
        var remote = new RemoteControl();

        remote.PressButton(new LightOnCommand(light));

        Assert.True(light.IsOn);
    }

    [Fact]
    public void RemoteControl_PressUndo_UndoesOnlyTheMostRecentCommand()
    {
        var light = new Light();
        var thermostat = new Thermostat();
        var remote = new RemoteControl();
        remote.PressButton(new LightOnCommand(light));
        remote.PressButton(new SetTemperatureCommand(thermostat, 72));

        remote.PressUndo();

        Assert.Equal(68, thermostat.TemperatureF);
        Assert.True(light.IsOn);
    }

    [Fact]
    public void RemoteControl_PressUndo_WithEmptyHistory_DoesNothing()
    {
        var remote = new RemoteControl();

        var exception = Record.Exception(() => remote.PressUndo());

        Assert.Null(exception);
    }
}
