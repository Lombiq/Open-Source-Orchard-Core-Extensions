#!/usr/bin/env bash
# push-issue-branches.sh
#
# Pushes issue/<WORK_ITEM_KEY> branches in the superproject and every submodule
# that has one checked out, enforcing the skill's push safety rules:
#   - Only pushes when HEAD is exactly issue/<WORK_ITEM_KEY>.
#   - Never pushes dev, renovate/* or a detached HEAD.
#   - Never pushes to any ref other than the same-named branch on origin.
#
# Repositories where a Renovate PR head branch is checked out (no issue branch)
# are reported as skipped — their existing Renovate PR is used instead.
#
# Usage:
#   bash .agents/skills/renovate-integration/scripts/git/push-issue-branches.sh OSOE-123
#
# Safety:
#   - Refuses to run without an explicit work item key.
#   - Does NOT create branches, commit, merge or force-push.

set -euo pipefail

KEY="${1:-}"

if [ -z "$KEY" ]; then
  echo "ERROR: pass the work item key (e.g. OSOE-123)." >&2
  exit 1
fi

export ISSUE_BRANCH="issue/$KEY"

push_current() {
  local label="$1"
  local branch
  branch=$(git rev-parse --abbrev-ref HEAD 2>/dev/null || echo "")

  if [ "$branch" != "$ISSUE_BRANCH" ]; then
    echo "SKIP $label: on '$branch', not '$ISSUE_BRANCH'."
    return 0
  fi

  echo "PUSH $label: $ISSUE_BRANCH"
  git push -u origin "$ISSUE_BRANCH"
}

# Superproject.
push_current "SUPERPROJECT"

# Submodules ($name is provided by git submodule foreach).
git submodule foreach --recursive '
  branch=$(git rev-parse --abbrev-ref HEAD 2>/dev/null || echo "")
  if [ "$branch" != "$ISSUE_BRANCH" ]; then
    echo "SKIP $name: on \"$branch\", not \"$ISSUE_BRANCH\"."
    exit 0
  fi
  echo "PUSH $name: $ISSUE_BRANCH"
  git push -u origin "$ISSUE_BRANCH"
'
