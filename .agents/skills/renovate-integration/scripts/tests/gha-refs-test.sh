#!/usr/bin/env bash
# gha-refs-test.sh
#
# Sandbox test for update-gha-refs.sh: verifies that only Lombiq/GitHub-Actions
# refs are rewritten, and that apply + revert round-trips exactly.
#
# Usage:
#   bash .agents/skills/renovate-integration/scripts/tests/gha-refs-test.sh

set -euo pipefail

script="$(cd "$(dirname "$0")/.." && pwd)/git/update-gha-refs.sh"
sandbox=$(mktemp -d)
trap 'rm -rf "$sandbox"' EXIT

mkdir -p "$sandbox/tools/Lombiq.GitHub.Actions/.github/actions/foo" "$sandbox/.github/workflows"
touch "$sandbox/.git"

cat > "$sandbox/tools/Lombiq.GitHub.Actions/.github/actions/foo/action.yml" <<'YAML'
runs:
  steps:
    - uses: Lombiq/GitHub-Actions/.github/actions/setup@dev
    - uses: Lombiq/PowerShell-Analyzers/.github/actions/lint@dev
    - uses: actions/checkout@dev
YAML

cat > "$sandbox/.github/workflows/build.yml" <<'YAML'
jobs:
  build:
    uses: Lombiq/GitHub-Actions/.github/workflows/build-and-test.yml@dev
YAML

before=$(cat "$sandbox/tools/Lombiq.GitHub.Actions/.github/actions/foo/action.yml" "$sandbox/.github/workflows/build.yml")

cd "$sandbox"
bash "$script" apply OSOE-123 > /dev/null

applied=$(cat tools/Lombiq.GitHub.Actions/.github/actions/foo/action.yml .github/workflows/build.yml)

if printf '%s' "$applied" | grep -q 'PowerShell-Analyzers/.github/actions/lint@issue'; then
  echo "FAIL: rewrote a non-Lombiq/GitHub-Actions ref."
  exit 1
fi

if printf '%s' "$applied" | grep -q 'actions/checkout@issue'; then
  echo "FAIL: rewrote actions/checkout."
  exit 1
fi

if [ "$(printf '%s' "$applied" | grep -c 'GitHub-Actions/[^@]*@issue/OSOE-123')" -ne 2 ]; then
  echo "FAIL: expected 2 rewritten Lombiq/GitHub-Actions refs."
  printf '%s\n' "$applied"
  exit 1
fi

bash "$script" revert OSOE-123 > /dev/null
after=$(cat tools/Lombiq.GitHub.Actions/.github/actions/foo/action.yml .github/workflows/build.yml)

if [ "$before" != "$after" ]; then
  echo "FAIL: revert did not restore the original content."
  diff <(printf '%s\n' "$before") <(printf '%s\n' "$after") || true
  exit 1
fi

echo "GHA_REFS_OK"
