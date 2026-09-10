# Chain of Responsibility

**Problem it solves:** a request needs to be handled by one of several
possible handlers, but the sender shouldn't have to know or decide
which one — and the set/order of handlers should be easy to change
without touching the sending code.

**When to use it**
- Requests should be offered to a sequence of handlers, each of which
  can either handle it or pass it along — support ticket escalation
  (L1 -> L2 -> L3) is a classic example: try the cheapest/fastest
  handler first, escalate only if it can't resolve things.
- You want to add, remove, or reorder handlers (e.g. insert a new
  triage step) without changing the code that submits requests.

**When NOT to use it (anti-example)**
- If exactly one handler should always process a given request and
  there's no real "try this, then that" escalation logic, a chain is
  unneeded indirection — just call the handler directly.
- Don't build a chain where every request must pass through every
  handler regardless of outcome (that's more of a pipeline/decorator
  situation) — Chain of Responsibility implies each handler can stop the
  chain by actually handling the request.

**Real-world analogy:** calling a company's support line. You don't get
routed straight to an engineer — a first-line rep tries to help, and
only if they can't do they escalate to the next tier, and so on. You
never decide who handles your call; each tier decides "can I handle
this, or does it go up?"

**Smell that suggests it:** a method with a long `if/else if` chain that
tries handler A, and if that "doesn't apply" falls through to try
handler B, then C — especially if that escalation order needs to change
or grow over time.
