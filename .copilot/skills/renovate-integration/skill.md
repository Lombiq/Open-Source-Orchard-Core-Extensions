# Renovate Integration Skill — Authoritative Instructions

> **This file is the single source of truth for the renovate-integration skill.**
> All behavior, rules, safety constraints, and phased execution logic are defined here.

---

## Overview

This skill automates safe, approval-gated integration of Renovate dependency updates across the OSOCE superproject and its Git submodules. It operates as a strict finite-state machine (FSM) with mandatory approval checkpoints between phases.

---

## Initialization

On first execution (or when explicitly invoked):

1. Ask the user:
   > "Please provide the Jira work item / issue key to use (e.g. OSOE-123)."
2. **Do not proceed** until a key is provided.
3. Store the value as `<WORK_ITEM_KEY>`.
4. Declare:

```
STATE: INITIALIZED
WORK ITEM KEY: <WORK_ITEM_KEY>
```

---

## Global Safety Rules (Non-Negotiable)

1. **NEVER** run `git push` unless explicitly instructed by the user.
2. **NEVER** commit to:
   - `renovate/*` branches
   - `dev`
3. **ALLOWED** commit branches **ONLY**: `issue/<WORK_ITEM_KEY>`
4. **NEVER** skip approval checkpoints.
5. **NEVER** perform later-phase actions early.
6. Use minimal verbosity unless a tool fails.
7. If a required tool is missing, **STOP** and report it.

---

## Execution States (Strict FSM)

At the **top of every response**, declare **exactly one** state:

| State                | Description                                |
|----------------------|--------------------------------------------|
| `INITIALIZED`        | Work item key captured, ready to begin     |
| `ANALYSIS`           | Phase 1 — reviewing renovate branches      |
| `AWAITING_APPROVAL`  | Waiting for user to approve a phase        |
| `IMPLEMENTATION`     | Phase 2 — applying changes                 |
| `PR_CREATION`        | Phase 3–4 — GitHub Actions & PRs           |
| `FINALIZATION`       | Phase 5 — final merge & cleanup            |

---

## Phased Execution

### Phase 1 — Analysis

**STATE: `ANALYSIS`**

Actions:
- Identify all `renovate/*` branches newer than `origin/dev` in the superproject and every submodule.
- Review release notes and diffs for each identified update.
- Classify changes into: **Breaking** / **Risky** / **Non-trivial** / **Feature**.
- Note if newer patch versions are available beyond what Renovate proposes.

Constraints:
- **Do NOT modify** any code or branches during this phase.

On completion, transition to:

```
STATE: AWAITING_APPROVAL
STATUS: Awaiting approval for Phase 1 (Analysis)
```

---

### Phase 2 — Implementation

**STATE: `IMPLEMENTATION`**

Proceed **only** after user confirms:
> `APPROVED: Phase 1`

Actions:
- Use the scripted helper `scripts/git/checkout-latest-renovate.sh` where applicable.
- Merge renovate changes into branch `issue/<WORK_ITEM_KEY>`.
- Handle analyzer warnings, build issues, test failures, and lockfile updates.
- Commit **only** to `issue/<WORK_ITEM_KEY>`.

On completion, transition to:

```
STATE: AWAITING_APPROVAL
STATUS: Awaiting approval for Phase 2 (Implementation)
```

---

### Phase 3 — GitHub Actions Updates

**STATE: `PR_CREATION`**

Proceed **only** after user confirms:
> `APPROVED: Phase 2`

Actions:
- Update GitHub Actions references in `.github/workflows/` as specified by the user.
- Validate workflow YAML syntax.

On completion, transition to:

```
STATE: AWAITING_APPROVAL
STATUS: Awaiting approval for Phase 3 (GitHub Actions)
```

---

### Phase 4 — PR Creation

**STATE: `PR_CREATION`**

Proceed **only** after user confirms:
> `APPROVED: Phase 3`

Actions:
- Open PRs in the correct dependency order (submodules first, superproject last).
- Ensure PR descriptions reference `<WORK_ITEM_KEY>`.

On completion, transition to:

```
STATE: AWAITING_APPROVAL
STATUS: Awaiting approval for Phase 4 (PR Creation)
```

---

### Phase 5 — Finalization

**STATE: `FINALIZATION`**

Proceed **only** after user confirms:
> `APPROVED: Phase 4`

Actions:
- Revert temporary references when instructed.
- Merge to `dev` **only** when explicitly approved by the user.
- Clean up local branches if instructed.

On completion:

```
STATE: FINALIZATION
STATUS: Complete
```

---

## Scripted Git Operations

### `scripts/git/checkout-latest-renovate.sh`

Purpose:
- Check out the latest applicable `renovate/*` branch per submodule.
- Ignore renovate branches older than `origin/dev`.
- Check out **only one** renovate branch per repository (the newest applicable one).

Important notes:
- Other renovate branches may exist in each submodule — this script **intentionally selects only the newest applicable one**.
- Always `git fetch` before evaluating branches.

---

## Self-Updating Rules

### When a Self-Update is Required

A self-update is **required** when the user:

1. Points out a mistake in the skill's logic, rules, or workflow.
2. Provides a correction that is **general**, **reusable**, and **project-wide**.
3. Gives new instructions that should apply to future executions.
4. Requests changes to existing helper scripts.
5. Adds new scripted steps that should persist.
6. Refines safety, branching, approval, or tooling rules.

### When a Self-Update is NOT Allowed

A self-update is **not allowed** when the feedback is:

- Situational or one-off.
- Specific to a single execution.
- Experimental or hypothetical.

### Self-Update Mechanism

When feedback qualifies as a self-update:

1. **STOP** normal execution.
2. Explain briefly:
   - What will be updated.
   - Which files will change.
3. Ask explicitly:
   > "Should I persist this change into the renovate-integration skill?"
4. **Do NOT proceed** without confirmation.
5. Proceed **only** if the user replies with:
   > `CONFIRM SKILL UPDATE`

### Self-Update Implementation

When performing a confirmed self-update:

1. Update **only** these files as appropriate:
   - `skill.md` (authoritative logic)
   - `README.md` (usage or explanation)
   - `scripts/*` (automation logic)
2. Append an entry to `CHANGELOG.md` including:
   - Date
   - Summary of change
   - Reason (user feedback / correction)
3. **Do NOT** retroactively rewrite history in `CHANGELOG.md`.
4. **Do NOT** remove safeguards unless explicitly instructed.
5. Preserve backward compatibility unless impossible.

After updating:
- Acknowledge the update.
- Continue execution **only** after user approval if applicable.

---

## Repository Context

This is the **Lombiq Open-Source Orchard Core Extensions (OSOCE)** superproject. It uses Git submodules extensively. Submodule tracking branches are defined in `.gitmodules` (all track `dev`). Renovate configuration files are `renovate.json5`, `renovate-osoce.json5`, `renovate-osoce-submodule.json5`, and `renovate-osoce-orchard-core-submodule.json5`.

Key submodule paths (non-exhaustive):
- `src/Modules/*` — Orchard Core modules
- `src/Libraries/*` — shared libraries
- `src/Themes/*` — themes
- `src/Utilities/*` — build/setup utilities
- `test/*` — testing toolboxes
- `tools/*` — analyzers, GitHub Actions, etc.
