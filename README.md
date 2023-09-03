# Unity UI Toolkit Utilities
Utilities for UI Toolkit.

---

## Query()
Instead of using Unity's convoluted and inconsistent UQuery, use selectors you're familiar with!

#### Selectors
- Type: `Query("Button")`
- Class: `Query(".class")`
- Name: `Query("#name")`
- Wildcard: `Query("*")`

#### Inheritance
- Immediate Descendents: `Query("#list > .list-entry")`
- All Descendents: `Query("#header .title")`

#### Combinations
- Selectors: `Query(".button#pause")`
- Queries `Query(".button#pause, .button#play")`
- Chains `Query("#controls").Query(".button")`

### Using The Results

A few ways of using the results:
- First Result: `Query(".label").First()`
- All Results: `Query(".label").All()`
- Iterate Results: `Query(".label").ForEach(element => {})`

All of those can be used or without a type. Results will be filtered by that type; no exception is thrown for an invalid cast.
- Example: `All<Button>()`, `First<Button>()` `ForEach<Button>(button => {})`

---

###### Note: These features are not designed for performance and will create garbage. Cache their results inside OnEnable() to prevent a performance impact.
