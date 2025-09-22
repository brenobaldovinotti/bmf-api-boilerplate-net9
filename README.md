# <PROJECT NAME>

Short one-liner: what this project does.

## Tech Stack
- .NET (LTS), C#
- Clean Architecture (Domain, Application, Infrastructure, API)
- GitHub Actions, CodeQL, Dependabot

## Getting Started
```bash
dotnet --info
dotnet restore
dotnet build
dotnet test
```

## Run Locally
```bash
cd src/<Your.Api>
dotnet run
```

## Tests & Coverage
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## CI/CD
- Pull Requests and `main` are built and tested automatically.
- Code scanning via CodeQL.
- Releases are created on tags matching `v*` (see **Versioning**).

## Versioning
Semantic Versioning: `vMAJOR.MINOR.PATCH`  
Pre-release tags: `-rc.N`, `-beta.N`

## Contributing
See [CONTRIBUTING.md](CONTRIBUTING.md).

## Security
See [SECURITY.md](SECURITY.md).

## License
See [LICENSE](LICENSE).

## Change Log
See [CHANGELOG](CHANGELOG.md).
