---
name: renovate-integration
description: Approval-gated workflow for integrating Renovate updates across the OSOCE superproject and submodules with strict branch and safety controls.
license: MIT
metadata:
  author: Lombiq Technologies
  version: "1.3"
---

# Renovate Integration

Use this skill to safely integrate Renovate dependency updates in OSOCE and its submodules.

## How to use
- Collect a Jira work item key, store as `<WORK_ITEM_KEY>`.
- Operate as a strict FSM with an approval checkpoint between every phase.
- Keep actions minimal and deterministic; stop when a required tool is unavailable.

Initialization prompt: `Please provide the Jira work item / issue key to use (e.g. OSOE-123).`

After key capture, declare:

```text
STATE: INITIALIZED
WORK ITEM KEY: <WORK_ITEM_KEY>
```

## Global safety rules
- Never `git push` unless explicitly instructed. Never target `dev` directly — no `git push origin dev`/`HEAD:dev`/`issue/<WORK_ITEM_KEY>:dev`, no direct `git merge` into `dev`. All changes go through PRs.
- Commit only on `issue/<WORK_ITEM_KEY>`; never on `dev` or `renovate/*`. Don't prefix commit messages with `<WORK_ITEM_KEY>:` (already in the branch name).
- Before pushing, verify with `git status`/`git log --oneline -3` that HEAD is on `issue/<WORK_ITEM_KEY>`.
- Before merging PRs (Phase 5), run `git branch -v` in the superproject and every affected submodule to confirm local `dev` matches `origin/dev`; stop and investigate if it has drifted.
- Never skip approval checkpoints or perform later-phase actions early.
- If a required tool is missing, stop and report it.

