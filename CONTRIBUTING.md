# Contributing to UnityAura UI

## Table of contents

1. [Branch model overview](#branch-model-overview)
2. [Long-lived branches](#long-lived-branches)
3. [Short-lived working branches](#short-lived-working-branches)
4. [Promotion flow: dev → test → prod](#promotion-flow-dev--test--prod)
5. [Hotfix flow](#hotfix-flow)
6. [Documentation branch flow](#documentation-branch-flow)
7. [PR and review policy](#pr-and-review-policy)
8. [Release conventions](#release-conventions)
9. [Branch protection configuration](#branch-protection-configuration)

---

## Branch model overview

```
docs/*  ──────────────────────────────────────────► docs
                                                      (documentation publishing)

feature/* ─┐
fix/*      ├──► develop ──► release/x.y.z ──► test ──► main ──► tag vX.Y.Z
chore/*    ┘                                             ▲
refactor/*                                               │
                                                hotfix/* (from main, back-merge to develop)
```

---

## Long-lived branches

| Branch    | Purpose                                   | Who pushes directly |
|-----------|-------------------------------------------|---------------------|
| `main`    | Production. Every commit is a release.    | Nobody (PR only)    |
| `test`    | Pre-production / UAT validation buffer.   | Nobody (PR only)    |
| `develop` | Active integration for day-to-day work.   | Nobody (PR only)    |
| `docs`    | Documentation publishing source of truth. | Nobody (PR only)    |

---

## Short-lived working branches

### Feature, fix, chore, refactor

```
git switch develop
git pull origin develop
git switch -c feature/<short-description>
# ... work ...
# Open PR: feature/<name> → develop
# Delete branch after merge
```

Naming conventions:
- `feature/<short-description>`
- `fix/<short-description>`
- `chore/<short-description>`
- `refactor/<short-description>`

### Release

```
git switch develop
git pull origin develop
git switch -c release/x.y.z
# Bump version in package.json
# Update CHANGELOG.md
# Open PR: release/x.y.z → test   (first stage)
# After test sign-off, open PR: test → main
# Tag main: git tag vX.Y.Z
# Back-merge: open PR release/x.y.z → develop
# Delete release branch after all merges
```

### Hotfix

```
git switch main
git pull origin main
git switch -c hotfix/<short-description>
# Apply minimal fix
# Bump patch version in package.json
# Update CHANGELOG.md
# Open PR: hotfix/<name> → main
# After merge, immediately open back-merge PR: hotfix/<name> → develop
# Open PR: hotfix/<name> → test  (if test is ahead of main)
# Delete hotfix branch after all merges
```

### Documentation

```
git switch docs
git pull origin docs
git switch -c docs/<short-description>
# ... update docs ...
# Open PR: docs/<name> → docs
# Delete branch after merge
```

---

## Promotion flow: dev → test → prod

```
1. All feature/fix work lands in `develop` via PR.

2. At release cutoff, create `release/x.y.z` from `develop`:
   git switch -c release/x.y.z develop

3. On the release branch:
   - Bump version in package.json to x.y.z
   - Write CHANGELOG.md entry
   - Verify no unfinished work is included

4. Open PR: release/x.y.z → test
   - Use the release PR template
   - Requires approvals + CI pass

5. Validate on `test`:
   - Perform any manual / integration testing
   - Obtain sign-off

6. Open PR: test → main
   - Use the release PR template
   - Requires approvals + CI pass

7. After merge to `main`, tag the release:
   git switch main && git pull origin main
   git tag vX.Y.Z
   git push origin vX.Y.Z
   (This triggers the Package Zip workflow)

8. Back-merge to `develop`:
   # Open PR: main → develop (use the default PR template)
   # Requires approvals + CI pass, same as any other PR to develop
```

---

## Hotfix flow

Only use this path for critical production issues that cannot wait for the normal release cycle.

```
hotfix/* ──► main ──► tag vX.Y.Z (patch)
              └──► develop  (immediate back-merge)
              └──► test     (if test branch has diverged)
```

The hotfix branch bypasses `develop` and `test` **for speed only**. The back-merge to `develop` (and optionally `test`) must happen immediately after the production merge to prevent drift.

---

## Documentation branch flow

The `docs` branch is the single source of truth for published documentation.

- All doc changes go through `docs/*` → `docs` PRs.
- Code PRs that change user-facing behaviour must be paired with a `docs/*` PR before the next release is promoted to `main`.
- The release checklist item "Documentation is up to date" should reference the relevant merged `docs/*` PR.

---

## PR and review policy

- All contributions must come through Pull Requests — no direct pushes to `main`, `test`, `develop`, or `docs`.
- PRs from **non-owners** require at least **3 unique approvals** (enforced by the `PR Approval Gate` workflow).
- Repository owner PRs are exempt from the 3-approval minimum.
- PRs targeting `main` or `test` must use the appropriate named PR template and complete all checklist items before merge.
- Draft PRs are excluded from the approval gate and can be used for early feedback.

### Selecting a PR template

GitHub will use the default template (`.github/pull_request_template.md`) automatically.
To use a named template, append `?template=<name>.md` to the PR creation URL, e.g.:

- Feature: `?template=feature.md`
- Release: `?template=release.md`
- Hotfix: `?template=hotfix.md`
- Docs: `?template=docs.md`

---

## Release conventions

- **Semantic versioning**: `MAJOR.MINOR.PATCH` — all releases are tagged `vX.Y.Z` on `main`.
- **CHANGELOG.md**: every release and hotfix must add an entry under its version heading.
- **Tag format**: `vX.Y.Z` triggers the `Package Zip` workflow, producing a distributable ZIP artifact.
- **No force-pushing** protected branches.

### Versioning guide

| Change type                  | Bump   | Example         |
|------------------------------|--------|-----------------|
| Breaking API/UX change       | MAJOR  | `1.0.0 → 2.0.0` |
| New backwards-compatible feature | MINOR | `1.0.0 → 1.1.0` |
| Bug fix / patch              | PATCH  | `1.0.0 → 1.0.1` |

---

## Branch protection configuration

The following rules **must be applied in GitHub repository settings** (`Settings → Branches`) to enforce this model. They cannot be set via files alone.

### `main`

| Setting                                     | Value           |
|---------------------------------------------|-----------------|
| Require a pull request before merging       | ✅ Enabled       |
| Required approving reviews (non-owner PRs)  | 3               |
| Require status checks to pass              | ✅ CI (`validate-package`), `PR Approval Gate` (`require-approvals`) |
| Require branches to be up to date          | ✅ Enabled       |
| Restrict who can push                      | Admins / owners only |
| Allow force pushes                         | ❌ Disabled      |
| Allow deletions                            | ❌ Disabled      |

### `test`

| Setting                                     | Value           |
|---------------------------------------------|-----------------|
| Require a pull request before merging       | ✅ Enabled       |
| Required approving reviews (non-owner PRs)  | 3               |
| Require status checks to pass              | ✅ CI (`validate-package`), `PR Approval Gate` (`require-approvals`) |
| Require branches to be up to date          | ✅ Enabled       |
| Allow force pushes                         | ❌ Disabled      |
| Allow deletions                            | ❌ Disabled      |

### `develop`

| Setting                                     | Value           |
|---------------------------------------------|-----------------|
| Require a pull request before merging       | ✅ Enabled       |
| Required approving reviews (non-owner PRs)  | 3               |
| Require status checks to pass              | ✅ CI (`validate-package`), `PR Approval Gate` (`require-approvals`) |
| Allow force pushes                         | ❌ Disabled      |
| Allow deletions                            | ❌ Disabled      |

### `docs`

| Setting                                     | Value           |
|---------------------------------------------|-----------------|
| Require a pull request before merging       | ✅ Enabled       |
| Required approving reviews (non-owner PRs)  | 3               |
| Require status checks to pass              | ✅ `PR Approval Gate` (`require-approvals`) |
| Allow force pushes                         | ❌ Disabled      |
| Allow deletions                            | ❌ Disabled      |

### Creating the long-lived branches

Run these commands once to bootstrap the branch structure:

```bash
# From a clean, up-to-date clone of main
git switch main
git pull origin main

git switch -c develop && git push origin develop
git switch main

git switch -c test && git push origin test
git switch main

git switch -c docs && git push origin docs
git switch main
```

Then apply branch protection rules in GitHub settings as described above.
