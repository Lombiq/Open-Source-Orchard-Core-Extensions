#!/usr/bin/env bash
# checkout-latest-renovate.sh
#
# Checks out the latest applicable renovate/* branch in each Git submodule.
#
# Behavior:
#   - Fetches all remotes in each submodule.
#   - Finds the newest renovate/* branch (by committer date).
#   - Skips it if its commit is older than origin/dev.
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

git submodule foreach --recursive '
  echo "--- Processing: $name ---"
  git fetch || { echo "WARN: fetch failed for $name, skipping"; exit 0; }

  renovate=$(git for-each-ref --sort=-committerdate --format="%(refname:short)" refs/remotes/origin/renovate/* | head -n1)

  if [ -z "$renovate" ]; then
    echo "No renovate/* branches found in $name, skipping."
    exit 0
  fi

  renovate_commit=$(git rev-parse "$renovate")
  dev_commit=$(git rev-parse origin/dev 2>/dev/null || { echo "WARN: origin/dev not found in $name, skipping"; exit 0; })

  renovate_ts=$(git show -s --format=%ct "$renovate_commit")
  dev_ts=$(git show -s --format=%ct "$dev_commit")

  if [ "$renovate_ts" -gt "$dev_ts" ]; then
    local_branch=${renovate#origin/}
    echo "Checking out $local_branch (from $renovate) in $name"
    git checkout -B "$local_branch" "$renovate"
  else
    echo "Newest renovate branch ($renovate) is not newer than origin/dev in $name, skipping."
  fi
'
