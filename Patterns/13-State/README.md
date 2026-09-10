# State

**Problem it solves:** an object's allowed behavior depends on which
"mode" or lifecycle stage it's currently in, and encoding that as one
big `if`/`switch` on a status field, repeated in every method, gets
unmanageable as states and transitions grow.

**When to use it**
- An object has a well-defined set of states (Placed, Paid, Shipped,
  Delivered, Cancelled) where the same action (e.g. "cancel") behaves
  differently — or isn't allowed at all — depending on the current
  state.
- You want adding a new state or changing one state's rules to touch
  one place, not every method that currently checks
  `if (status == X) ... else if (status == Y) ...`.

**When NOT to use it (anti-example)**
- A simple boolean flag (`IsActive`) with one or two conditionals
  doesn't need this — a full State pattern for two states is more
  ceremony than a single `if`.
- Don't use it when transitions don't actually change *behavior*, only
  a label — if every state responds identically to every action and the
  "state" is purely informational, a plain enum field is enough.

**Real-world analogy:** a vending machine. "Insert coin" does something
different depending on whether the machine is idle, already has a coin
in it, or is dispensing — same button, different behavior, because the
machine's internal state changed. The machine doesn't relearn how coins
work; it just behaves according to whichever state it's currently in.

**Smell that suggests it:** a class with a `Status`/`State` enum field
and several methods that each start with a `switch (status)` covering
mostly the same cases, especially when adding a new status means
touching every one of those switches.
