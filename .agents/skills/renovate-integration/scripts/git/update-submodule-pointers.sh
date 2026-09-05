#!/usr/bin/env bash
# update-submodule-pointers.sh
#
# Phase 5 step: after all submodule PRs are merged, points every submodule at the
# merged origin/dev head and stages the updated pointers in the superproject.
#
# Behavior:
#   - In each submodule: fetch, then check out origin/dev (detached).
#   - In the superproject: stage every submodule path whose pointer changed.
#   - Prints the list of updated submodules and leaves committing to the caller
#     (commit message must contain [skip ci]).
#
# Usage:
#   bash .agents/skills/renovate-integration/scripts/git/update-submodule-pointers.sh
#
# Safety:
#   - Does NOT commit or push anything.
#   - Must be run from the superproject root, with the superproject on
#     issue/<WORK_ITEM_KEY> (verified below).

set -euo pipefail

branch=$(git rev-parse --abbrev-ref HEAD 2>/dev/null || echo "")
case "$branch" in
  issue/*) ;;
  *)
    echo "ERROR: superproject is on '$branch'; expected an issue/<WORK_ITEM_KEY> branch." >&2
    exit 1
    ;;
esac

git submodule foreach --recursive '
  git fetch origin dev >/dev/null 2>&1 || { echo "WARN: fetch failed for $name, skipping"; exit 0; }
  git rev-parse --verify --quiet origin/dev >/dev/null || { echo "WARN: origin/dev not found in $name, skipping"; exit 0; }
  git checkout --detach origin/dev >/dev/null 2>&1
  echo "$name -> $(git rev-parse --short HEAD)"
'

echo "--- Staging changed submodule pointers ---"
staged=0
while read -r _ path; do
  if ! git diff --quiet -- "$path"; then
    git add -- "$path"
    echo "  staged $path"
    staged=$((staged + 1))
  fi
done < <(git config -f .gitmodules --get-regexp '^submodule\..*\.path$')

if [ "$staged" -eq 0 ]; then
  echo "No submodule pointers changed."
  exit 0
fi

echo "Done ($staged pointers). Commit them with a message containing [skip ci]."
