# Iterator

**Problem it solves:** code that wants to walk through a collection's
elements shouldn't need to know how that collection is stored
internally, and a collection should be able to support more than one way
of walking through it (in order, shuffled) without exposing its guts.

**When to use it**
- You want callers to use a plain `foreach` over your custom collection
  without exposing the backing array/list/tree directly.
- The same collection needs multiple distinct traversal orders (e.g. a
  playlist played sequentially vs. shuffled) and you don't want callers
  choosing "traversal logic" by copying and reordering the underlying
  list themselves.

**When NOT to use it (anti-example)**
- If a plain `List<T>`/array (or C#'s built-in `IEnumerable<T>` via
  `yield return`) already gives callers everything they need, don't
  hand-roll a custom `IEnumerator` — that's exactly what C#'s iterator
  support and `yield` exist to avoid.
- Don't build multiple iterator types "just in case" if only one
  traversal order will ever be needed — that's speculative complexity.

**Real-world analogy:** a TV remote's "channel up" button. You don't
need to know how channels are stored or numbered internally — you just
press "next" repeatedly and get channels one at a time, and a different
remote mode ("favorites only") can walk through the exact same channel
lineup in a different order.

**Smell that suggests it:** calling code reaches into a class's private
list/array field (or a public property that leaks it) just to loop over
it manually, especially if there's already more than one place in the
codebase doing that same manual loop differently.
