# Contributing

## Development Setup
- Install .NET LTS (see `global.json`).
- Restore + build: `dotnet restore && dotnet build`.
- Format check: `dotnet format --verify-no-changes`.

## Branch & Commits
- Trunk-based: feature branches `feat/<slug>`, fixes `fix/<slug>`.
- Conventional Commits (`feat:`, `fix:`, `docs:`, `chore:`, `refactor:`, `test:`).

## PRs
- Include: Summary, Why, How tested, Screenshots (if UI), Breaking changes.
- CI must pass.

## Code Style
- Enforced via `.editorconfig` and analyzers (warnings→errors in CI).
