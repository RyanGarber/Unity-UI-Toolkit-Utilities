# Unity UI Toolkit Utility
Utilities for UI Toolkit.

---

### Find()
Instead of using Unity's convoluted and inconsistent UQuery, use selectors you're familiar with!

#### Selectors
- Type: `Find("Button")`
- Class: `Find(".class")`
- Name: `Find("#name")`
- Wildcard: `Find("*")`

#### Inheritance
- Descendents: `Find("#header .title")`
- Children: `Find("#list > .list-entry")`

#### Combinations
- Selectors: `Find("Button.selected")`
- Queries: `Find("#play, #pause")`

---

###### Note: These features are not designed for performance and will create garbage. Cache their results inside OnEnable() to prevent a performance impact.
