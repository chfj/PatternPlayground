# Command

**Problem it solves:** you need to treat "a request to do something" as
an object in its own right — so it can be queued, logged, handed to a
generic "invoker" that doesn't know what it does, or undone later —
instead of just calling a method directly.

**When to use it**
- You need undo/redo: each action needs to remember not just what to do,
  but how to reverse itself, and a history of those objects is what
  makes undo possible.
- Something needs to trigger an action without knowing what that action
  actually does — a remote control's button doesn't know if it's wired
  to a light or a thermostat, it just knows "press -> execute this
  command."

**When NOT to use it (anti-example)**
- For a simple, direct call with no need for queuing, logging, or undo,
  just call the method — wrapping every action in a command object when
  none of that is needed is ceremony without payoff.
- Don't use it when actions can't meaningfully be undone or replayed
  (e.g. "send this email") unless you actually need to track that
  they *happened*, not reverse them — Command's undo/redo machinery adds
  no value there.

**Real-world analogy:** a restaurant order ticket. The waiter doesn't
cook — they write "Table 5: no onions, extra cheese" on a ticket and
hand it to the kitchen. The ticket is a self-contained request that can
be queued behind other tickets, reordered, or voided (undone) before
it's cooked, all without the waiter needing to know how to cook
anything.

**Smell that suggests it:** you're implementing undo/redo, an action
queue, or a macro/replay feature, and keep reaching for a giant
`switch` on "what kind of action was this" to figure out how to reverse
or re-run it.
