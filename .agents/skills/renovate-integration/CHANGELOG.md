# Changelog — renovate-integration skill

All notable changes to this skill are documented here. This log is **append-only** — entries must not be retroactively edited or removed.

---

## 2026-03-12

**Add age and merge filters to renovate branch selection**

- Updated `scripts/git/checkout-latest-renovate.sh` to skip renovate branches older than 5 days (`MAX_AGE_DAYS`) and branches already merged into `origin/dev`.
- Replaced the old "newer than origin/dev timestamp" heuristic with explicit `git merge-base --is-ancestor` check and date-based cutoff.
- Updated `SKILL.md` Phase 1 analysis and script documentation to reflect the new filters.

Reason: Prevent the skill from picking up stale renovate branches from weeks or months ago.

---

## 2026-03-12

**Format standardization**

- Updated `SKILL.md` to standard AI skill format with YAML frontmatter (`name`, `description`, `license`, `metadata`).
- Reorganized instructions into consistent sections (`How to use`, `Global safety rules`, `Execution states`, `Workflow phases`, `Scripts`, `Self-update policy`, `Repository context`).
- Preserved the existing FSM logic and approval-gated workflow behavior.
- Updated README references from `skill.md` to `SKILL.md`.

Reason: Align skill format with common AI tooling expectations.

---

## 2026-03-12

**Initial creation**

- Created skill structure: `skill.md`, `README.md`, `CHANGELOG.md`, `scripts/`.
- Defined 5-phase approval-gated workflow (Analysis → Implementation → GitHub Actions → PR Creation → Finalization).
- Implemented strict FSM state declarations.
- Established global safety rules (no push without permission, no commits to `dev`/`renovate/*`).
- Created `scripts/git/checkout-latest-renovate.sh` for automated submodule renovate branch checkout.
- Defined self-update mechanism with `CONFIRM SKILL UPDATE` gate.

Reason: Initial skill creation per user specification.
