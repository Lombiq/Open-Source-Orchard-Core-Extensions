#!/usr/bin/env bash
set -euo pipefail

script="$(cd "$(dirname "$0")/.." && pwd)/git/push-issue-branches.sh"
sandbox=$(mktemp -d)
trap 'rm -f -- "$sandbox/bin/git" "$sandbox/pushes"; rmdir -- "$sandbox/bin" "$sandbox/work/parent/child" "$sandbox/work/parent" "$sandbox/work/other" "$sandbox/work" "$sandbox"' EXIT
mkdir -p "$sandbox/bin" "$sandbox/work/parent/child" "$sandbox/work/other"

cat > "$sandbox/bin/git" <<'GIT'
#!/usr/bin/env bash
set -euo pipefail
case "$1" in
  submodule)
    printf '%s\n' 'parent' 'parent/child' 'other'
    ;;
  rev-parse)
    if [ "$PWD" = "$MOCK_ROOT/other" ]; then
      echo dev
    else
      echo issue/OSOE-123
    fi
    ;;
  push)
    if [ "$PWD" = "$MOCK_ROOT" ]; then
      echo SUPERPROJECT >> "$MOCK_LOG"
    else
      printf '%s\n' "${PWD#"$MOCK_ROOT"}" >> "$MOCK_LOG"
    fi
    if [ "${FAIL_CHILD:-false}" = true ] && [ "$PWD" = "$MOCK_ROOT/parent/child" ]; then
      exit 1
    fi
    ;;
  *)
    echo "Unexpected git command: $*" >&2
    exit 1
    ;;
esac
GIT
chmod +x "$sandbox/bin/git"
export PATH="$sandbox/bin:$PATH"
export MOCK_ROOT="$sandbox/work"
export MOCK_LOG="$sandbox/pushes"
cd "$MOCK_ROOT"

if bash "$script" > /dev/null 2>&1; then
  echo "FAIL: a missing work item key was accepted."
  exit 1
fi

bash "$script" OSOE-123 > /dev/null
expected=$(printf '%s\n' '/parent/child' '/parent' 'SUPERPROJECT')
actual=$(cat "$MOCK_LOG")
if [ "$actual" != "$expected" ]; then
  echo "FAIL: incorrect push order."
  printf 'Expected:\n%s\nActual:\n%s\n' "$expected" "$actual"
  exit 1
fi

: > "$MOCK_LOG"
export FAIL_CHILD=true
if bash "$script" OSOE-123 > /dev/null 2>&1; then
  echo "FAIL: a submodule push failure was ignored."
  exit 1
fi
if [ "$(cat "$MOCK_LOG")" != '/parent/child' ]; then
  echo "FAIL: a containing repository was pushed after its submodule failed."
  exit 1
fi

echo "PUSH_ORDER_OK"
