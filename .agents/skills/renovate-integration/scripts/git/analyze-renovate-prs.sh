#!/usr/bin/env bash
# analyze-renovate-prs.sh
#
# Read-only analysis of all OPEN Renovate pull requests across the superproject
# and every submodule. For each PR it reports:
#   - Number, title, head branch, draft state and last update.
#   - Whether it is ELIGIBLE or SKIPPED (draft / too old / already merged into origin/dev).
#   - A diff --stat against origin/dev.
#   - A commit log against origin/dev.
#
# PRs are the source of truth: only branches that have an open Renovate PR are
# considered. Stale renovate/* branches without a PR are ignored on purpose.
#
# Usage:
#   bash .agents/skills/renovate-integration/scripts/git/analyze-renovate-prs.sh
#
# Requirements:
#   - gh (GitHub CLI), authenticated.
#
# Safety:
#   - This script does NOT modify any branches.
#   - This script does NOT commit or push anything.
#   - Fetches all remotes before evaluating PR head branches.

set -euo pipefail

MAX_AGE_DAYS="${MAX_AGE_DAYS:-5}"

if ! command -v gh >/dev/null 2>&1; then
  echo "ERROR: gh (GitHub CLI) is required but not installed." >&2
  exit 1
fi

# Shared body used for both the superproject and (via submodule foreach) each submodule.
# Expects $label to be set.
analyze_repo_body='
  git fetch --prune 2>/dev/null || echo "WARN: fetch failed for $label"

  prs=$(gh pr list --state open --limit 100 \
    --json number,title,headRefName,isDraft,updatedAt \
    --jq ".[] | select(.headRefName | startswith(\"renovate/\")) | [.number, .headRefName, .isDraft, .updatedAt, .title] | @tsv" \
    2>/dev/null || true)

  if [ -z "$prs" ]; then
    exit 0
  fi

  echo "=========================================="
  echo "$label"
  echo "=========================================="

  cutoff=$(date -d "$MAX_AGE_DAYS days ago" +%s 2>/dev/null || date -v-"$MAX_AGE_DAYS"d +%s)

  printf "%s\n" "$prs" | while IFS="$(printf "\t")" read -r number head is_draft updated title; do
    ref="origin/$head"

    if [ "$is_draft" = "true" ]; then
      echo "  SKIP #$number $head: draft PR (Renovate rate-limited)"
      continue
    fi

    ref_ts=$(git show -s --format=%ct "$ref" 2>/dev/null || true)
    if [ -z "$ref_ts" ]; then
      echo "  SKIP #$number $head: head branch not found locally after fetch"
      continue
    fi

    ref_date=$(git show -s --format=%ci "$ref" 2>/dev/null)

    if [ "$ref_ts" -lt "$cutoff" ]; then
      echo "  SKIP #$number $head ($ref_date): older than $MAX_AGE_DAYS days"
      continue
    fi

    if git merge-base --is-ancestor "$ref" origin/dev 2>/dev/null; then
      echo "  SKIP #$number $head ($ref_date): already merged into origin/dev"
      continue
    fi

    echo "  ELIGIBLE #$number $head ($ref_date)"
    echo "    Title: $title"
    echo "  --- Diff against origin/dev ---"
    git diff --stat "origin/dev...$ref" 2>/dev/null | head -30
    echo "  --- Commit log ---"
    git log --oneline "origin/dev..$ref" 2>/dev/null | head -10
    echo ""
  done
'

# Superproject.
export MAX_AGE_DAYS
bash -c "label='SUPERPROJECT'; $analyze_repo_body"
echo ""

# Submodules ($name is provided by git submodule foreach).
git submodule foreach --recursive "label=\"\$name\"; $analyze_repo_body"
