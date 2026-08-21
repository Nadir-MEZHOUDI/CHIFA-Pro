# Contributing to CHIFA Pro

Thanks for your interest! This guide explains how to contribute to the project.

## Ground rules

- Code must compile **without warnings** (analyzers are enabled via `Directory.Build.props`).
- `Nullable` is enabled across all projects — use it correctly.
- Follow the existing conventions: `_camelCase` private fields, PascalCase methods/properties,
  `Uc` suffix for UserControls, `ValueTask` for data-returning async methods.
- No new dependencies unless clearly justified (especially not commercial ones).
- **Never commit secrets** — connection strings, passwords, keys, or tokens.
- No comments unless the code truly needs them.

## Development setup

1. Install .NET 10 SDK and (for the client) a DevExpress license.
2. Set database parameters via environment variables:
   - `CHIFA_DB_PASSWORD` — main app DB password
   - `CHIFA_TEST_DB_HOST`, `CHIFA_TEST_DB_PORT`, `CHIFA_TEST_DB_USER`, `CHIFA_TEST_DB_PWD`,
     `CHIFA_TEST_DB_NAME` — test DB connection

## Submitting changes

1. **Fork** the repository and create a feature branch:
   ```bash
   git switch -c feat/my-awesome-feature
   ```
2. Make your changes, run the formatter:
   ```bash
   dotnet format "CHIFA Pro.sln"
   ```
3. Run the unit tests (no DB needed):
   ```bash
   dotnet test "CHIFA.Tests/CHIFA.Tests.csproj" --filter "Category!=DbFunctional&Category!=Perf"
   ```
4. Add tests for new behaviour:
   - Pure logic tests in `CHIFA.Tests` (no DB).
   - Database-backed tests with the `DbFunctional` trait (require a live DB).
   - Performance work with the `Perf` trait.
5. Commit with a concise message (Conventional Commits style):
   ```text
   feat(scope): add psychotropes audit view
   fix(scope): preserve period filter across views
   docs: update README build instructions
   ```
6. Open a **Pull Request** against `master`. Describe what changed and why.

## Code review process

- At least one maintainer approves the PR.
- CI must be green (build + unit tests).
- Keep PRs focused; large rewrites should be discussed first in an issue or discussion.

## Reporting bugs

Search existing [issues](https://github.com/Nadir-MEZHOUDI/CHIFA-Pro/issues) first.
Include steps to reproduce, expected vs actual behaviour, and environment details (OS, .NET version, DB version).

Security issues: see [SECURITY.md](SECURITY.md) — report privately, not in public issues.
