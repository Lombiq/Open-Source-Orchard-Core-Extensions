#!/usr/bin/env bash
# build-with-analyzers.sh
#
# Builds a solution with analyzers enabled and prints only deduplicated
# error/warning lines, so build logs never flood the conversation.
#
# Uses --warnaserror / TreatWarningsAsErrors=true by default, matching the
# superproject's actual CI (tools/Lombiq.GitHub.Actions build-dotnet action
# defaults warnings-as-errors to true). Without this, a warning can look
# harmless locally while still failing the real CI build — pass
# WARN_AS_ERROR=false only to intentionally reproduce the more lenient,
# non-default local behavior.
#
# Usage:
#   bash .agents/skills/renovate-integration/scripts/dotnet/build-with-analyzers.sh [solution-or-project]
#   WARN_AS_ERROR=false bash .../build-with-analyzers.sh [solution-or-project]
#
# Defaults to Lombiq.OSOCE.slnx in the superproject root.
#
# Exit codes:
#   0 = build succeeded, 1 = build failed (diagnostics printed above).

set -uo pipefail

TARGET="${1:-Lombiq.OSOCE.slnx}"
WARN_AS_ERROR="${WARN_AS_ERROR:-true}"
LOG=$(mktemp)
trap 'rm -f "$LOG"' EXIT

echo "--- Building $TARGET with analyzers (WARN_AS_ERROR=$WARN_AS_ERROR) ---"
dotnet build "$TARGET" \
  /property:RunAnalyzersDuringBuild=true \
  /property:GenerateFullPaths=true \
  /property:TreatWarningsAsErrors="$WARN_AS_ERROR" \
  $([ "$WARN_AS_ERROR" = "true" ] && echo --warnaserror) \
  /consoleloggerparameters:NoSummary \
  > "$LOG" 2>&1
status=$?

grep -E ' (error|warning) [A-Z]+[0-9]+' "$LOG" \
  | sed 's/ \[[^]]*\]$//' \
  | sort -u \
  | head -100

echo "--- Build exit code: $status ---"
exit $status
