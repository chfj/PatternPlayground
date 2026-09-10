using System;
using System.Collections.Generic;
using System.Linq;

namespace Patterns.Composite.Solution;

public interface IGameEntity
{
    string Name { get; }
    int TotalHealth { get; }
    void TakeDamage(int amount);
    void Move(int dx, int dy);
}

// The leaf: does the real work, with no knowledge that it might be part of
// a larger group.
public sealed class Unit(string name, int health) : IGameEntity
{
    public string Name { get; } = name;
    public int TotalHealth { get; private set; } = health;

    public void TakeDamage(int amount)
    {
        TotalHealth = Math.Max(0, TotalHealth - amount);
        Console.WriteLine($"{Name} takes {amount} damage (health: {TotalHealth})");
    }

    public void Move(int dx, int dy) => Console.WriteLine($"{Name} moves by ({dx}, {dy})");
}

// The composite: implements the SAME interface as a Unit, but every
// operation just forwards to its members. Because a member can itself be a
// Squad, calling TakeDamage/Move/TotalHealth on the top-level squad
// recurses through the whole tree for free — no special-casing needed
// anywhere for "is this a leaf or a group?"
public sealed class Squad(string name) : IGameEntity
{
    private readonly List<IGameEntity> _members = [];

    public string Name { get; } = name;

    public void Add(IGameEntity member) => _members.Add(member);

    public int TotalHealth => _members.Sum(member => member.TotalHealth);

    public void TakeDamage(int amount)
    {
        foreach (var member in _members)
        {
            member.TakeDamage(amount);
        }
    }

    public void Move(int dx, int dy)
    {
        foreach (var member in _members)
        {
            member.Move(dx, dy);
        }
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
        mainForce.Add(scoutSquad);

        Console.WriteLine($"{mainForce.Name} total health: {mainForce.TotalHealth}");

        Console.WriteLine("\nEnemy casts an AoE fireball for 10 damage on Main Force:");
        mainForce.TakeDamage(10); // recurses into scoutSquad automatically
        Console.WriteLine($"{mainForce.Name} total health after fireball: {mainForce.TotalHealth}");

        Console.WriteLine("\nMain Force advances (dx=5, dy=0):");
        mainForce.Move(5, 0);
    }
}
