# Observer

**Problem it solves:** several independent parties need to react
whenever an object's state changes, but that object shouldn't have to
know who they are or hard-code calls to each of them.

**When to use it**
- The set of interested parties is dynamic or app-specific — a customer
  wants an SMS on order status changes, an analytics service wants to
  log every change, and neither should be wired into `OrderProcessor`
  itself.
- You want to add a new kind of reaction to a state change (e.g. a new
  analytics sink) without modifying the class whose state is changing.

**When NOT to use it (anti-example)**
- If there's exactly one fixed listener that will ever exist and it's
  tightly coupled to the subject anyway, just call it directly — a
  subscribe/notify mechanism for a permanent 1:1 relationship is
  needless indirection.
- Be careful with observers that must run in a guaranteed order or that
  can fail and need to roll back the whole operation — plain Observer
  doesn't give you ordering or transactional guarantees, so don't reach
  for it when you need those.

**Real-world analogy:** a YouTube channel's subscribers. The channel
doesn't maintain a personal relationship with each subscriber or know
what they'll do with a new video — it just uploads, and everyone who
subscribed gets notified. New subscribers can join, existing ones can
unsubscribe, all without the channel's upload process changing at all.

**Smell that suggests it:** a method that, after doing its main job,
also directly calls into two or three unrelated other services ("...and
also email the customer, and also update analytics, and also ping
Slack") — especially if that side-effect list keeps growing.
