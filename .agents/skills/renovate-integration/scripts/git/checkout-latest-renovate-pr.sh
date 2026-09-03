#!/usr/bin/env bash
# checkout-latest-renovate-pr.sh
#
# Checks out the head branch of the latest applicable OPEN Renovate pull request
# in each Git submodule.
#
# Behavior:
#   - Fetches all remotes in each submodule.
#   - Lists open PRs whose head branch starts with renovate/ (newest head commit first).
#   - Skips draft PRs (Renovate rate-limited).
#   - Skips PRs whose head commit is older than 5 days.
#   - Skips PRs already merged into origin/dev.
#   - Checks out ONLY ONE Renovate PR branch per submodule (the newest applicable),
#     and lists the remaining eligible PRs so they can be merged manually.
#
# Usage:
#   bash .agents/skills/renovate-integration/scripts/git/checkout-latest-renovate-pr.sh
#
# Requirements:
#   - gh (GitHub CLI), authenticated.
#
# Safety:
#   - This script does NOT push anything.
#   - This script does NOT commit anything.
#   - It only checks out local branches tracking the selected PR's head branch.

set -euo pipefail

MAX_AGE_DAYS="${MAX_AGE_DAYS:-5}"

if ! command -v gh >/dev/null 2>&1; then
  echo "ERROR: gh (GitHub CLI) is required but not installed." >&2
  exit 1
fi

export MAX_AGE_DAYS

git submodule foreach --recursive '
  echo "--- Processing: $name ---"
  git fetch --prune || { echo "WARN: fetch failed for $name, skipping"; exit 0; }

  git rev-parse origin/dev >/dev/null 2>&1 || { echo "WARN: origin/dev not found in $name, skipping"; exit 0; }

  prs=$(gh pr list --state open --limit 100 \
    --json number,title,headRefName,isDraft \
    --jq ".[] | select(.headRefName | startswith(\"renovate/\")) | [.number, .headRefName, .isDraft] | @tsv" \
    2>/dev/null || true)

  if [ -z "$prs" ]; then
    echo "No open Renovate PRs in $name, skipping."
    exit 0
  fi

  cutoff=$(date -d "$MAX_AGE_DAYS days ago" +%s 2>/dev/null || date -v-"$MAX_AGE_DAYS"d +%s)

  # Collect eligible PRs as "<head commit timestamp> <number> <branch>", newest first.
  eligible=$(printf "%s\n" "$prs" | while IFS="$(printf "\t")" read -r number head is_draft; do
    ref="origin/$head"

    if [ "$is_draft" = "true" ]; then
      echo "Skipping #$number $head: draft PR." >&2
      continue
    fi

    ref_ts=$(git show -s --format=%ct "$ref" 2>/dev/null || true)
    if [ -z "$ref_ts" ]; then
      echo "Skipping #$number $head: head branch not found locally after fetch." >&2
      continue
    fi

    if [ "$ref_ts" -lt "$cutoff" ]; then
      echo "Skipping #$number $head: older than $MAX_AGE_DAYS days." >&2
      continue
    fi

    if git merge-base --is-ancestor "$ref" origin/dev 2>/dev/null; then
      echo "Skipping #$number $head: already merged into origin/dev." >&2
      continue
    fi

    echo "$ref_ts $number $head"
  done | sort -rn)

  if [ -z "$eligible" ]; then
    echo "No eligible open Renovate PRs in $name (all draft, too old or already merged), skipping."
    exit 0
  fi

  chosen=$(printf "%s\n" "$eligible" | head -1)
  chosen_number=$(printf "%s" "$chosen" | cut -d" " -f2)
  chosen_head=$(printf "%s" "$chosen" | cut -d" " -f3)

  echo "Checking out $chosen_head (PR #$chosen_number) in $name"
  git checkout -B "$chosen_head" "origin/$chosen_head"

  others=$(printf "%s\n" "$eligible" | tail -n +2)
  if [ -n "$others" ]; then
    echo "Other eligible Renovate PRs in $name (merge manually in Phase 2):"
    printf "%s\n" "$others" | while read -r ts number head; do
      echo "  #$number $head"
    done
  fi
'
