#!/usr/bin/env bash
# Parse-check the embedded shell body of analyze-renovate-prs.sh.
set -euo pipefail
dir="$(cd "$(dirname "$0")/.." && pwd)"
awk "/^analyze_repo_body='/,/^'\$/" "$dir/git/analyze-renovate-prs.sh" > /tmp/renovate-body.sh
# shellcheck disable=SC1091
. /tmp/renovate-body.sh
printf '%s' "label=x; $analyze_repo_body" | bash -n && echo BODY_OK

awk "/^git submodule foreach --recursive '\$/,/^'\$/" "$dir/git/checkout-latest-renovate-pr.sh" \
  | sed "1d;\$d" | bash -n && echo CHECKOUT_BODY_OK
