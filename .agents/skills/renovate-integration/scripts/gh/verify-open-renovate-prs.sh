#!/usr/bin/env bash
# verify-open-renovate-prs.sh
#
# Post-merge sanity check (Phase 5): Renovate reuses rolling branch names (e.g.
# "renovate/non-breaking-dependency-versions", "renovate/major-browsers") and can
# force-push new commits to them at any time, including while a long-running
# integration (CI waits, approval checkpoints) is still in progress. When that
# happens, a PR whose earlier content was already merged stays OPEN, because its
# current head commit is no longer the one that got merged.
#
# For every OPEN PR with a "renovate/*" head branch in the superproject and each
# submodule, this reports one of:
#   - MERGED-BUT-STALE: the PR's current head commit IS an ancestor of origin/dev.
#     Unexpected — GitHub should normally auto-close this. Investigate/close by hand.
#   - UPDATED-MID-INTEGRATION: dev already contains a "Merge pull request #<N> ..."
#     commit for this PR number, but the PR's current head commit is NOT an
#     ancestor of dev. Renovate pushed new commits after we merged an earlier
#     snapshot of the branch — expected, not a bug. Leave it for the next
#     integration pass and shows the diff of what changed since.
#   - NOT-YET-INTEGRATED: no merge commit for this PR number was found in dev at
#     all. Verify whether it was simply missed in this integration.
#
# Usage:
#   bash .agents/skills/renovate-integration/scripts/gh/verify-open-renovate-prs.sh
#
# Requirements:
#   - gh (GitHub CLI), authenticated.
#
# Safety:
#   - Read-only apart from fetching; never checks out, resets, commits or pushes.

set -euo pipefail

if ! command -v gh >/dev/null 2>&1; then
  echo "ERROR: gh (GitHub CLI) is required but not installed." >&2
  exit 1
fi

# Shared body used for both the superproject and (via submodule foreach) each submodule.
# Expects $label to be set.
verify_repo_body='
  git fetch --prune 2>/dev/null || echo "WARN: fetch failed for $label"

  prs=$(gh pr list --state open --limit 100 \
    --json number,title,headRefName \
    --jq ".[] | select(.headRefName | startswith(\"renovate/\")) | [.number, .headRefName, .title] | @tsv" \
    2>/dev/null || true)

  if [ -z "$prs" ]; then
    exit 0
  fi

  echo "=========================================="
  echo "$label"
  echo "=========================================="

  printf "%s\n" "$prs" | while IFS="$(printf "\t")" read -r number head title; do
    ref="origin/$head"
    head_sha=$(git rev-parse "$ref" 2>/dev/null || true)
    if [ -z "$head_sha" ]; then
      echo "  SKIP #$number $head: head branch not found locally after fetch"
      continue
    fi

    if git merge-base --is-ancestor "$ref" origin/dev 2>/dev/null; then
      echo "  MERGED-BUT-STALE #$number $head ($title): head is already an ancestor of dev but the PR is still open — investigate/close by hand."
      continue
    fi

    merge_commit=$(git log --oneline --grep="Merge pull request #$number from Lombiq/" origin/dev 2>/dev/null | head -1 || true)
    if [ -n "$merge_commit" ]; then
      echo "  UPDATED-MID-INTEGRATION #$number $head ($title): already merged earlier ($merge_commit), but Renovate pushed new commits since. Expected — leave for the next integration pass."
      echo "    --- Diff of new content vs what was merged ---"
      merged_sha=$(printf "%s" "$merge_commit" | cut -d" " -f1)
      git diff --stat "$merged_sha" "$ref" 2>/dev/null | head -10
    else
      echo "  NOT-YET-INTEGRATED #$number $head ($title): no merge commit for this PR number found in dev — verify it was not simply missed."
    fi
  done
'

# Superproject.
bash -c "label='SUPERPROJECT'; $verify_repo_body"
echo ""

# Submodules ($name is provided by git submodule foreach).
git submodule foreach --recursive "label=\"\$name\"; $verify_repo_body"
