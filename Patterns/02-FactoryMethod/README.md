# Factory Method

**Problem it solves:** calling code needs an object that implements a
common interface, but which *concrete* type to create depends on a
runtime condition — and you don't want that `if`/`switch` on type
duplicated everywhere the object gets created.

**When to use it**
- You have one interface (`INotification`) and a growing family of
  implementations (`EmailNotification`, `SmsNotification`,
  `PushNotification`), and the choice of which one to build is driven by
  data (a user's preference, a config value, a file extension).
- You want adding a new variant to touch one place (a new `case` in the
  factory) instead of every call site that currently does
  `if (type == "email") new EmailNotification() else ...`.

**When NOT to use it (anti-example)**
- If there's only one concrete implementation and no realistic prospect
  of a second, a factory is pure ceremony — just `new` the thing.
- Don't reach for it when the "variation" is really just constructor
  *parameters* (e.g. different message text) rather than different
  *behavior* — that's not a family of types, it's one type with options.

**Real-world analogy:** a pizzeria's order ticket says "large pepperoni"
or "small veggie" — the customer doesn't personally shape the dough and
pick toppings; they hand a spec to the kitchen, and the kitchen (the
factory) decides how to actually build that specific pizza.

**Smell that suggests it:** a chain of `if (kind == "X") return new
X(...); else if (kind == "Y") return new Y(...);` — especially if that
same chain is copy-pasted at more than one call site.
