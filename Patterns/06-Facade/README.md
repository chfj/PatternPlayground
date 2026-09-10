# Facade

**Problem it solves:** performing one conceptual operation (checking
out an order) requires coordinating several subsystems in a specific
order (check inventory, charge payment, book shipping, send a
confirmation) — and every caller shouldn't have to know that whole
dance.

**When to use it**
- A workflow spans multiple subsystems with real dependencies between
  the calls (you can't charge payment before you've confirmed stock),
  and most callers just want "do the workflow," not control over each
  step.
- You want to simplify the *common* path through a complex set of APIs
  while still leaving the underlying subsystems directly reachable for
  the rare caller that genuinely needs fine-grained control.

**When NOT to use it (anti-example)**
- If callers routinely need to skip steps, reorder them, or inject
  custom logic between them, a facade that hides the sequence works
  against them — expose the subsystems directly (or use something like
  Strategy/Template Method for the variable parts) instead.
- Don't make it a dumping ground — a facade that grows to expose every
  parameter of every subsystem has just become a second, worse copy of
  those subsystems' APIs.

**Real-world analogy:** a restaurant's front counter. Ordering "a
burger meal" doesn't require you to talk to the grill station, the fry
station, the drink dispenser, and the register separately — the counter
person coordinates all of that behind one interaction. The kitchen
stations still exist and could be reached directly by staff, but you as
a customer only need the one interface.

**Smell that suggests it:** the same 4-5 line sequence of calls into
different subsystems, in the same order, copy-pasted at every call site
that needs to "place an order" (or whatever the workflow is).
