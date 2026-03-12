---
name: renovate-integration
description: Approval-gated workflow for integrating Renovate updates across the OSOCE superproject and submodules with strict branch and safety controls.
license: MIT
metadata:
  author: Lombiq Technologies
  version: "1.1"
---

# Renovate Integration

Use this skill to safely integrate Renovate dependency updates in OSOCE and its submodules.

## How to use
- Start by collecting a Jira work item key and store it as `<WORK_ITEM_KEY>`.
- Operate as a strict FSM with approval checkpoints between every phase.
- Keep actions minimal and deterministic; stop when required tools are unavailable.

Initialization prompt:

```text
Please provide the Jira work item / issue key to use (e.g. OSOE-123).
```

After key capture, declare:

```text
STATE: INITIALIZED
WORK ITEM KEY: <WORK_ITEM_KEY>
```

## Global safety rules
- Never run `git push` unless explicitly instructed by the user.
- Never commit to `renovate/*` branches.
- Never commit to `dev`.
- Only commit to `issue/<WORK_ITEM_KEY>`.
- Never skip approval checkpoints.
- Never perform later phase actions early.
- If a required tool is missing, stop and report it.

## Execution states
At the top of every response, declare exactly one state:

| State | Description |
| --- | --- |
| `INITIALIZED` | Work item key captured, ready to begin |
| `ANALYSIS` | Phase 1: reviewing Renovate branches |
| `AWAITING_APPROVAL` | Waiting for user approval |
| `IMPLEMENTATION` | Phase 2: applying changes |
| `PR_CREATION` | Phase 3-4: GitHub Actions and PR work |
| `FINALIZATION` | Phase 5: merge and cleanup |

## Workflow phases

### Phase 1: Analysis
Required state: `ANALYSIS`

Actions:
- Run `scripts/git/checkout-latest-renovate.sh` (dry-run or read its output) to identify eligible `renovate/*` branches. **Do not write your own git commands to discover renovate branches — always use the script.**
- Review relevant release notes and diffs.
- Classify each change as Breaking, Risky, Non-trivial, or Feature.
- Note newer patch versions beyond Renovate proposals.

Constraints:
- Do not modify code or branches.
- Do not generate ad-hoc scripts or inline git commands that duplicate the logic in `scripts/git/checkout-latest-renovate.sh`.

Completion output:

```text
STATE: AWAITING_APPROVAL
STATUS: Awaiting approval for Phase 1 (Analysis)
```

### Phase 2: Implementation
Required state: `IMPLEMENTATION`
Gate: proceed only after `APPROVED: Phase 1`

Actions:
- Run `scripts/git/checkout-latest-renovate.sh` to check out the selected renovate branches. **Always use this script — never generate replacement commands.**
- Merge Renovate changes into `issue/<WORK_ITEM_KEY>`.
- Resolve analyzer warnings, build/test failures, and lockfile updates.
- Commit only on `issue/<WORK_ITEM_KEY>`.

Completion output:

```text
STATE: AWAITING_APPROVAL
STATUS: Awaiting approval for Phase 2 (Implementation)
```

### Phase 3: GitHub Actions updates
Required state: `PR_CREATION`
Gate: proceed only after `APPROVED: Phase 2`

Actions:
- Update references in `.github/workflows/` as requested.
- Validate workflow YAML syntax.

Completion output:

```text
STATE: AWAITING_APPROVAL
STATUS: Awaiting approval for Phase 3 (GitHub Actions)
```

### Phase 4: PR creation
Required state: `PR_CREATION`
Gate: proceed only after `APPROVED: Phase 3`

Actions:
- Open PRs in dependency order (submodules first, superproject last).
- Reference `<WORK_ITEM_KEY>` in PR descriptions.

Completion output:

```text
STATE: AWAITING_APPROVAL
STATUS: Awaiting approval for Phase 4 (PR Creation)
```

### Phase 5: Finalization
Required state: `FINALIZATION`
Gate: proceed only after `APPROVED: Phase 4`

Actions:
- Revert temporary references when instructed.
- Merge to `dev` only when explicitly approved.
- Clean up local branches only when instructed.

Completion output:

```text
STATE: FINALIZATION
STATUS: Complete
```

## Scripts
**Always use the scripts below instead of generating equivalent inline commands or new scripts.** The scripts encode the canonical filtering logic (age cutoff, merge check) and must be the single source of truth.

### scripts/git/checkout-latest-renovate.sh
- Checks out the newest applicable `renovate/*` branch per repository.
- Skips branches older than 5 days (configurable via `MAX_AGE_DAYS`).
- Skips branches already merged into `origin/dev`.
- Intentionally selects only one applicable renovate branch per repository.
- Always fetch before evaluating branch freshness.

## Self-update policy

### Requires self-update
Trigger self-update when user feedback is general, reusable, and project-wide, including:
- Logic or workflow corrections.
- New persistent instructions.
- Helper script changes.
- Safety, branching, approval, or tooling rule refinements.

### Must not self-update
Do not self-update for one-off, situational, or hypothetical feedback.

### Confirmation protocol
When self-update is required:
1. Stop normal execution.
2. Briefly explain planned updates and affected files.
3. Ask: `Should I persist this change into the renovate-integration skill?`
4. Continue only after: `CONFIRM SKILL UPDATE`

### Allowed files and changelog
- Update only `SKILL.md`, `README.md`, and `scripts/*` as needed.
- Append to `CHANGELOG.md` with date, summary, and reason.
- Never rewrite changelog history.
- Never remove safeguards unless explicitly instructed.
- Preserve backward compatibility unless impossible.

## Repository context
OSOCE is a superproject with extensive Git submodule usage. Submodules track `dev` via `.gitmodules`.

Relevant Renovate config files:
- `renovate.json5`
- `renovate-osoce.json5`
- `renovate-osoce-submodule.json5`
- `renovate-osoce-orchard-core-submodule.json5`

Key repository areas (non-exhaustive):
- `src/Modules/*`
- `src/Libraries/*`
- `src/Themes/*`
- `src/Utilities/*`
- `test/*`
- `tools/*`
