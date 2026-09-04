# Changelog — renovate-integration skill

All notable changes to this skill are documented here. This log is **append-only** — entries must not be retroactively edited or removed.

---

## 2026-09-04 (2)

**Remove the age-based eligibility cutoff**

- Removed `MAX_AGE_DAYS` (previously defaulted to 5 days) from `scripts/git/analyze-renovate-prs.sh` and `scripts/git/checkout-latest-renovate-pr.sh`. Every open Renovate PR is now checked and included regardless of how old it is — an open PR is the only eligibility signal (besides draft state and already-merged-into-`dev`).
- Reason: PRs were being silently excluded from analysis purely for being old, even though they were still open, unmerged and otherwise applicable.

---

## 2026-09-04

**Script the remaining deterministic steps instead of describing them in prose**

- Added `scripts/git/update-gha-refs.sh apply|revert <KEY>` (targeted `Lombiq/GitHub-Actions/...@dev` ↔ `@issue/<KEY>` rewrite across the submodule's and superproject's workflow YAMLs), replacing the Phase 3/5 regex instructions.
- Added `scripts/git/push-issue-branches.sh <KEY>`, which enforces the "HEAD must be `issue/<KEY>`" push rule across the superproject and all submodules.
- Added `scripts/gh/wait-for-checks.sh <owner/repo> <pr>`, replacing the manual CI polling loop (60s→300s backoff, compact summary, no `--watch` TUI).
- Added `scripts/git/verify-dev-sync.sh` (pre-merge `dev` drift check) and `scripts/git/update-submodule-pointers.sh` (move submodules to merged `origin/dev` and stage pointers).
- Added `scripts/dotnet/build-with-analyzers.sh` for analyzer-enabled builds with deduplicated diagnostics only.
- Added `scripts/tests/gha-refs-test.sh` (sandbox apply/revert round-trip test proving non-Lombiq/GitHub-Actions refs are untouched).
- Condensed the Scripts section into a table and removed the prose the scripts now encode.
- Bumped `metadata.version` to `1.5`.

Reason: User request to move verbal instructions into scripts for fewer tokens and more deterministic execution.

---

## 2026-09-03

**Drive the workflow from open Renovate PRs instead of `renovate/*` branches**

- Phase 1 now enumerates **open Renovate pull requests** (PRs whose head branch starts with `renovate/`) in the superproject and every submodule, and records their numbers and head branches; branches without an open PR are out of scope.
- Phase 2 checks out those PRs' head branches (and merges the additional ones into `issue/<WORK_ITEM_KEY>`); Phases 4-5 reference the PR numbers gathered in Phase 1.
- Replaced `scripts/git/analyze-renovate-branches.sh` with `scripts/git/analyze-renovate-prs.sh` and `scripts/git/checkout-latest-renovate.sh` with `scripts/git/checkout-latest-renovate-pr.sh`. Both use `gh pr list` (authenticated `gh` is now a requirement), skip draft PRs in addition to the existing age/already-merged filters, and the checkout script now prints the other eligible PRs it didn't check out.
- Bumped `metadata.version` to `1.4`.

Reason: Explicit user request — open PRs are the accurate signal of pending Renovate work, whereas raw `renovate/*` branch scanning also surfaces abandoned/closed-PR branches.

---

## 2026-08-27

**Reduce token usage during execution, not just file size**

- Added a "Token efficiency" section with rules that apply while running the workflow: never dump full raw tool output (build logs, `git diff`, `gh ... --json`) into the conversation — summarize instead; filter command output at the source (grep/`Select-String` for errors/warnings, minimal `--json`/`jq` fields); reuse Phase 1's `analyze-renovate-branches.sh` output instead of re-running it; back off CI polling to 3-5 minutes for longer-running workflows; read only the files/sections needed instead of whole changelogs/diffs.
- Phase 2 and Phase 4 now point to this section instead of repeating ad-hoc guidance.
- Bumped `metadata.version` to `1.3`.

Reason: Explicit user follow-up request — the previous pass only shrank `SKILL.md` itself; this pass targets tokens consumed while the skill is actually being executed (tool outputs, polling loops, log dumps).

