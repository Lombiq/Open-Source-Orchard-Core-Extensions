# Renovate Integration Skill

A project-level Copilot agent skill that automates safe, approval-gated integration of Renovate dependency updates across the OSOCE superproject and its Git submodules.

## Purpose

Renovate opens pull requests with dependency updates in this superproject and its submodules. Integrating these updates manually is tedious and error-prone. This skill provides a structured, phased workflow with safety gates at every step. It discovers work by listing **open Renovate PRs** in every repository and then checking out their head branches — stale `renovate/*` branches without an open PR are ignored.

## Requirements

- [GitHub CLI](https://cli.github.com/) (`gh`), authenticated — used to enumerate and merge Renovate PRs.

## Quick Start

Invoke the skill and it will prompt you for a Jira work item key (e.g. `OSOE-123`). All work is performed on an `issue/<WORK_ITEM_KEY>` branch — never directly on `dev` or `renovate/*`.

## Workflow Phases

| Phase | Name              | Description                                              |
|-------|-------------------|----------------------------------------------------------|
| 1     | Analysis          | List open Renovate PRs, review diffs, classify risk      |
| 2     | Implementation    | Check out/merge PR branches into `issue/` branch, fix build issues |
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
    │   ├── analyze-renovate-prs.sh
    │   └── checkout-latest-renovate-pr.sh
    ├── dotnet/
    │   └── .gitkeep
    └── tests/
        ├── .gitkeep
        └── parse-check.sh
```

## Scripts

### `scripts/git/analyze-renovate-prs.sh`

Read-only. Lists every open PR with a `renovate/*` head branch in the superproject and each submodule, marking each as ELIGIBLE or SKIP (draft / too old / already merged), with a diff stat and commit log against `origin/dev`.

**Usage:**
```bash
bash .agents/skills/renovate-integration/scripts/git/analyze-renovate-prs.sh
```

### `scripts/git/checkout-latest-renovate-pr.sh`

Checks out the head branch of the newest applicable open Renovate PR in each submodule. Intentionally picks only one PR per repo, and prints the remaining eligible PR numbers so they can be merged manually.

**Usage:**
```bash
bash .agents/skills/renovate-integration/scripts/git/checkout-latest-renovate-pr.sh
```

## Self-Updating

This skill is self-maintaining. When you provide feedback that is general, reusable, and project-wide, the skill will offer to update itself. It will:

1. Explain what will change and which files are affected.
2. Ask for explicit confirmation (`CONFIRM SKILL UPDATE`).
3. Update the relevant files and append an entry to `CHANGELOG.md`.

One-off or situational feedback is applied in-session only, without modifying skill files.

## Authoritative Reference

All behavioral rules, safety constraints, FSM states, and phased execution logic are defined in [SKILL.md](SKILL.md). That file is the single source of truth.
