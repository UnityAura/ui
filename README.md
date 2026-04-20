# UnityAura UI Package

This repository is managed as a Unity Package (UPM).

## Contribution and PR policy

- All contributions must come through Pull Requests.
- PRs from non-owners must have at least **3 unique approvals** before merge.
- Repository owner PRs are exempt from the 3-approval minimum.
- The `PR Approval Gate` workflow enforces this policy and should be required in branch protection rules.

## Release packaging

- The `Package Zip` workflow creates a clean ZIP artifact that contains a single `ui/` package folder.
- Triggered on version tags (`v*`) or manually (`workflow_dispatch`).
- This ZIP can be used for version publishing.

## Unity compatibility

- Package metadata is configured for **Unity v6+** (`6000.0` and higher).
