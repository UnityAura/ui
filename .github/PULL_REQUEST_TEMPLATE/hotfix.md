## Hotfix: <!-- short description -->

**Branch:** `hotfix/<name>` → `main`

## Problem

<!-- Describe the production issue being fixed. -->

## Fix

<!-- Describe the fix applied. -->

## Impact assessment

- Severity: <!-- critical / high / medium -->
- Affected versions: <!-- e.g. v1.2.3 -->
- Root cause: <!-- brief explanation -->

## Hotfix checklist

- [ ] Branched from `main`
- [ ] Fix is minimal and targeted — no scope creep
- [ ] `package.json` version updated (patch bump)
- [ ] `CHANGELOG.md` entry added
- [ ] CI checks pass
- [ ] Back-merge to `develop` (and `test` if applicable) is planned immediately after merge
