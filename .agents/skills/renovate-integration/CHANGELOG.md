# Changelog — renovate-integration skill

All notable changes to this skill are documented here. This log is **append-only** — entries must not be retroactively edited or removed.

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
