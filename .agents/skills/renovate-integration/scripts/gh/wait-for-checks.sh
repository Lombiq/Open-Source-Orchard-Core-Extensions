#!/usr/bin/env bash
# wait-for-checks.sh
#
# Polls a pull request's checks until none are pending, then prints a compact
# summary and exits 0 (all passing) or 1 (something failed).
#
# Deliberately avoids `gh pr checks --watch` / `gh run watch`: both open a
# full-screen TUI that never returns control in a non-interactive terminal.
# Polling backs off from 60s to 300s to keep the number of round-trips low.
#
# Usage:
#   bash .agents/skills/renovate-integration/scripts/gh/wait-for-checks.sh <owner/repo> <pr-number> [timeout-minutes]
#
# Requirements:
#   - gh (GitHub CLI), authenticated.
#
# Safety:
#   - Read-only: only queries check state, never merges, pushes or labels.

set -euo pipefail

REPO="${1:-}"
PR="${2:-}"
TIMEOUT_MINUTES="${3:-90}"

if [ -z "$REPO" ] || [ -z "$PR" ]; then
  echo "ERROR: usage: wait-for-checks.sh <owner/repo> <pr-number> [timeout-minutes]" >&2
  exit 2
fi

if ! command -v gh >/dev/null 2>&1; then
  echo "ERROR: gh (GitHub CLI) is required but not installed." >&2
  exit 2
fi

deadline=$(( $(date +%s) + TIMEOUT_MINUTES * 60 ))
interval=60

while :; do
  checks=$(gh pr checks "$PR" --repo "$REPO" --json name,state \
    --jq '.[] | [.state, .name] | @tsv' 2>/dev/null || true)

  if [ -z "$checks" ]; then
    echo "No checks reported yet for $REPO#$PR."
  else
    pending=$(printf "%s\n" "$checks" | grep -c -E '^(PENDING|QUEUED|IN_PROGRESS|EXPECTED|WAITING)' || true)
    failed=$(printf "%s\n" "$checks" | grep -c -E '^(FAILURE|ERROR|TIMED_OUT|CANCELLED|ACTION_REQUIRED|STARTUP_FAILURE)' || true)
    total=$(printf "%s\n" "$checks" | wc -l | tr -d ' ')

    echo "$REPO#$PR: $total checks, $pending pending, $failed failed"

    if [ "$pending" -eq 0 ]; then
      if [ "$failed" -gt 0 ]; then
        echo "--- Failing checks ---"
        printf "%s\n" "$checks" | grep -E '^(FAILURE|ERROR|TIMED_OUT|CANCELLED|ACTION_REQUIRED|STARTUP_FAILURE)'
        exit 1
      fi
      echo "All checks passed for $REPO#$PR."
      exit 0
    fi
  fi

  if [ "$(date +%s)" -ge "$deadline" ]; then
    echo "TIMEOUT after $TIMEOUT_MINUTES minutes; checks still pending for $REPO#$PR."
    exit 3
  fi

  sleep "$interval"
  # Back off: 60s -> 120s -> 240s -> capped at 300s.
  interval=$(( interval * 2 ))
  [ "$interval" -gt 300 ] && interval=300
done
