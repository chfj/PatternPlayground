# Decorator

**Problem it solves:** you need to add optional, stackable behavior
around an object (compression, encryption, logging) without baking every
combination into a subclass, and without changing the object's own
class.

**When to use it**
- Behavior needs to be layered and combined at runtime in different
  combinations for different callers — e.g. "export as CSV, compressed
  and encrypted" vs. "export as CSV, just logged" — using the exact same
  base exporter.
- Each layer should be independently understandable and testable, and
  new layers should be addable without touching existing ones.

**When NOT to use it (anti-example)**
- If there's exactly one fixed combination of behavior that will ever be
  needed, just write one class that does all of it — wrapping three
  single-purpose decorators around a base object is more indirection
  than a straightforward method with three sequential steps.
- Avoid decorators when the "added behavior" needs to fundamentally
  change what the object *is* (its type/identity), not just wrap
  extra behavior around the same operation — that's a different
  problem, not a Decorator one.

**Real-world analogy:** dressing for winter — a base layer, then a
sweater, then a coat, then a scarf, each layered on top of the last
without any one of them needing to know about the others. You choose
which layers to put on independently, and can add a new kind of layer
(like a rain shell) without redesigning the ones you already own.

**Smell that suggests it:** an explosion of subclasses like
`CsvExporter`, `CompressedCsvExporter`,
`EncryptedCompressedCsvExporter`, `LoggedEncryptedCompressedCsvExporter`
— one subclass per *combination* of optional behavior.
