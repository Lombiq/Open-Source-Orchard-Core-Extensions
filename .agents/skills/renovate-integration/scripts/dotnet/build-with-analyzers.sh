#!/usr/bin/env bash
# build-with-analyzers.sh
#
# Builds a solution with analyzers enabled and prints only deduplicated
# error/warning lines, so build logs never flood the conversation.
#
# Usage:
#   bash .agents/skills/renovate-integration/scripts/dotnet/build-with-analyzers.sh [solution-or-project]
#
# Defaults to Lombiq.OSOCE.slnx in the superproject root.
#
# Exit codes:
#   0 = build succeeded, 1 = build failed (diagnostics printed above).

set -uo pipefail

TARGET="${1:-Lombiq.OSOCE.slnx}"
LOG=$(mktemp)
trap 'rm -f "$LOG"' EXIT

echo "--- Building $TARGET with analyzers ---"
dotnet build "$TARGET" \
  /property:RunAnalyzersDuringBuild=true \
  /property:GenerateFullPaths=true \
  /consoleloggerparameters:NoSummary \
  > "$LOG" 2>&1
status=$?

grep -E ' (error|warning) [A-Z]+[0-9]+' "$LOG" \
  | sed 's/ \[[^]]*\]$//' \
  | sort -u \
  | head -100

echo "--- Build exit code: $status ---"
exit $status
