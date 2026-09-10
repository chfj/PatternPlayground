using Patterns.Composite.Task;

namespace Tests;

public class CompositeTests
{
    [Fact]
    public void Unit_TakeDamage_ReducesHealth()
    {
        var unit = new Unit("Archer", 30);

        unit.TakeDamage(10);

        Assert.Equal(20, unit.TotalHealth);
    }

    [Fact]
    public void Unit_TakeDamage_ClampsAtZeroInsteadOfGoingNegative()
    {
        var unit = new Unit("Archer", 30);

        unit.TakeDamage(999);

        Assert.Equal(0, unit.TotalHealth);
    }

    [Fact]
    public void Squad_TotalHealth_SumsAllDirectMembers()
    {
        var squad = new Squad("Main Force");
        squad.Add(new Unit("A", 30));
        squad.Add(new Unit("B", 20));

        Assert.Equal(50, squad.TotalHealth);
    }

    [Fact]
    public void Squad_TotalHealth_IncludesNestedSubSquads()
    {
        var subSquad = new Squad("Scouts");
        subSquad.Add(new Unit("Scout-1", 20));
        subSquad.Add(new Unit("Scout-2", 20));

        var mainForce = new Squad("Main Force");
        mainForce.Add(new Unit("Archer", 30));
        mainForce.Add(subSquad);

        Assert.Equal(70, mainForce.TotalHealth);
    }

    [Fact]
    public void Squad_TakeDamage_AppliesToEveryMemberIncludingNestedSquads()
    {
        var subSquad = new Squad("Scouts");
        subSquad.Add(new Unit("Scout-1", 20));

        var mainForce = new Squad("Main Force");
        mainForce.Add(new Unit("Archer", 30));
        mainForce.Add(subSquad);

        mainForce.TakeDamage(10);

        // Archer: 30-10=20, Scout-1 (via subSquad): 20-10=10 -> total 30
        Assert.Equal(30, mainForce.TotalHealth);
    }

    [Fact]
    public void Squad_Move_AppliesToEveryMemberByName()
    {
        var squad = new Squad("Main Force");
        squad.Add(new Unit("Archer-1", 30));
        squad.Add(new Unit("Archer-2", 30));

        var output = ConsoleCapture.Capture(() => squad.Move(5, 0));

        Assert.Contains("Archer-1", output);
        Assert.Contains("Archer-2", output);
    }
}
