---
name: renovate-integration
description: Approval-gated workflow for integrating Renovate updates across the OSOCE superproject and submodules with strict branch and safety controls.
license: MIT
metadata:
  author: Lombiq Technologies
  version: "1.5"
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
- Work is driven by **open Renovate PRs**, not by scanning branches: enumerate PRs first, then check out their head branches. Never operate on a `renovate/*` branch that has no open PR.
- Never `git push` unless explicitly instructed. Never target `dev` directly — no `git push origin dev`/`HEAD:dev`/`issue/<WORK_ITEM_KEY>:dev`, no direct `git merge` into `dev`. All changes go through PRs.
- Commit only on `issue/<WORK_ITEM_KEY>`; never on `dev` or `renovate/*`. Don't prefix commit messages with `<WORK_ITEM_KEY>:` (already in the branch name).
- Push only via `scripts/git/push-issue-branches.sh <WORK_ITEM_KEY>`, which enforces the HEAD check.
- Before merging PRs (Phase 5), run `scripts/git/verify-dev-sync.sh`; stop and investigate if it reports DRIFT.
- Renovate can force-push new commits onto rolling branch names (e.g. `renovate/non-breaking-dependency-versions`, `renovate/major-browsers`) at any point, including while a long-running integration is still in progress. A PR staying open after its branch was merged doesn't necessarily mean the merge failed — verify with `scripts/gh/verify-open-renovate-prs.sh` (Phase 5) before assuming something is wrong.
- `[skip ci]` on a submodule-pointer commit is only safe when every pointer it changes was already covered by a passing CI run on the superproject PR. Never use it on a pointer-update commit that introduces submodule content the superproject's own CI hasn't actually built and tested yet — that defeats the purpose of the check.
- Never skip approval checkpoints or perform later-phase actions early.
- If a required tool is missing, stop and report it.

