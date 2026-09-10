# Builder

**Problem it solves:** an object has many optional parts and needs to be
assembled step by step, but a single constructor with a dozen optional
parameters (half of them `null` most of the time) is unreadable and
error-prone.

**When to use it**
- Constructing the object involves several optional or ordered steps
  (add line items, maybe apply a discount, maybe add a footer note) and
  you want each step to read like what it does, not like positional
  constructor arguments.
- You want the same step-by-step process to be reusable for genuinely
  different "shapes" of the final object.

**When NOT to use it (anti-example)**
- A type with two or three required fields and no optional variation
  doesn't need a builder — a constructor or an object initializer
  (`new Invoice { CustomerName = ..., Total = ... }`) is simpler and just
  as readable.
- Don't build a builder whose steps must always be called in one fixed
  order and never skipped — at that point it's just a constructor with
  extra ceremony.

**Real-world analogy:** ordering a custom sandwich at a deli counter —
bread, then protein, then you *optionally* add cheese, toppings, and a
sauce, and finally it gets wrapped. You don't hand the cashier one giant
form with 15 blank fields; you build it step by step, skipping what you
don't want.

**Smell that suggests it:** a constructor (or method) with 6+
parameters, several of them optional/nullable, especially if callers
routinely pass `null`/`default` for most of them and only a couple
matter for a given case.
