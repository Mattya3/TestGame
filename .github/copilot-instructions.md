# Copilot review instructions for TestGame

Review this repository as a Unity project built with C#.
Write review comments in Japanese.

Prioritize findings that affect correctness, runtime safety, regression risk, error handling, maintainability, and Unity-specific safety over style-only comments.

General review rules:

- First check whether the code is simply wrong, unsafe, likely to break at runtime, or missing necessary error handling.
- Treat `Docs/CodingStandards/CSharp.md` and `Docs/CodingStandards/UnityRules.md` as project standards, but do not limit reviews to those files alone.
- Treat `Docs/PR/ReviewProcess.md` as the preferred review style and severity guideline.
- Use the standards as supporting context, not as the only source of review criteria.
- Prefer comments only when there is a concrete bug, regression risk, rule violation, or meaningful maintainability issue.
- Do not ask for changes that are only personal preference if the current code is clear and consistent.
- Prefer standard C# and Unity conventions in new code.
- When possible, prefix review comments with `[MUST]`, `[SHOULD]`, `[COULD]`, or `[WANT]` according to severity.
- Prefer `[MUST]` for correctness, runtime safety, regression risk, broken references, and insufficient error handling.
- Prefer `[SHOULD]` for maintainability or clarity issues that have meaningful impact.
- Avoid overusing `[WANT]` for personal taste.

Repository-wide expectations:

- Flag logic that appears incorrect, incomplete, or inconsistent with the stated behavior of the code.
- Flag places where null handling, failure handling, guard clauses, or recovery behavior are missing or insufficient.
- Flag changes that may throw exceptions, leave invalid state behind, or fail silently without enough diagnostics.
- Flag risky changes to control flow, initialization order, state transitions, and async or coroutine behavior.
- Flag edge cases that are not handled when the code clearly depends on them.
- `public` fields should be avoided unless external write access is intentionally part of the API.
- Values exposed only for Inspector editing should usually be `[SerializeField] private`.
- `MonoBehaviour` classes should keep a narrow responsibility. Flag classes that mix unrelated concerns.
- Method and type names should make intent clear. Flag vague names and newly added private methods named like `_MethodName()`.
- `Awake`, `Start`, `OnEnable`, `OnDisable`, `FixedUpdate`, and `LateUpdate` should be used for the right purpose.
- Event subscriptions should normally be paired with unsubscription.
- Flag null-reference risks when required serialized references or required components are not validated.
- Flag heavy per-frame work in `Update` or `FixedUpdate`, especially repeated component lookup, scene-wide lookup, LINQ, or avoidable allocations.
- Prefer `CompareTag` over direct tag string comparison.
- Temporary `Debug.Log` statements should not remain in merged code.

Unity asset review rules:

- Watch for missing or inconsistent `.meta` files when assets are added, renamed, moved, or deleted.
- Watch for risky Scene or Prefab changes that could break references or introduce `Missing Script` issues.
- Prefer reusable Prefab-based changes over one-off Scene-only setup when the object is intended for reuse.
