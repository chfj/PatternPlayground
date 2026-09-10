# Adapter

**Problem it solves:** you have working code that expects one interface,
and an existing class (often third-party or legacy) that does the same
job but exposes a different, incompatible interface — and you can't or
shouldn't rewrite either side.

**When to use it**
- Integrating a legacy or third-party component (an old XML-based
  payment gateway SDK) into a codebase that's standardized on a newer
  interface (a JSON-based `IPaymentProcessor`), without touching the
  legacy code or leaking its shape into the rest of the app.
- You want to swap vendors/implementations later — code depends on your
  interface, and only the adapter needs to change per vendor.

**When NOT to use it (anti-example)**
- If you control both sides and they're incompatible only by accident
  (e.g. one method is named `Send` and the other `Submit` but they do
  identical things with identical parameters), just rename one — an
  adapter class adds a layer of indirection for a problem a rename would
  fix outright.
- Don't use it to paper over a fundamentally wrong abstraction — if the
  legacy component can't actually satisfy the new interface's contract
  (e.g. it's synchronous-only but the new interface promises async
  cancellation), an adapter will lie about that, not fix it.

**Real-world analogy:** a travel plug adapter. Your laptop charger has a
US plug; the wall socket in another country is a different shape. The
adapter doesn't rewire your charger or the building — it just sits
between the two, translating one plug shape into the other.

**Smell that suggests it:** you're tempted to modify a third-party
class's source, or you've got a wrapper class whose methods just
forward to a differently-shaped API, translating parameters/return
types on the way through.
