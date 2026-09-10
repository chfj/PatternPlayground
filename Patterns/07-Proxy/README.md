# Proxy

**Problem it solves:** you need to control access to an object — delay
creating it until it's actually needed, cache its results, or gate who
can call it — without changing the object's interface or the code that
calls it.

**When to use it**
- The real object is expensive to create or call (a network request to
  fetch video metadata) and you want to defer that cost until it's
  actually needed, and/or cache the result for repeat calls.
- You want to add access control (only allow calls once some condition
  is met) transparently to something that otherwise looks and behaves
  exactly like the real thing.

**When NOT to use it (anti-example)**
- If callers need to reason about caching/lazy-loading explicitly (e.g.
  "force a refresh," "bypass the cache") a proxy that hides those
  concerns behind the same interface can get in the way — surface a
  cache-aware API instead of pretending to be transparent.
- Don't add a proxy for a cheap, always-needed object "just in case" —
  if it's always going to be constructed and always going to be called,
  a proxy adds a layer of indirection with no actual benefit.

**Real-world analogy:** a credit card. You don't hand a merchant your
actual bank account — you hand them a card that *stands in* for it,
checks whether the charge is authorized, and only then reaches through
to the real funds. The merchant's interaction looks identical either
way.

**Smell that suggests it:** a class does an expensive operation (I/O,
network call) unconditionally in its constructor or on every method
call, when many callers never end up needing the result, or ask for the
same result repeatedly.

**Note:** this exercise implements a caching + lazy-loading proxy, one
of several Proxy variants (others include protection proxies for access
control and remote proxies for network calls) — the same
same-interface-stands-in-for-the-real-object idea applies to all of
them.
