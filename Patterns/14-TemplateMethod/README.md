# Template Method

**Problem it solves:** several variants of a process share the same
overall sequence of steps (read data, validate it, transform it, save
it) but differ in the details of one or two steps — and you don't want
to copy-paste the whole sequence for each variant.

**When to use it**
- The *order* of steps is fixed and shared across variants (CSV import
  vs. JSON import both: read -> validate -> transform -> save), but
  individual steps differ in implementation.
- You want to guarantee every variant actually performs every step, in
  the right order, without relying on each subclass to remember to call
  them all correctly.

**When NOT to use it (anti-example)**
- If variants need to skip steps, reorder them, or run steps
  conditionally in ways that differ wildly between variants, forcing
  them through one fixed skeleton fights the actual problem — Strategy
  (swap the whole algorithm) fits better.
- Don't use inheritance-based Template Method just to share a couple of
  utility calls — a plain shared helper method/function does that
  without locking variants into a base-class hierarchy.

**Real-world analogy:** a standardized recipe card format — "1. Prep
ingredients. 2. Cook. 3. Plate. 4. Garnish." Every dish follows those
four steps in that order, but *what* prepping, cooking, and garnishing
actually involve is completely different for a stir-fry versus a
dessert. The card's structure doesn't change; the specifics per dish
do.

**Smell that suggests it:** two or more classes/functions with nearly
identical step-by-step structure (same steps, same order) where only a
couple of the steps' internals actually differ between them — a sign
the shared skeleton should be factored out once.
