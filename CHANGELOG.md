# Changelog
All notable changes to this project will be documented here.

## [Unreleased] - 2025-09-22
### Summary
<!-- What changed and why? -->
- Scaffold **Bmf.ApiBoilerplate** solution with layered projects:
  - `src`: API, Application, Domain, Infrastructure
  - `tests`: Domain.Tests, Application.Tests, Infrastructure.Tests, API.Tests, Architecture.Tests
- Enforce dependencies via project refs (**API → Application → Domain**, **Infrastructure → (Domain, Application)**).
- Add **NuGet Central Package Management** via `Directory.Packages.props` (EFCore, Npgsql, FluentValidation, Serilog, OTel, Swagger, Versioning, Testcontainers, NetArchTest, analyzers, etc.).
- Add `Directory.Build.props`:
  - `net9.0`, C# 12, `Nullable=enable`, `TreatWarningsAsErrors=true`
  - `AnalysisLevel=9.0` (avoid VS analyzer version bug)
  - inbox analyzers + StyleCop wired
  - XML docs enabled (`GenerateDocumentationFile=true`, suppress `CS1591`)
- Add tuned `.editorconfig`:
  - LF endings, final newline, 4-space indents
  - private fields `_camelCase`, async suffix guidance, naming/style baselines
  - doc rule severities (valid XML & param/return emphasized)
- Add developer docs: `docs/Documentation-Standards.md` (XML doc standards).
- Add repo hygiene: `.gitignore`, `global.json` (9.0.100), `README.md`.
- Minimal API `Program.cs` and placeholder tests to keep build green.

### How was it tested?
<!-- Steps / commands / screenshots -->
- Clean build:
  ```powershell
  Get-ChildItem -Recurse -Include bin,obj | Remove-Item -Recurse -Force
  dotnet restore
  dotnet build -warnaserror
  dotnet test
  ```
- Verified Visual Studio loads projects without analyzer error (using `AnalysisLevel=9.0`).
- Confirmed XML documentation files emitted under `bin/**/Bmf.ApiBoilerplate.*.xml`.

### Checklist
- [ ] CI green
- [x] Docs updated (if needed)
- [x] No breaking changes (or documented)

## [Unreleased] - 2025-09-22
### PR: STEP-02 — Domain Kernel & Shared Types

**feat: domain kernel primitives + tests**  
**refactor: rename solution/projects/namespaces to `Bmf.Api.Boilerplate`**  
**test: add IClock/SystemClock tests**

### Summary
<!-- What changed and why? -->
- **Domain Kernel (no external refs):**
  - `Abstractions/Result.cs`: `Result` and `Result<T>` with canonical `Error` (codes/messages) for clean use-case outcomes.
  - `Primitives/StronglyTypedId.cs`: Guid-backed base for strongly-typed IDs via record classes (simple, testable).
  - `Primitives/Entity.cs`, `AggregateRoot.cs`, `IDomainEvent.cs`: light DDD primitives with event queue + clear semantics.
  - `Time/IClock.cs`, `Time/SystemClock.cs`: time abstraction for deterministic testing.
- **Unit Tests (green):**
  - Domain tests for `Result`, strongly-typed ID equality/parse, aggregate events (raise/clear).
  - **New:** `ClockTests` for `IClock`/`SystemClock` (progression, UTC semantics, fake clock sample).
  - Architecture test to enforce **inward dependencies** (Domain has no deps on API/Application/Infrastructure).
- **Renames for clarity & consistency:**
  - Solution and projects renamed **`Bmf.ApiBoilerplate.*` → `Bmf.Api.Boilerplate.*`**.
  - API project name standardized to **`Bmf.Api.Boilerplate`** (no redundant `.API`).
  - Namespaces/usings, project references, and solution entries updated.
- **Tooling/Packages (Central Package Management):**
  - Pinned test/runtime packages: `Microsoft.NET.Test.Sdk 17.11.1`, `xunit 2.9.3`, `xunit.runner.visualstudio 3.1.4`, `FluentAssertions 8.6.0`, `coverlet.collector 6.0.4`, `NetArchTest.Rules 1.3.8`, `Testcontainers.PostgreSql 4.7.0`, `BouncyCastle.Cryptography 2.4.0`.
  - Fixed malformed XML in `Directory.Packages.props` and ensured clean, deterministic restores.

### How was it tested?
<!-- Steps / commands / screenshots -->
- **Clean restore/build/test (CLI):**
  ```powershell
  # from repo root
  dotnet nuget locals all --clear
  Get-ChildItem -Recurse -Include bin,obj | Remove-Item -Recurse -Force
  Get-ChildItem -Recurse -Filter packages.lock.json | Remove-Item -Force

  dotnet restore --force-evaluate --no-cache -v minimal
  dotnet build -warnaserror
  dotnet test -v minimal
  ```
- **Sanity checks:**
  - Confirmed package pins via CPM:
    ```powershell
    dotnet list tests\Bmf.Api.Boilerplate.Domain.Tests package --include-transitive
    dotnet list tests\Bmf.Api.Boilerplate.Infrastructure.Tests package --include-transitive
    ```
  - **Architecture Tests:** `Domain_should_not_depend_on_other_layers` passes (null-safe failure message handling).
  - **Clock tests:** `SystemClock.UtcNow` bounds + forward progression + `FakeClock` demonstration.
- **IDE/VS validation:**
  - Visual Studio 2022 (17.11+) loads solution and discovers tests.
  - Removed legacy VSIX test adapters (use NuGet adapters only).

### Checklist
- [ ] CI green
- [x] Docs updated (not required for this step; XML doc standards already in repo)
- [x] No breaking changes (internal refactor/rename only)

## [0.1.0] - YYYY-MM-DD

