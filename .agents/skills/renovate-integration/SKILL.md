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
- Never commit to `dev`. Never push directly to `dev` on any repo — all changes must go through PRs on `issue/<WORK_ITEM_KEY>` branches.
- Only commit to `issue/<WORK_ITEM_KEY>`.
- **Do not prefix commit messages with `<WORK_ITEM_KEY>:`** — the key is already encoded in the branch name.
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
- Run `scripts/git/analyze-renovate-branches.sh` to discover **all** eligible `renovate/*` branches across the superproject and every submodule. The script applies the same age/merge filters as `checkout-latest-renovate.sh` but lists **every** eligible branch (not just one per repo) with diffs and commit logs. **Do not write your own git commands to discover renovate branches — always use this script.**
- Also run `scripts/git/checkout-latest-renovate.sh` (dry-run or read its output) to confirm which single branch per submodule the checkout script would select.
- Review relevant release notes and diffs.
- Classify each change as Breaking, Risky, Non-trivial, or Feature. Highlight each version's release notes.

Constraints:
- Do not modify code or branches.
- Do not generate ad-hoc scripts or inline git commands that duplicate the logic in `scripts/git/checkout-latest-renovate.sh` or `scripts/git/analyze-renovate-branches.sh`.

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
- For submodules with only a **single** eligible renovate branch and no further changes needed, **do not create an `issue/<WORK_ITEM_KEY>` branch** — leave the renovate branch checked out. The existing Renovate PR will suffice.
- For submodules that need **multiple renovate branches merged** or **additional manual changes** (e.g. GHA ref updates, patch version bumps), create `issue/<WORK_ITEM_KEY>` from `origin/dev` and merge all applicable renovate branches into it.
- When merging renovate branches into `issue/<WORK_ITEM_KEY>`, always use **merge commits** (not fast-forward): pass `--no-ff` to `git merge` so a merge commit is always created.
- Since the script selects only one branch per submodule, **also merge any additional eligible `renovate/*` branches** identified during Phase 1 analysis into `issue/<WORK_ITEM_KEY>` in each affected submodule.
- Check for a `renovate/*` branch in the **superproject** itself (e.g. `origin/renovate/non-breaking-dependency-versions`). If one exists and is eligible, merge it into `issue/<WORK_ITEM_KEY>` in the superproject instead of manually editing the same files.
- Resolve analyzer warnings, build/test failures, and lockfile updates. Build with `/property:RunAnalyzersDuringBuild=true` to surface analyzer violations (especially important when analyzer packages like Meziantou.Analyzer are updated).
- When analyzer package updates (e.g. Meziantou.Analyzer, SonarAnalyzer.CSharp) introduce **new warnings in existing code**, fix the warnings in the affected submodules rather than downgrading the analyzer version. Create `issue/<WORK_ITEM_KEY>` branches in those submodules if they don't already have one, merge any applicable renovate branches into them, and commit the warning fixes.
- When fixing string equality warnings (e.g. MA0127), prefer the `EqualsOrdinalIgnoreCase` and similar extension methods from `Lombiq.HelpfulLibraries` (declared in `namespace System;` so no extra using is needed) over raw `string.Equals(…, StringComparison.…)` calls.
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
- When `tools/Lombiq.GitHub.Actions` has changes (e.g. lock file maintenance in asset-lint), its internal workflow/action `@dev` references must be temporarily updated to `@issue/<WORK_ITEM_KEY>` so CI can resolve them from the issue branch.
- Update **all** `Lombiq/GitHub-Actions/...@dev` references to `@issue/<WORK_ITEM_KEY>` in every `.yml` file under `tools/Lombiq.GitHub.Actions/.github/` (both `actions/` and `workflows/`).
- Then update the **superproject's** `.github/workflows/*.yml` files the same way — replace `Lombiq/GitHub-Actions/...@dev` with `@issue/<WORK_ITEM_KEY>`.
- Commit the submodule changes first, then stage the updated submodule pointer together with the superproject workflow changes and commit.
- These are **temporary** references; they will be reverted back to `@dev` in Phase 5 (Finalization).
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
- Push `issue/<WORK_ITEM_KEY>` branches to all repos that have one (submodules with multiple renovate branches or additional changes, and the superproject).
- **Do not push or create PRs** for submodules where only a single renovate branch was checked out without further changes — their existing Renovate PRs are sufficient.
- Open the **superproject PR first**, targeting `dev`, referencing `<WORK_ITEM_KEY>` in the PR title and description.
- **Wait 60 seconds** after the superproject PR is created.
- Then open PRs for the submodules that have `issue/<WORK_ITEM_KEY>` branches, targeting `dev`, referencing `<WORK_ITEM_KEY>`.
- Submodule PR titles should reference the specific updates included, e.g. `Update dependencies: Microsoft.NET.Test.Sdk 18.0.1 → 18.3.0, Swashbuckle.AspNetCore 10.1.4 → 10.1.5`.
- After PRs are created, **wait for all CI workflow runs to complete**. Poll the run status periodically (e.g. every 60 seconds) using `gh run list`.
- Once the Ubuntu build (Build and Test) succeeds on the superproject PR, add the `run-windows-build` label to the PR (using `gh pr edit --add-label run-windows-build`) to trigger the Windows build, then wait for it to succeed too.
- If all checks pass, proceed to ask for approval.
- If any checks fail, investigate the failures, fix the errors, push the fixes, and wait for the new runs to pass before asking for approval.

