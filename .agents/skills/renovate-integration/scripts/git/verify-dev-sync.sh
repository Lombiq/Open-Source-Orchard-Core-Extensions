#!/usr/bin/env bash
# verify-dev-sync.sh
#
# Pre-merge safety check (Phase 5): confirms that the local `dev` branch matches
# `origin/dev` in the superproject and every submodule. Reports OK / DRIFT /
# NO LOCAL DEV per repository and exits non-zero if any repository has drifted.
#
# Usage:
#   bash .agents/skills/renovate-integration/scripts/git/verify-dev-sync.sh
#
# Safety:
#   - Read-only apart from fetching; never checks out, resets, commits or pushes.

set -euo pipefail

check_repo() {
  local label="$1"
  git fetch --prune >/dev/null 2>&1 || echo "  WARN $label: fetch failed"

  if ! git rev-parse --verify --quiet refs/heads/dev >/dev/null; then
    echo "  NO LOCAL DEV $label"
    return 0
  fi

  local local_dev remote_dev
  local_dev=$(git rev-parse refs/heads/dev)
  remote_dev=$(git rev-parse refs/remotes/origin/dev 2>/dev/null || echo "")

  if [ -z "$remote_dev" ]; then
    echo "  WARN $label: origin/dev not found"
    return 0
  fi

  if [ "$local_dev" = "$remote_dev" ]; then
    echo "  OK $label ($(printf '%.7s' "$local_dev"))"
  else
    echo "  DRIFT $label: local $(printf '%.7s' "$local_dev") != origin $(printf '%.7s' "$remote_dev")"
    echo "drift" >> "$DRIFT_FILE"
  fi
}

DRIFT_FILE=$(mktemp)
export DRIFT_FILE
trap 'rm -f "$DRIFT_FILE"' EXIT

check_repo "SUPERPROJECT"

git submodule foreach --quiet --recursive '
  if ! git rev-parse --verify --quiet refs/heads/dev >/dev/null; then
    echo "  NO LOCAL DEV $name"
    exit 0
  fi

  git fetch --prune >/dev/null 2>&1 || echo "  WARN $name: fetch failed"

  local_dev=$(git rev-parse refs/heads/dev)
  remote_dev=$(git rev-parse refs/remotes/origin/dev 2>/dev/null || echo "")

  if [ -z "$remote_dev" ]; then
    echo "  WARN $name: origin/dev not found"
    exit 0
  fi

  if [ "$local_dev" = "$remote_dev" ]; then
    echo "  OK $name"
  else
    echo "  DRIFT $name: local $local_dev != origin $remote_dev"
    echo "drift" >> "$DRIFT_FILE"
  fi
'

if [ -s "$DRIFT_FILE" ]; then
  echo "RESULT: DRIFT DETECTED — stop and investigate before merging."
  exit 1
fi

echo "RESULT: all local dev branches match origin/dev."
