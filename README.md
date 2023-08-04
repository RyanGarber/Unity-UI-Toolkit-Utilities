# Unity UI Toolkit Utility
Utilities for UI Toolkit.

### Find()
Instead of using Unity's convoluted, inconsistent, obtuse UQuery, use selectors you're familiar with.
Note: As this is much slower than UQuery, it should only be used inside OnEnable().

#### Selectors
- Types: `Find("Button")`
- Descendents: `Find("#header .title")`
- Children: `Find("#list > .list-entry")`
- Wildcards: `Find("*")`

#### Combinations
- Selectors: `Find("Button#toggle-dark-mode.selected")`
- Queries: `Find("#settings.tab, #settings.tab-content")`