Completion output:

```text
STATE: AWAITING_APPROVAL
STATUS: Awaiting approval for Phase 4 (PR Creation) — all CI checks passed.
```

### Phase 5: Finalization
Required state: `FINALIZATION`
Gate: proceed only after `APPROVED: Phase 4`

Actions:
- **Before merging any PRs**, revert temporary GHA references that were changed in Phase 3:
  - In `tools/Lombiq.GitHub.Actions`: revert all `@issue/<WORK_ITEM_KEY>` references back to `@dev` in every `.yml` file under `.github/` (both `actions/` and `workflows/`). Commit on the `issue/<WORK_ITEM_KEY>` branch and push. This ensures `@dev` self-references land on `dev` when the PR is merged.
  - In the **superproject's** `.github/workflows/*.yml` files: revert the same `@issue/<WORK_ITEM_KEY>` → `@dev` replacements.
- Merge **all** submodule branches to `dev`:
  - Merge PRs for submodules that have `issue/<WORK_ITEM_KEY>` branches (these were created in Phase 4). Use `gh pr merge --merge` — **never** use `git push origin dev` or `git merge` directly onto `dev`, even if a repo has a merge queue or branch protection that blocks `gh pr merge`. If `gh pr merge` fails due to branch protection, report the failure and ask the user how to proceed rather than bypassing via direct push.
  - **Also merge the existing Renovate PRs** for submodules where only a single renovate branch was checked out directly (no issue branch was created). These PRs still exist on GitHub and must be merged too.
- After all submodule PRs are merged, update **every** submodule pointer in the superproject to the merged `dev` head (`git fetch origin dev && git checkout origin/dev` in each submodule).
- Commit the updated submodule pointers together with the superproject workflow ref reverts.
- Include `[skip ci]` in commit messages for submodule pointer updates and reference reverts. Do not wait for CI on these commits.
- Merge the superproject PR to `dev` only when explicitly approved.
- After the superproject PR is merged, check out the merged `dev` in the superproject itself (`git fetch origin dev && git checkout origin/dev`).
- Clean up local branches only when instructed.

Completion output:

```text
STATE: FINALIZATION
STATUS: Complete
```

## Scripts
**Always use the scripts below instead of generating equivalent inline commands or new scripts.** The scripts encode the canonical filtering logic (age cutoff, merge check) and must be the single source of truth.

### scripts/git/analyze-renovate-branches.sh
- **Read-only** analysis of all `renovate/*` branches in the superproject and every submodule.
- **Fetches all remotes** before evaluating branches (same as `checkout-latest-renovate.sh`) so newly-pushed Renovate branches are always visible.
- Reports each branch as ELIGIBLE or SKIP (too old / already merged) using the same age and merge-base logic as `checkout-latest-renovate.sh`.
- Shows `diff --stat` and commit log against `origin/dev` for every eligible branch.
- Covers the **superproject** as well (unlike `checkout-latest-renovate.sh` which only processes submodules).
- Configurable via `MAX_AGE_DAYS` (default: 5).
- Does **not** modify any branches, commit, or push.

### scripts/git/checkout-latest-renovate.sh
- Checks out the newest applicable `renovate/*` branch per repository.
- Skips branches older than 5 days (configurable via `MAX_AGE_DAYS`).
- Skips branches already merged into `origin/dev`.
- **Intentionally selects only one** applicable renovate branch per repository. Additional eligible branches must be discovered via `analyze-renovate-branches.sh` and merged during Phase 2.
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