## Token efficiency
- Prefer the bundled scripts over ad-hoc command sequences: they already filter their output and replace many tool round-trips with one.
- Never dump full raw tool output (build logs, `git diff`, `gh ... --json`) into the conversation — summarize findings in a few bullets instead.
- Filter command output at the source rather than after the fact: request only the `gh`/`jq` fields you need (e.g. `--json name,state,conclusion`) instead of full JSON.
- Reuse Phase 1's `analyze-renovate-prs.sh` output for later phases instead of re-running it; don't re-run scripts to re-derive information already gathered.
- Read only the files/sections needed to make a decision (e.g. a package's changelog entry for the updated version range) rather than whole changelogs or unrelated diff hunks.

## Execution states
At the top of every response, declare exactly one state:

| State | Description |
| --- | --- |
| `INITIALIZED` | Work item key captured, ready to begin |
| `ANALYSIS` | Phase 1: reviewing open Renovate PRs |
| `AWAITING_APPROVAL` | Waiting for user approval |
| `IMPLEMENTATION` | Phase 2: applying changes |
| `PR_CREATION` | Phase 3-4: GitHub Actions and PR work |
| `FINALIZATION` | Phase 5: merge and cleanup |

## Workflow phases

### Phase 1: Analysis
Required state: `ANALYSIS`

Actions:
- Run `scripts/git/analyze-renovate-prs.sh` to discover **all** eligible **open Renovate PRs** across the superproject and every submodule (with their head branches, diffs and commit logs). Open PRs are the source of truth — `renovate/*` branches without an open PR are out of scope. **Never write ad-hoc git/gh commands for this — always use the script.**
- Also run (or read the output of) `scripts/git/checkout-latest-renovate-pr.sh` to confirm which single PR per submodule it would check out.
- Review release notes, PR descriptions and diffs; classify each change as Breaking, Risky, Non-trivial, or Feature.
- Record each eligible PR's repo, number and head branch — they're needed in Phases 2, 4 and 5.

Constraints:
- Do not modify code or branches.
- Do not generate scripts or inline commands duplicating `checkout-latest-renovate-pr.sh` / `analyze-renovate-prs.sh`.

Completion output:

```text
STATE: AWAITING_APPROVAL
STATUS: Awaiting approval for Phase 1 (Analysis)
```

### Phase 2: Implementation
Required state: `IMPLEMENTATION`
Gate: proceed only after `APPROVED: Phase 1`

Actions:
- Run `scripts/git/checkout-latest-renovate-pr.sh` to check out the head branch of the selected Renovate PR in each submodule. **Always use this script — never generate replacement commands.** In the superproject, check out an eligible PR's head branch with `gh pr checkout <number>` when it's the only one.
- Single eligible PR, no further changes needed → leave its head branch checked out; do **not** create an `issue/<WORK_ITEM_KEY>` branch (the existing Renovate PR suffices).
- Multiple eligible PRs to combine, or additional manual changes needed (e.g. GHA ref updates, patch bumps) → create `issue/<WORK_ITEM_KEY>` from `origin/dev` and merge every applicable PR's head branch into it with `git merge --no-ff` (always a merge commit, never fast-forward), including the ones the checkout script didn't select.
- If the **superproject** itself has an eligible open Renovate PR (e.g. head branch `renovate/non-breaking-dependency-versions`), merge its head branch into `issue/<WORK_ITEM_KEY>` instead of manually editing the same files.
- Resolve analyzer warnings, build/test failures, and lockfile updates. Build with `scripts/dotnet/build-with-analyzers.sh [solution]` (analyzers on, deduplicated diagnostics only). When analyzer package updates introduce new warnings in existing code, fix the warnings rather than downgrading the analyzer — create `issue/<WORK_ITEM_KEY>` branches (merging the applicable PR head branches) in the affected submodules if they don't already have one.
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
- When `tools/Lombiq.GitHub.Actions` has changes (e.g. lock file maintenance in asset-lint), its internal `Lombiq/GitHub-Actions/...@dev` refs must temporarily point at the issue branch so CI resolves them from there.
- Run `scripts/git/update-gha-refs.sh apply <WORK_ITEM_KEY>` from the superproject root. It rewrites only refs preceded by `Lombiq/GitHub-Actions/` in `tools/Lombiq.GitHub.Actions/.github/**` and the superproject's `.github/workflows/**`, leaving other repos' refs (e.g. `Lombiq/PowerShell-Analyzers`) untouched. **Never hand-roll this replacement.**
- Commit the submodule change first, then stage the updated submodule pointer with the superproject workflow changes and commit.
- These refs are **temporary** — reverted in Phase 5 with `update-gha-refs.sh revert <WORK_ITEM_KEY>`.
- Validate workflow YAML syntax, then proceed directly to Phase 4 (no approval checkpoint).

### Phase 4: PR creation
Required state: `PR_CREATION`
Gate: proceed only after `APPROVED: Phase 2`

Actions:
- Run `scripts/git/push-issue-branches.sh <WORK_ITEM_KEY>` to push every repo that has an `issue/<WORK_ITEM_KEY>` branch checked out. It skips repos on any other branch (including those left on a Renovate PR head branch, whose existing PR is sufficient).
- Open the **superproject PR first**, targeting `dev`, referencing `<WORK_ITEM_KEY>`. **The title must literally start with `<WORK_ITEM_KEY>: `** (e.g. `OSOE-1311: Update dependencies`) — submodule `validate-pull-request`/`Check-Parent.ps1` checks search for this exact prefix in the superproject's open PR titles and fail otherwise. GitHub does not add this automatically.
- **Wait 60 seconds**, then open submodule PRs targeting `dev`, referencing `<WORK_ITEM_KEY>` in the description. Submodule PR titles must **not** include the issue key (added automatically from the branch name) — instead reference the specific updates, e.g. `Update dependencies: Microsoft.NET.Test.Sdk 18.0.1 → 18.3.0, Swashbuckle.AspNetCore 10.1.4 → 10.1.5`.
- PR bodies containing backticks: write to a temp file and use `gh pr create/edit --body-file <path>` instead of inline `--body "..."` (PowerShell backtick-escaping corrupts inline text — see PowerShell gotchas memory). Verify with `gh pr view <number> --json body --jq '.body'` after creation.
- After PRs are created, wait for CI with `scripts/gh/wait-for-checks.sh <owner/repo> <pr-number>` (polls with backoff, prints a one-line summary plus failing checks, exits non-zero on failure). **Never use `gh run watch` or `gh pr checks --watch`** — both open a full-screen TUI that never returns control in a non-interactive terminal.
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
- **Before merging any PRs**, run `scripts/git/verify-dev-sync.sh` (stop on DRIFT), then revert the Phase 3 temporary refs with `scripts/git/update-gha-refs.sh revert <WORK_ITEM_KEY>` and commit + push the result in both `tools/Lombiq.GitHub.Actions` and the superproject. This ensures `@dev` self-references land on `dev` once merged.
- Merge **all** submodule branches to `dev` with `gh pr merge --merge --admin` (never squash/rebase, never `git push`/`git merge` directly onto `dev`; `--admin` bypasses merge queues/branch protection):
  - PRs for submodules with `issue/<WORK_ITEM_KEY>` branches (from Phase 4).
  - The existing Renovate PRs (by number, from Phase 1) for submodules where only a single PR branch was checked out directly (no issue branch).
- After all submodule PRs are merged, run `scripts/git/update-submodule-pointers.sh` to move every submodule to the merged `origin/dev` head and stage the pointers, then commit them alongside the workflow ref reverts.
  - Include `[skip ci]` in this commit message **only** when it moves every pointer to exactly the commits Phase 4's CI wait already validated (the routine, single-pass case where nothing changed since) — the superproject's own build was never actually re-run against these exact submodule commits otherwise.
  - If any submodule content changed since Phase 4's CI wait (a later fold-in round, see below), the pointer-update commit must **not** use `[skip ci]`: push it normally and run `scripts/gh/wait-for-checks.sh` again on the superproject PR, the same as Phase 4, before merging.
- Merge the superproject PR to `dev` only when explicitly approved, then check out the merged `dev` in the superproject (`git fetch origin dev && git checkout origin/dev`).
- Before the superproject PR is merged, run `scripts/gh/verify-open-renovate-prs.sh` and report the result. Renovate reuses rolling branch names (e.g. `renovate/non-breaking-dependency-versions`, `renovate/major-browsers`) and can force-push new commits to them at any time, including mid-integration — a PR whose earlier content was merged can still show as open with new content by the time Phase 5 finishes.
  - Default handling: `UPDATED-MID-INTEGRATION` results are expected and safe to leave for the next integration pass.
  - If explicitly instructed to fold an `UPDATED-MID-INTEGRATION` PR into the current batch instead: re-fetch its current head (branches like these can move again between the check and the merge — re-verify the head commit right before merging, don't assume an earlier snapshot is still current), repeat Phase 2–4 for it alone (fresh `issue/<WORK_ITEM_KEY>` branch off current `origin/dev`, merge, build-validate, push, open a PR, wait for its own CI, merge with `--admin`), then redo `update-submodule-pointers.sh` and the superproject CI wait as described above — never bypass CI for a pointer bump that includes new content.
  - `MERGED-BUT-STALE` or `NOT-YET-INTEGRATED` (for PRs that were supposed to be part of this batch) need investigation.
- Clean up local branches only when instructed.

Completion output:

```text
STATE: FINALIZATION
STATUS: Complete
```

## Scripts
**Always use these instead of generating equivalent inline commands or new scripts.** They encode the canonical logic (filters, targeted regexes, safety checks) and are the single source of truth. All are run from the superproject root with `bash .agents/skills/renovate-integration/scripts/<path>`. The `gh/` and PR-listing scripts need an authenticated `gh` CLI.

| Script | Phase | Purpose |
| --- | --- | --- |
| `git/analyze-renovate-prs.sh` | 1 | Read-only. Lists every **open PR with a `renovate/*` head branch** in the superproject and each submodule as ELIGIBLE or SKIP (draft / already merged), with PR number, title, `diff --stat` and commit log vs. `origin/dev`. No age cutoff — every open Renovate PR is checked regardless of age. |
| `git/checkout-latest-renovate-pr.sh` | 2 | Checks out the head branch of the newest applicable open Renovate PR per submodule (same filters). **One PR per repo**, but prints the other eligible PR numbers/branches to merge manually. |
| `dotnet/build-with-analyzers.sh [target]` | 2 | Builds with `RunAnalyzersDuringBuild=true`, printing only deduplicated error/warning lines. Defaults to `Lombiq.OSOCE.slnx`. |
| `git/update-gha-refs.sh apply\|revert <KEY>` | 3, 5 | Rewrites `Lombiq/GitHub-Actions/...@dev` ↔ `@issue/<KEY>` in `tools/Lombiq.GitHub.Actions/.github/**` and `.github/workflows/**`; never touches other repos' refs. Doesn't commit. |
| `git/push-issue-branches.sh <KEY>` | 4 | Pushes `issue/<KEY>` in the superproject and every submodule where it's the checked-out branch; skips all others. Never force-pushes or targets `dev`. |
| `gh/wait-for-checks.sh <owner/repo> <pr>` | 4 | Polls PR checks with 60s→300s backoff, prints a compact summary and failing checks; exit 0 pass / 1 fail / 3 timeout. No TUI. |
| `git/verify-dev-sync.sh` | 5 | Verifies local `dev` matches `origin/dev` everywhere; exits non-zero on drift. |
| `git/update-submodule-pointers.sh` | 5 | Moves every submodule to merged `origin/dev` and stages changed pointers in the superproject (requires an `issue/*` branch; doesn't commit). |
| `gh/verify-open-renovate-prs.sh` | 5 | Read-only. Lists every still-**open** `renovate/*` PR post-merge as `MERGED-BUT-STALE` (unexpectedly not auto-closed), `UPDATED-MID-INTEGRATION` (Renovate force-pushed new commits after we merged an earlier snapshot — expected), or `NOT-YET-INTEGRATED` (no merge commit found — verify it wasn't missed). |
| `tests/parse-check.sh` | — | Validates the embedded shell bodies of the PR scripts after editing them. |

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
