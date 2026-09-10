# Composite

**Problem it solves:** you have individual objects and groups of those
same objects, and you want calling code to treat "one unit" and "a
group of units" through the exact same interface, including groups that
contain other groups.

**When to use it**
- The domain is naturally tree-shaped and operations (move, take
  damage, render) make sense both on a single leaf and on any subtree —
  a single game unit or an entire squad (which might itself contain
  sub-squads) should both respond to `TakeDamage(10)` the same way.
- You want to add new group-level behavior without callers needing to
  branch on "is this one thing or a collection of things?"

**When NOT to use it (anti-example)**
- If groups and individual items genuinely behave differently (a group
  operation isn't just "do X to every member"), forcing them through one
  interface hides that difference instead of modeling it — don't use
  Composite just because something happens to contain a list of
  something else.
- Skip it for a fixed, shallow, two-level structure with no real
  recursion (e.g. "a form has fields," full stop, never "a form has
  fields, some of which are forms") — a plain collection is simpler and
  clearer.

**Real-world analogy:** a file system. You can ask for the size of a
single file, or the size of a folder — and a folder's size is just the
sum of whatever's inside it, files and sub-folders alike, computed the
same way regardless of how deep the nesting goes.

**Smell that suggests it:** code that does `if (item is Collection) {
loop and recurse } else { handle single item }` at more than one call
site — that "is it one or many?" branch is exactly what Composite
collapses into a single interface.
