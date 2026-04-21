# UnityAura UI Package

This repository is managed as a Unity Package (UPM).

## Branch model

| Branch      | Purpose                                        |
|-------------|------------------------------------------------|
| `main`      | Production — every commit here is a release.   |
| `test`      | Pre-production / UAT validation buffer.        |
| `develop`   | Active integration for day-to-day development. |
| `docs`      | Documentation publishing source of truth.      |

Short-lived branches follow the pattern:
`feature/*`, `fix/*`, `release/*`, `hotfix/*`, `docs/*`, `chore/*`, `refactor/*`

See [CONTRIBUTING.md](CONTRIBUTING.md) for the full branch lifecycle, promotion flow, and PR templates.

## Contribution and PR policy

- All contributions must come through Pull Requests — no direct pushes to protected branches.
- PRs from non-owners must have at least **3 unique approvals** before merge.
- Repository owner PRs are exempt from the 3-approval minimum.
- The `PR Approval Gate` workflow enforces this policy and must be a required status check in branch protection rules.

## Release packaging

- The `Package Zip` workflow creates a clean ZIP artifact that contains a single `ui/` package folder.
- Triggered automatically on version tags (`vX.Y.Z`) pushed to `main`, or manually via `workflow_dispatch`.
- This ZIP is the distributable UPM package artifact.
- See [CHANGELOG.md](CHANGELOG.md) for version history.

## CI

- The `CI` workflow validates `package.json` structure and version format on all PRs targeting `main`, `test`, and `develop`.

## Unity compatibility

- Package metadata is configured for **Unity v6+** (`6000.0` and higher).

## Discord community

Join our Discord community: https://discord.gg/nZUpmVXS4E