---

## 2026-08-27

**Condense SKILL.md for token efficiency**

- Rewrote `SKILL.md` to tighten prose throughout (Global safety rules, Phases 1-5, Scripts, Self-update policy, Repository context), merging redundant bullets and cutting rationale/explanatory text that duplicates what's already in `CHANGELOG.md` or user memory (e.g. the PowerShell backtick-escaping mechanism). No behavioral rules, safety gates, or actionable steps were removed — only wording was shortened.
- Bumped `metadata.version` to `1.2`.

Reason: Explicit user request to optimize the skill for token usage, since the full `SKILL.md` is loaded on every invocation.

---

## 2026-08-21

**Never use `gh run watch` or `gh pr checks --watch` for polling**

- Added a rule to Phase 4 forbidding `gh run watch` and `gh pr checks --watch`: both open a full-screen alternate-buffer TUI that does not return control when run through the terminal tool. Poll instead with plain, repeated `gh pr checks <number> --repo <repo> --json name,state --jq '...'` calls (no watch flag).

Reason: During OSOE-1311 Phase 5, both `gh run watch` and `gh pr checks --watch` got the terminal stuck in an alternate-buffer/TUI state that didn't respond to further input, wasting turns recovering it.

---

## 2026-08-21

**Fix PR body corruption from PowerShell backtick escaping; document the superproject PR title prefix requirement**

- Added a rule to Phase 4 to never pass PR body text with backticks as an inline `--body "..."` PowerShell string, since PowerShell's backtick escape character silently eats letters like `r`/`n`/`t` and replaces them with control characters (e.g. `` `renovate `` becomes a line break plus "enovate"). Always use `--body-file` with a temp file instead, and verify the rendered body afterwards.
- Corrected the PR title guidance: the superproject PR title must literally start with `<WORK_ITEM_KEY>: ` (submodule `validate-pull-request` checks search for this exact prefix in the superproject's open PR titles), whereas submodule PR titles should not include the key.

Reason: During OSOE-1311, all 9 PR bodies were corrupted (missing letters, stray line breaks) because backtick-wrapped branch/file names in `--body "..."` arguments were parsed as PowerShell escape sequences. Separately, the superproject PR was initially created without the `<WORK_ITEM_KEY>: ` prefix, which is required by submodule PR validation but wasn't previously documented as a strict requirement.

---

## 2026-08-20

**Avoid breaking changes in Phase 2; follow the automated breaking-changes PR comment**

- Added a bullet to Phase 2 instructing to avoid breaking changes whenever feasible, and to follow the repo's automated "this pull request appears to contain breaking changes" PR comment (prefer a non-breaking fix; otherwise apply the `ignore-breaking-changes` label if the flagged change isn't actually breaking for consumers, and only accept a genuine breaking change with a documented migration when unfeasible to avoid).

Reason: During OSOE-1311, fixing a stale `CompatibilitySuppressions.xml` entry (removing an unnecessary suppression) was itself flagged as a breaking change by the packaging pipeline, requiring the `ignore-breaking-changes` label per the repo's own automated PR comment.

---

## 2026-08-20

**Require submodule PR checks to pass in Phase 4, not just the superproject's**

- Added a bullet to Phase 4 stating that submodule PR checks must also pass, chiefly the **Validate NuGet Publish** workflow, before proceeding to ask for approval.

Reason: User feedback during OSOE-1311 — the skill previously only called out waiting for the superproject PR's Ubuntu/Windows builds, which could let submodule PRs with failing checks (e.g. Validate NuGet Publish) slip through to Phase 5 merging.

---

## 2026-03-27

**Fix Phase 5: revert GHA refs in submodule before merging**

- Updated Phase 5 to revert `@issue/<WORK_ITEM_KEY>` → `@dev` in `tools/Lombiq.GitHub.Actions` (commit + push on the issue branch) **before** merging the submodule PR, not just in the superproject after merging.
- Without this, the `@issue/...` self-references would land on `dev` in the GitHub-Actions repo.

