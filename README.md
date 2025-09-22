# BMF Api Boilerplate

Production-ready .NET API boilerplate (Clean Architecture, light DDD, selective CQRS, Use-Case handlers).

## Tech Stack
- .NET 9 / C# 12
- NuGet Central Package Management
- Nullable enabled, warnings as errors
- Layered: **API → Application → Domain**; **Infrastructure → (Domain, Application)**

## Getting Started
```bash
dotnet --info
dotnet restore
dotnet build
dotnet test
```

## Run Locally
```bash
cd src/Bmf.ApiBoilerplate.API
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
