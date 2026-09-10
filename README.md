# PatternPlayground

A hands-on C# tutorial covering all 15 classic (GoF) design patterns.
Each pattern has its own folder with a short README, a `Task.cs`
exercise you fill in, and a `Solution.cs` reference implementation — plus
an interactive Runner console app that can demo every pattern's finished
solution right away, no setup required.

## How to run

```
dotnet run --project Runner
```

Pick a number 1-15 to see that pattern's reference solution run, or `0`
to exit. To work through the exercises yourself, open a pattern's
`Task.cs`, read its `README.md`, fill in the `// TODO`s, and compare
your result against `Solution.cs` when you're done.

## Patterns

| # | Pattern | What it's for |
|---|---------|----------------|
| 1 | [Singleton](Patterns/01-Singleton/README.md) | Guarantee a single, globally reachable instance of something that must not be duplicated. |
| 2 | [Factory Method](Patterns/02-FactoryMethod/README.md) | Let subclasses/callers decide which concrete type to instantiate behind a common interface. |
| 3 | [Builder](Patterns/03-Builder/README.md) | Construct a complex object step by step, keeping optional parts out of a giant constructor. |
| 4 | [Adapter](Patterns/04-Adapter/README.md) | Make an existing interface work with client code that expects a different one. |
| 5 | [Decorator](Patterns/05-Decorator/README.md) | Attach behavior to an object dynamically without touching its class or its siblings. |
| 6 | [Facade](Patterns/06-Facade/README.md) | Offer one simple entry point over a set of complex, interdependent subsystems. |
| 7 | [Proxy](Patterns/07-Proxy/README.md) | Control access to an expensive or sensitive object via a stand-in with the same interface. |
| 8 | [Composite](Patterns/08-Composite/README.md) | Treat individual objects and groups of objects through the same interface. |
| 9 | [Observer](Patterns/09-Observer/README.md) | Notify a dynamic set of interested parties whenever an object's state changes. |
| 10 | [Strategy](Patterns/10-Strategy/README.md) | Swap an algorithm's implementation at runtime without changing the code that uses it. |
| 11 | [Command](Patterns/11-Command/README.md) | Turn a request into an object so it can be queued, logged, or undone. |
| 12 | [Iterator](Patterns/12-Iterator/README.md) | Traverse a collection's elements without exposing how it's stored internally. |
| 13 | [State](Patterns/13-State/README.md) | Let an object change its behavior when its internal state changes, without giant if/switch blocks. |
| 14 | [Template Method](Patterns/14-TemplateMethod/README.md) | Fix the skeleton of an algorithm in a base class, letting subclasses fill in the steps. |
| 15 | [Chain of Responsibility](Patterns/15-ChainOfResponsibility/README.md) | Pass a request along a chain of handlers until one of them handles it. |

## Layout

```
DesignPatterns.sln
/Patterns
  /01-Singleton
    Task.cs        <- exercise scaffold with // TODOs
    Solution.cs     <- reference implementation
    README.md       <- problem, when (not) to use it, analogy, smell
  ...
/Runner
  Program.cs        <- interactive console menu
  Runner.csproj      <- single console project; compiles all of /Patterns too
```

`Runner.csproj` is the only project in the solution — it wildcard-includes
every `.cs` file under `/Patterns`, so `Task.cs` and `Solution.cs` for
every pattern compile into one assembly. Each pattern uses its own
namespace pair (e.g. `Patterns.Singleton.Task` / `Patterns.Singleton.Solution`)
so the exercise scaffolding and the reference answer never collide.
