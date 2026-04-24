---
applyTo: "Assets/**/*.cs"
---

# Unity C# review focus

When reviewing C# files under `Assets/`, check these points first:

- Write review comments in Japanese.
- Check whether the logic is correct before commenting on code style.
- Flag bugs, wrong conditions, missing branches, invalid assumptions, and regressions in behavior.
- Flag missing or weak error handling, null handling, validation, guard clauses, and cleanup.
- Flag code paths that can leave inconsistent state, subscribe twice, fail silently, or throw at runtime.
- Inspector-facing data should usually be `[SerializeField] private`, not mutable `public` fields.
- Public members should exist only when they are intentional API for other classes.
- `MonoBehaviour` classes should not combine unrelated concerns such as input, movement, UI, audio, and game-state orchestration.
- New private or protected methods should use standard `PascalCase` names instead of `_MethodName()`.
- `Awake` should handle self-initialization and component caching. `Start` should be used when other objects must already be initialized.
- `OnEnable` and `OnDisable` should be paired for event subscription lifecycles.
- `FixedUpdate` should be used for physics-related movement. `LateUpdate` should be used for follow-up behavior such as camera tracking.
- Flag repeated `GetComponent`, `FindObjectOfType`, `FindAnyObjectByType`, `GameObject.Find`, LINQ, or allocations inside per-frame code.
- Flag missing null checks or configuration validation for required serialized references.
- Prefer comments that explain why the code exists, not comments that restate the code.
- Flag unused `using` directives and unnecessary `using static` when they reduce readability.

Use the project coding standards as guidance, but do not restrict reviews to explicit rule violations in those documents.
Leave style-only comments only when they materially improve readability or consistency with these rules.
