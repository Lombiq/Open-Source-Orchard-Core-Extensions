# Renovate Integration Skill

A project-level Copilot agent skill that automates safe, approval-gated integration of Renovate dependency updates across the OSOCE superproject and its Git submodules.

## Purpose

Renovate creates `renovate/*` branches with dependency updates in this superproject and its submodules. Integrating these updates manually is tedious and error-prone. This skill provides a structured, phased workflow with safety gates at every step.

## Quick Start

Invoke the skill and it will prompt you for a Jira work item key (e.g. `OSOE-123`). All work is performed on an `issue/<WORK_ITEM_KEY>` branch — never directly on `dev` or `renovate/*`.

## Workflow Phases

| Phase | Name              | Description                                              |
|-------|-------------------|----------------------------------------------------------|
| 1     | Analysis          | Identify renovate branches, review diffs, classify risk  |
| 2     | Implementation    | Merge changes into `issue/` branch, fix build issues     |
| 3     | GitHub Actions    | Update workflow references as needed                     |
| 4     | PR Creation       | Open PRs in correct dependency order                     |
| 5     | Finalization      | Revert temp references, merge to `dev` when approved     |

Every phase transition requires **explicit user approval**. The skill will never proceed without it.

## Safety Guarantees

- **No push without permission** — `git push` is never executed unless explicitly instructed.
- **No commits to protected branches** — `renovate/*` and `dev` are never committed to directly.
- **Approval-gated execution** — each phase waits for explicit user approval before proceeding.
- **Minimal blast radius** — all work happens on `issue/<WORK_ITEM_KEY>` branches.

## File Structure

```
.agents/skills/renovate-integration/
├── README.md           ← This file (human-facing documentation)
├── SKILL.md            ← Canonical Copilot instructions (authoritative)
├── CHANGELOG.md        ← Skill evolution log (append-only)
└── scripts/
    ├── git/
    │   └── checkout-latest-renovate.sh
    ├── dotnet/
    │   └── .gitkeep
    └── tests/
        └── .gitkeep
```

## Scripts

### `scripts/git/checkout-latest-renovate.sh`

Checks out the latest applicable `renovate/*` branch in each submodule. Only selects branches newer than `origin/dev`. Intentionally picks only the newest applicable branch per repo — other renovate branches may exist but are ignored.

**Usage:**
```bash
bash .agents/skills/renovate-integration/scripts/git/checkout-latest-renovate.sh
```

## Self-Updating

This skill is self-maintaining. When you provide feedback that is general, reusable, and project-wide, the skill will offer to update itself. It will:

1. Explain what will change and which files are affected.
2. Ask for explicit confirmation (`CONFIRM SKILL UPDATE`).
3. Update the relevant files and append an entry to `CHANGELOG.md`.

One-off or situational feedback is applied in-session only, without modifying skill files.

## Authoritative Reference

All behavioral rules, safety constraints, FSM states, and phased execution logic are defined in [SKILL.md](SKILL.md). That file is the single source of truth.
