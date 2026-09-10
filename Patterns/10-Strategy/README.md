# Strategy

**Problem it solves:** the same operation needs to run using one of
several interchangeable algorithms, chosen at runtime, without an
`if`/`switch` on "which algorithm" scattered through the calling code.

**When to use it**
- You have several ways to compute the same kind of result (shipping
  cost via Standard/Express/Overnight) and the choice depends on
  something decided at runtime (what the customer picked at checkout).
- You want to add a new algorithm variant without touching the code that
  uses it — just add a new implementation of the shared interface.

**When NOT to use it (anti-example)**
- If there's only ever going to be one algorithm, a strategy interface
  is unnecessary indirection — just write the function.
- Don't use it when the "variants" actually need to share most of their
  steps and differ in only one or two — that's closer to Template
  Method (share a skeleton, vary a step) than Strategy (swap the whole
  algorithm).

**Real-world analogy:** a GPS app's route options — "fastest," "shortest,"
"avoid tolls." You pick one, and the app plugs that specific routing
algorithm into the exact same "get me directions" flow. The app doesn't
have a giant if-chain hardcoded for every routing preference; it just
calls whichever routing strategy you selected.

**Smell that suggests it:** a method with a `switch` on an enum/string
that picks between several different *calculations* that all produce
the same kind of result (a price, a duration, a score) — especially if
that same switch shows up in more than one place.
