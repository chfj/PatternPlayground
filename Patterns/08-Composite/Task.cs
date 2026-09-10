using System;
using System.Collections.Generic;
using System.Linq;

namespace Patterns.Composite.Task;

// Scenario: a game needs to apply area-of-effect damage and movement
// orders uniformly to either a single unit or an entire squad — and a
// squad can itself contain smaller sub-squads. Model both "leaf" units and
// "composite" squads behind one IGameEntity interface so calling code never
// has to check "is this one unit or a group?"

public interface IGameEntity
{
    string Name { get; }
    int TotalHealth { get; }
    void TakeDamage(int amount);
    void Move(int dx, int dy);
}

public sealed class Unit(string name, int health) : IGameEntity
{
    public string Name { get; } = name;
    public int TotalHealth { get; private set; } = health;

    // TODO: reduce TotalHealth by `amount`, clamped at 0 (never negative).
    public void TakeDamage(int amount)
    {
        throw new NotImplementedException();
    }

    // TODO: print something like "<Name> moves by (dx, dy)".
    public void Move(int dx, int dy)
    {
        throw new NotImplementedException();
    }
}

public sealed class Squad(string name) : IGameEntity
{
    private readonly List<IGameEntity> _members = [];

    public string Name { get; } = name;

    public void Add(IGameEntity member) => _members.Add(member);

    // TODO: a squad's total health is the sum of every member's
    // TotalHealth (members can themselves be squads - that's fine, just
    // read their TotalHealth recursively via this same property).
    public int TotalHealth => throw new NotImplementedException();

    // TODO: apply TakeDamage(amount) to every member (AoE damage hits the
    // whole squad, sub-squads included).
    public void TakeDamage(int amount)
    {
        throw new NotImplementedException();
    }

    // TODO: apply Move(dx, dy) to every member.
    public void Move(int dx, int dy)
    {
        throw new NotImplementedException();
    }

    public static void Demo()
    {
        var archer1 = new Unit("Archer-1", 30);
        var archer2 = new Unit("Archer-2", 30);

        var scoutSquad = new Squad("Scout Squad");
        scoutSquad.Add(new Unit("Scout-1", 20));
        scoutSquad.Add(new Unit("Scout-2", 20));

        var mainForce = new Squad("Main Force");
        mainForce.Add(archer1);
        mainForce.Add(archer2);
        mainForce.Add(scoutSquad); // a squad nested inside a squad

        Console.WriteLine($"{mainForce.Name} total health: {mainForce.TotalHealth}");

        Console.WriteLine("\nEnemy casts an AoE fireball for 10 damage on Main Force:");
        mainForce.TakeDamage(10);
        Console.WriteLine($"{mainForce.Name} total health after fireball: {mainForce.TotalHealth}");

        Console.WriteLine("\nMain Force advances (dx=5, dy=0):");
        mainForce.Move(5, 0);
    }
}
