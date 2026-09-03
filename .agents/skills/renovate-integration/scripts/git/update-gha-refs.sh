#!/usr/bin/env bash
# update-gha-refs.sh
#
# Applies or reverts the temporary Lombiq/GitHub-Actions refs used while an
# issue branch is in flight (Phase 3 applies, Phase 5 reverts).
#
# Only refs preceded by "Lombiq/GitHub-Actions/" are touched — refs to other
# repositories (e.g. Lombiq/PowerShell-Analyzers) are left alone, since they
# have no issue/<WORK_ITEM_KEY> branch.
#
# Usage:
#   bash .agents/skills/renovate-integration/scripts/git/update-gha-refs.sh apply OSOE-123
#   bash .agents/skills/renovate-integration/scripts/git/update-gha-refs.sh revert OSOE-123
#
# Files covered:
#   - tools/Lombiq.GitHub.Actions/.github/**/*.yml (actions/ and workflows/)
#   - .github/workflows/*.yml (superproject)
#
# Safety:
#   - This script does NOT commit or push anything.
#   - Must be run from the superproject root.

set -euo pipefail

MODE="${1:-}"
KEY="${2:-}"

if [ "$MODE" != "apply" ] && [ "$MODE" != "revert" ]; then
  echo "ERROR: first argument must be 'apply' or 'revert'." >&2
  exit 1
fi

if [ -z "$KEY" ]; then
  echo "ERROR: second argument must be the work item key (e.g. OSOE-123)." >&2
  exit 1
fi

if [ ! -d .git ] && [ ! -f .git ]; then
  echo "ERROR: run this script from the superproject root." >&2
  exit 1
fi

issue_ref="issue/$KEY"

if [ "$MODE" = "apply" ]; then
  from="dev"
  to="$issue_ref"
else
  from="$issue_ref"
  to="dev"
fi

# Escape "/" for the sed replacement (branch names contain slashes).
from_escaped=$(printf '%s' "$from" | sed 's/[\/&]/\\&/g')
to_escaped=$(printf '%s' "$to" | sed 's/[\/&]/\\&/g')

changed=0

process_dir() {
  local dir="$1"
  [ -d "$dir" ] || return 0

  while IFS= read -r file; do
    if grep -q "Lombiq/GitHub-Actions/[^@]*@$from" "$file"; then
      sed -i "s|\(Lombiq/GitHub-Actions/[^@[:space:]]*\)@$from_escaped\b|\1@$to_escaped|g" "$file"
      echo "  updated $file"
      changed=$((changed + 1))
    fi
  done < <(find "$dir" -type f \( -name '*.yml' -o -name '*.yaml' \))
}

echo "--- $MODE: Lombiq/GitHub-Actions @$from -> @$to ---"
process_dir "tools/Lombiq.GitHub.Actions/.github"
process_dir ".github/workflows"

echo "Files changed: $changed"

if [ "$changed" -eq 0 ]; then
  echo "WARN: no refs matched @$from — verify whether this step was already done."
fi
