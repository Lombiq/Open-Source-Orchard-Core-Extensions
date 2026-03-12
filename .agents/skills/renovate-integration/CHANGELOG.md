# Changelog — renovate-integration skill

All notable changes to this skill are documented here. This log is **append-only** — entries must not be retroactively edited or removed.

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
