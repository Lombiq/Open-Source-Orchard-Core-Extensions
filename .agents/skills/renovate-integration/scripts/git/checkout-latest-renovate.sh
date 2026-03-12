#!/usr/bin/env bash
# checkout-latest-renovate.sh
#
# Checks out the latest applicable renovate/* branch in each Git submodule.
#
# Behavior:
#   - Fetches all remotes in each submodule.
#   - Finds renovate/* branches (by committer date, newest first).
#   - Skips branches older than 5 days.
#   - Skips branches already merged into origin/dev.
#   - Checks out ONLY ONE renovate branch per submodule (the newest applicable).
#
# NOTE: Other renovate branches may exist in each submodule. This script
# intentionally selects only the newest applicable one. If you need to
# integrate a different branch, do so manually.
#
# Usage:
#   bash .agents/skills/renovate-integration/scripts/git/checkout-latest-renovate.sh
#
# Safety:
#   - This script does NOT push anything.
#   - This script does NOT commit anything.
#   - It only checks out local branches tracking the selected renovate remote branch.

set -euo pipefail

MAX_AGE_DAYS=5

git submodule foreach --recursive '
  echo "--- Processing: $name ---"
  git fetch || { echo "WARN: fetch failed for $name, skipping"; exit 0; }

  dev_commit=$(git rev-parse origin/dev 2>/dev/null || { echo "WARN: origin/dev not found in $name, skipping"; exit 0; })

  cutoff=$(date -d "'"$MAX_AGE_DAYS"' days ago" +%s 2>/dev/null || date -v-'"$MAX_AGE_DAYS"'d +%s)

  chosen=""
  for ref in $(git for-each-ref --sort=-committerdate --format="%(refname:short)" refs/remotes/origin/renovate/*); do
    ref_ts=$(git show -s --format=%ct "$ref")

    # Skip branches older than the cutoff.
    if [ "$ref_ts" -lt "$cutoff" ]; then
      echo "Skipping $ref: older than '"$MAX_AGE_DAYS"' days."
      continue
    fi

    # Skip branches already merged into origin/dev.
    if git merge-base --is-ancestor "$ref" origin/dev 2>/dev/null; then
      echo "Skipping $ref: already merged into origin/dev."
      continue
    fi

    chosen="$ref"
    break
  done

  if [ -z "$chosen" ]; then
    echo "No eligible renovate/* branches in $name (all too old or already merged), skipping."
    exit 0
  fi

  local_branch=${chosen#origin/}
  echo "Checking out $local_branch (from $chosen) in $name"
  git checkout -B "$local_branch" "$chosen"
'