## Token efficiency
- Never dump full raw tool output (build logs, `git diff`, `gh ... --json`) into the conversation — summarize findings in a few bullets instead.
- Filter command output at the source rather than after the fact: pipe builds/tests through a filter for errors/warnings/failures only (e.g. `dotnet build ... | Select-String -Pattern 'error|warning'`); request only the `gh`/`jq` fields you need (e.g. `--json name,state,conclusion`) instead of full JSON.
- Reuse Phase 1's `analyze-renovate-branches.sh` output for later phases instead of re-running it; don't re-run scripts to re-derive information already gathered.
- When polling CI (Phase 4), start at 60s but back off to 3-5 minutes for longer-running workflows instead of polling at a fixed short interval — fewer polls means fewer tool round-trips.
- Read only the files/sections needed to make a decision (e.g. a package's changelog entry for the updated version range) rather than whole changelogs or unrelated diff hunks.

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
- Run `scripts/git/analyze-renovate-branches.sh` to discover **all** eligible `renovate/*` branches across the superproject and every submodule (same age/merge filters as `checkout-latest-renovate.sh`, but lists every eligible branch, not just one per repo, with diffs and commit logs). **Never write ad-hoc git commands for this — always use the script.**
- Also run (or read the output of) `scripts/git/checkout-latest-renovate.sh` to confirm which single branch per submodule it would select.
- Review release notes and diffs; classify each change as Breaking, Risky, Non-trivial, or Feature.

Constraints:
- Do not modify code or branches.
- Do not generate scripts or inline commands duplicating `checkout-latest-renovate.sh` / `analyze-renovate-branches.sh`.

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
- Single eligible branch, no further changes needed → leave the renovate branch checked out; do **not** create an `issue/<WORK_ITEM_KEY>` branch (the existing Renovate PR suffices).
- Multiple renovate branches to merge, or additional manual changes needed (e.g. GHA ref updates, patch bumps) → create `issue/<WORK_ITEM_KEY>` from `origin/dev` and merge all applicable renovate branches into it with `git merge --no-ff` (always a merge commit, never fast-forward). Also merge any additional eligible branches found in Phase 1 that the script didn't select.
- If the **superproject** itself has an eligible `renovate/*` branch (e.g. `origin/renovate/non-breaking-dependency-versions`), merge it into `issue/<WORK_ITEM_KEY>` instead of manually editing the same files.
- Resolve analyzer warnings, build/test failures, and lockfile updates. Build with `/property:RunAnalyzersDuringBuild=true` to surface analyzer violations (important when analyzer packages like Meziantou.Analyzer are updated), filtering output per the Token efficiency rules. When such updates introduce new warnings in existing code, fix the warnings rather than downgrading the analyzer — create `issue/<WORK_ITEM_KEY>` branches (merging applicable renovate branches) in the affected submodules if they don't already have one.
- For string equality warnings (e.g. MA0127), prefer `EqualsOrdinalIgnoreCase` and similar extensions from `Lombiq.HelpfulLibraries` (in `namespace System;`, no extra using needed) over raw `string.Equals(…, StringComparison.…)`.
- Avoid breaking changes even when an unrelated fix incidentally triggers one (e.g. removing a stale `CompatibilitySuppressions.xml` entry can be flagged as breaking by the packaging pipeline). If a PR gets an automated "this pull request appears to contain breaking changes" comment: prefer a non-breaking fix; otherwise, if the flagged change isn't actually breaking for consumers, add the `ignore-breaking-changes` label and push again; only accept a genuinely breaking change when unavoidable, and document the migration in the PR description.
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
- When `tools/Lombiq.GitHub.Actions` has changes (e.g. lock file maintenance in asset-lint), temporarily update its internal `@dev` refs to `@issue/<WORK_ITEM_KEY>` so CI resolves them from the issue branch.
- Update **all** `Lombiq/GitHub-Actions/...@dev` refs to `@issue/<WORK_ITEM_KEY>` in every `.yml` under `tools/Lombiq.GitHub.Actions/.github/` (`actions/` and `workflows/`). **Use a targeted regex matching `@dev` only when preceded by `Lombiq/GitHub-Actions/`** — never a global `@dev` replacement, since that would also break refs to other repos (e.g. `Lombiq/PowerShell-Analyzers`) that have no `issue/<WORK_ITEM_KEY>` branch.
- Apply the same targeted replacement to the **superproject's** `.github/workflows/*.yml` files, leaving other repo refs untouched.
- Commit the submodule change first, then stage the updated submodule pointer with the superproject workflow changes and commit.
- These refs are **temporary** — reverted to `@dev` in Phase 5.
- Validate workflow YAML syntax, then proceed directly to Phase 4 (no approval checkpoint).

### Phase 4: PR creation
Required state: `PR_CREATION`
Gate: proceed only after `APPROVED: Phase 2`

Actions:
- Push `issue/<WORK_ITEM_KEY>` branches for every repo that has one. **Skip** submodules where only a single renovate branch was checked out with no further changes — their existing Renovate PR is sufficient.
- Open the **superproject PR first**, targeting `dev`, referencing `<WORK_ITEM_KEY>`. **The title must literally start with `<WORK_ITEM_KEY>: `** (e.g. `OSOE-1311: Update dependencies`) — submodule `validate-pull-request`/`Check-Parent.ps1` checks search for this exact prefix in the superproject's open PR titles and fail otherwise. GitHub does not add this automatically.
- **Wait 60 seconds**, then open submodule PRs targeting `dev`, referencing `<WORK_ITEM_KEY>` in the description. Submodule PR titles must **not** include the issue key (added automatically from the branch name) — instead reference the specific updates, e.g. `Update dependencies: Microsoft.NET.Test.Sdk 18.0.1 → 18.3.0, Swashbuckle.AspNetCore 10.1.4 → 10.1.5`.
- PR bodies containing backticks: write to a temp file and use `gh pr create/edit --body-file <path>` instead of inline `--body "..."` (PowerShell backtick-escaping corrupts inline text — see PowerShell gotchas memory). Verify with `gh pr view <number> --json body --jq '.body'` after creation.
- After PRs are created, poll CI until all runs complete (see Token efficiency for interval/output guidance) via `gh run list` or `gh pr checks <number> --repo <repo> --json name,state --jq '.[] | "\(.name): \(.state)"'`. **Never use `gh run watch` or `gh pr checks --watch`** — both open a full-screen TUI that never returns control in a non-interactive terminal.
- Once the superproject PR's Ubuntu build (Build and Test) succeeds, add the `run-windows-build` label (`gh pr edit --add-label run-windows-build`) and wait for the Windows build too.
- Submodule PR checks must also pass, chiefly **Validate NuGet Publish** — the superproject PR's green checks alone aren't sufficient.
- If checks fail: investigate and fix. For test failures, reproduce and fix locally first (`dotnet test --filter`) — never use CI as the iteration loop. Push only once tests pass locally, then wait for CI before asking for approval.

Completion output:

```text
STATE: AWAITING_APPROVAL
STATUS: Awaiting approval for Phase 4 (PR Creation) — all CI checks passed.
```

### Phase 5: Finalization
Required state: `FINALIZATION`
Gate: proceed only after `APPROVED: Phase 4`

Actions:
- **Before merging any PRs**, revert the Phase 3 temporary GHA refs: in `tools/Lombiq.GitHub.Actions`, revert all `@issue/<WORK_ITEM_KEY>` refs back to `@dev` in every `.yml` under `.github/` (commit + push on `issue/<WORK_ITEM_KEY>`); do the same for the **superproject's** `.github/workflows/*.yml`. This ensures `@dev` self-references land on `dev` once merged.
- Merge **all** submodule branches to `dev` with `gh pr merge --merge --admin` (never squash/rebase, never `git push`/`git merge` directly onto `dev`; `--admin` bypasses merge queues/branch protection):
  - PRs for submodules with `issue/<WORK_ITEM_KEY>` branches (from Phase 4).
  - The existing Renovate PRs for submodules where only a single branch was checked out directly (no issue branch).
- After all submodule PRs are merged, update every submodule pointer in the superproject to the merged `dev` head (`git fetch origin dev && git checkout origin/dev` in each submodule) and commit alongside the workflow ref reverts. Include `[skip ci]` in these commit messages; don't wait for CI on them.
- Merge the superproject PR to `dev` only when explicitly approved, then check out the merged `dev` in the superproject (`git fetch origin dev && git checkout origin/dev`).
- Clean up local branches only when instructed.

Completion output:

```text
STATE: FINALIZATION
STATUS: Complete
```

## Scripts
**Always use these instead of generating equivalent inline commands or new scripts.** They encode the canonical filtering logic (age cutoff, merge check) and are the single source of truth.

### scripts/git/analyze-renovate-branches.sh
Read-only. Fetches all remotes, then reports every `renovate/*` branch in the superproject and each submodule as ELIGIBLE or SKIP (too old / already merged), with `diff --stat` and commit log vs. `origin/dev` for eligible ones. Configurable via `MAX_AGE_DAYS` (default 5). Covers the superproject too (unlike the checkout script). Never modifies branches, commits, or pushes.

### scripts/git/checkout-latest-renovate.sh
Fetches, then checks out the newest applicable `renovate/*` branch per repository (submodules only), skipping branches older than `MAX_AGE_DAYS` (default 5) or already merged into `origin/dev`. **Intentionally selects only one branch per repo** — additional eligible branches must be found via `analyze-renovate-branches.sh` and merged manually during Phase 2.

## Self-update policy
Trigger a self-update for general, reusable, project-wide feedback (logic/workflow corrections, new persistent instructions, helper script changes, safety/branching/approval/tooling refinements). Do not self-update for one-off, situational, or hypothetical feedback.

When triggered:
1. Stop normal execution.
2. Briefly explain the planned updates and affected files.
3. Ask: `Should I persist this change into the renovate-integration skill?`
4. Continue only after `CONFIRM SKILL UPDATE`.

Only update `SKILL.md`, `README.md`, and `scripts/*`; append an entry (date, summary, reason) to `CHANGELOG.md` without rewriting its history. Never remove safeguards unless explicitly instructed; preserve backward compatibility unless impossible.

## Repository context
OSOCE is a superproject with extensive Git submodule usage; submodules track `dev` via `.gitmodules`.

Renovate config files: `renovate.json5`, `renovate-osoce.json5`, `renovate-osoce-submodule.json5`, `renovate-osoce-orchard-core-submodule.json5`.

Key repository areas (non-exhaustive): `src/Modules/*`, `src/Libraries/*`, `src/Themes/*`, `src/Utilities/*`, `test/*`, `tools/*`.