Reason: In OSOE-1252, the GitHub-Actions submodule was merged to `dev` with `@issue/OSOE-1252` refs still in place because Phase 5 only instructed reverting in the superproject.

---

## 2026-03-27

**Add analyze-renovate-branches.sh script for Phase 1**

- Created `scripts/git/analyze-renovate-branches.sh` — a read-only script that lists **all** eligible `renovate/*` branches (with diffs and commit logs) across both the superproject and submodules.
- Updated Phase 1 to reference the new script instead of requiring manual `git for-each-ref` commands after running the checkout script.
- Updated Phase 1 constraints to also forbid duplicating the new script's logic.
- Added script documentation to the Scripts section of SKILL.md.

Reason: During session OSOE-1252, ad-hoc temp scripts (`_list_renovate.sh`, `_check_renovate.sh`) were created to fill a gap — the existing `checkout-latest-renovate.sh` only selects one branch per submodule and doesn't cover the superproject. The new script makes Phase 1 analysis deterministic and eliminates the need for ad-hoc scripts.

---

## 2026-03-19

**Build with RunAnalyzersDuringBuild in Phase 2**

- Updated Phase 2 "Resolve analyzer warnings" step to specify building with `/property:RunAnalyzersDuringBuild=true` to surface analyzer violations, especially when analyzer packages (e.g. Meziantou.Analyzer) are updated.

Reason: Default builds don't run analyzers during build; enabling this flag catches new violations introduced by analyzer package updates.

---

## 2026-03-12

**Do not prefix commit messages with the issue key**

- Added global safety rule: commit messages must not be prefixed with `<WORK_ITEM_KEY>:` since the key is already encoded in the branch name.

Reason: Avoid redundant issue key prefixes in commit messages.

---

## 2026-03-12

**PR creation order and issue branch rules**

- Phase 2: Do not create `issue/<WORK_ITEM_KEY>` branches in submodules with only a single renovate branch and no further changes — leave the renovate branch and let the existing Renovate PR handle it.
- Phase 4: Open the superproject PR first, wait 60 seconds, then create submodule PRs. Only create PRs for submodules that have `issue/<WORK_ITEM_KEY>` branches.

Reason: Avoid redundant PRs for single-branch submodules and ensure the superproject PR is created before submodule PRs for correct cross-repository reference resolution.

---

## 2026-03-12

**Handle multiple renovate branches per submodule**

- Phase 1: Added instruction to manually list all remaining `renovate/*` branches in each affected submodule after the script runs, since the script only selects one per submodule.
- Phase 2: Added instruction to merge any additional eligible renovate branches identified during Phase 1 into `issue/<WORK_ITEM_KEY>`.
- Scripts section: Clarified that the script intentionally selects only one branch per repository, and additional branches must be discovered and merged manually.

Reason: The script's one-branch-per-submodule design caused additional eligible renovate branches (e.g. `renovate/all-dependencies`, `renovate/browsers`) to be silently skipped.

---

## 2026-03-12

**Enforce use of existing script, disallow ad-hoc replacements**

- Phase 1 now explicitly requires running `scripts/git/checkout-latest-renovate.sh` instead of writing custom git commands.
- Phase 2 changed from "use where applicable" to mandatory script usage.
- Scripts section strengthened: scripts are the single source of truth; generating equivalent inline commands is prohibited.

Reason: Prevent the agent from ignoring the script and generating its own branch-discovery logic.

---

## 2026-03-12

**Clarify Phase 2 superproject renovate branch handling and Phase 3 branch reference updates**

- Phase 2: Added instruction to check for and merge the superproject's own `renovate/*` branch instead of manually editing files that Renovate already updated.
- Phase 3: Expanded with detailed steps for temporarily updating `@dev` branch references to `@issue/<WORK_ITEM_KEY>` in both `tools/Lombiq.GitHub.Actions/.github/` and the superproject's `.github/workflows/` when GitHub Actions has changes (e.g. lock file maintenance in asset-lint).
- Phase 3: Clarified that these are temporary references reverted in Phase 5.

Reason: Ensure consistency — superproject dependency updates should come from Renovate branches, and CI must resolve GitHub Actions references from the issue branch during testing.

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
