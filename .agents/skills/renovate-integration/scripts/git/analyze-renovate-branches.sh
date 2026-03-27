#!/usr/bin/env bash
# analyze-renovate-branches.sh
#
# Read-only analysis of all renovate/* branches across the superproject
# and every submodule. For each branch it reports:
#   - Whether it is ELIGIBLE or SKIPPED (too old / already merged).
#   - A diff --stat against origin/dev.
#   - A commit log against origin/dev.
#
# This complements checkout-latest-renovate.sh (which selects only one branch
# per submodule and checks it out). Use this script during Phase 1 to discover
# ALL eligible branches before deciding which ones to integrate.
#
# Usage:
#   bash .agents/skills/renovate-integration/scripts/git/analyze-renovate-branches.sh
#
# Safety:
#   - This script does NOT modify any branches.
#   - This script does NOT commit or push anything.
#   - It only reads remote refs that are already fetched locally.

set -euo pipefail

MAX_AGE_DAYS="${MAX_AGE_DAYS:-5}"

analyze_repo() {
  local label="$1"
  local refs
  refs=$(git for-each-ref --sort=-committerdate --format="%(refname:short)" \
    refs/remotes/origin/renovate/ 2>/dev/null || true)

  if [ -z "$refs" ]; then
    return
  fi

  echo "=========================================="
  echo "$label"
  echo "=========================================="

  local cutoff
  cutoff=$(date -d "${MAX_AGE_DAYS} days ago" +%s 2>/dev/null \
    || date -v-"${MAX_AGE_DAYS}"d +%s)

  for ref in $refs; do
    local ref_ts
    ref_ts=$(git show -s --format=%ct "$ref" 2>/dev/null || true)
    if [ -z "$ref_ts" ]; then
      echo "  SKIP $ref: cannot read commit"
      continue
    fi

    local ref_date
    ref_date=$(git show -s --format=%ci "$ref" 2>/dev/null)

    if [ "$ref_ts" -lt "$cutoff" ]; then
      echo "  SKIP $ref ($ref_date): older than ${MAX_AGE_DAYS} days"
      continue
    fi

    if git merge-base --is-ancestor "$ref" origin/dev 2>/dev/null; then
      echo "  SKIP $ref ($ref_date): already merged into origin/dev"
      continue
    fi

    echo "  ELIGIBLE $ref ($ref_date)"
    echo "  --- Diff against origin/dev ---"
    git diff --stat "origin/dev...$ref" 2>/dev/null | head -30
    echo "  --- Commit log ---"
    git log --oneline "origin/dev..$ref" 2>/dev/null | head -10
    echo ""
  done
}

# Superproject
analyze_repo "SUPERPROJECT"
echo ""

# Submodules
git submodule foreach --recursive '
  refs=$(git for-each-ref --sort=-committerdate --format="%(refname:short)" \
    refs/remotes/origin/renovate/ 2>/dev/null || true)

  if [ -z "$refs" ]; then
    exit 0
  fi

  echo "=========================================="
  echo "$name"
  echo "=========================================="

  cutoff=$(date -d "'"${MAX_AGE_DAYS}"' days ago" +%s 2>/dev/null \
    || date -v-'"${MAX_AGE_DAYS}"'d +%s)

  for ref in $refs; do
    ref_ts=$(git show -s --format=%ct "$ref" 2>/dev/null || true)
    if [ -z "$ref_ts" ]; then
      echo "  SKIP $ref: cannot read commit"
      continue
    fi

    ref_date=$(git show -s --format=%ci "$ref" 2>/dev/null)

    if [ "$ref_ts" -lt "$cutoff" ]; then
      echo "  SKIP $ref ($ref_date): older than '"${MAX_AGE_DAYS}"' days"
      continue
    fi

    if git merge-base --is-ancestor "$ref" origin/dev 2>/dev/null; then
      echo "  SKIP $ref ($ref_date): already merged into origin/dev"
      continue
    fi

    echo "  ELIGIBLE $ref ($ref_date)"
    echo "  --- Diff against origin/dev ---"
    git diff --stat "origin/dev...$ref" 2>/dev/null | head -30
    echo "  --- Commit log ---"
    git log --oneline "origin/dev..$ref" 2>/dev/null | head -10
    echo ""
  done
'
