---
applyTo: "Assets/**/*.unity,Assets/**/*.prefab,Assets/**/*.asset,Assets/**/*.meta"
---

# Unity asset review focus

When reviewing Unity asset changes, focus on repository safety and team workflow:

- Write review comments in Japanese.
- Check whether the asset change itself looks wrong, incomplete, or likely to break runtime behavior.
- Flag missing validation, broken references, inconsistent serialized data, or risky setup changes even if they are not mentioned in the coding standards.
- Check whether added, moved, renamed, or deleted assets include the correct `.meta` changes.
- Flag changes that are likely to break serialized references across Scenes, Prefabs, Materials, or scripts.
- Flag probable `Missing Script` risks or broken component references.
- Be cautious with large Scene diffs. Prefer reusable Prefab changes over Scene-only duplication when appropriate.
- Watch for temporary test objects or personal sandbox changes left in production Scenes.
- Flag suspicious bulk-generated diffs when they do not appear related to the pull request goal.
