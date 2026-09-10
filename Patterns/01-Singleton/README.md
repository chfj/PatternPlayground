# Singleton

**Problem it solves:** guarantee that a piece of shared, stateful
machinery — one that would corrupt itself or waste resources if two
copies existed — is only ever instantiated once, with one well-known
access point to reach it.

**When to use it**
- Something genuinely must be unique process-wide: a hardware handle, a
  single write-lock over one file, a connection pool.
- You need lazy, on-demand creation of that one instance instead of
  paying its setup cost at startup.

**When NOT to use it (anti-example)**
- As a substitute for dependency injection just to avoid passing an
  object through a few constructors — that turns a normal service (say,
  an `IOrderRepository`) into hidden global state, making unit tests
  hard (you can't swap in a fake instance) and hiding a class's real
  dependencies from its constructor signature.
- For anything you might legitimately want more than one of later (e.g.
  "for now there's only one game save slot") — that assumption tends to
  age badly.

**Real-world analogy:** a household only has one main circuit breaker
panel. Every room's wiring runs back to that same panel — you don't
wire a second one "just in case," and every electrician who needs to
flip a breaker goes to that one panel, not a copy of it.

**Smell that suggests it:** you catch yourself passing the same object
through five constructors just so a deeply nested class can reach it,
or you're tempted to make a field `static` purely so "everyone can get
to it," without actually needing more than one.
