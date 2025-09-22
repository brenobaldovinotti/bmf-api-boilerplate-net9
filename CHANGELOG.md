# Changelog
All notable changes to this project will be documented here.

## [Unreleased]

## [0.1.0] - 2025-09-22
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
