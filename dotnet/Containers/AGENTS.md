## Project overview

A .NET library for strongly-typed, validated container image references (Docker/OCI).
Parses, builds, and manipulates image references like `docker.io/library/nginx:1.25`.

## Architecture

### Technology stack

- **Platform**: .NET 10, C# 14, NuGet CPM
- **Build system**: [NUKE](https://nuke.build/)
- **Packaging**: NuGet
- **Testing**: TUnit
- **Continuous Integration**: GitHub Actions

### Project structure

```
dotnet/
├── Containers/                       # Main library project for image reference parsing and manipulation
├── Containers.Extensions.Nuke/       # Use image references seamlessly with NUKE
├─ <ProjectName>.Tests/               # Tests related to a project
... (more)
```

## Development guidelines

### C# coding

- Nullable reference types enabled (`<Nullable>enable</Nullable>`)
- Null-forgiving operator (`!`) should be avoided and if used, should be justified with a comment
- If a csproj file has `<IsPackable>true</IsPackable>`, then it must also have:
    - `<PackageTags></PackageTags>`, `<Description></Description>` and a README.md included
- Image reference component's string representation should be lowercase. You may suppress CA1308 to achieve this.

### Architecture principles

- Immutable value types for image reference components (registry, repository, tag, digest, etc.)
- No external dependencies for core functionality (keep it lean)
- Avoid string concatenation for building references
- Follow OCI standards but allow for widely used Docker-specific conventions
- Stable public API surface. You must warn me if you change it.

### Error handling

- Specific exception types: `InvalidTagException`, etc.
- Validate inputs early, fail fast with clear error messages
- Methods must document exceptions that can be thrown

### Testing requirements

- Unit tests for all parsers (valid and invalid inputs)
- Test edge cases: max lengths, special characters, case sensitivity
- Test OCI spec examples from official documentation
- Test Docker-specific conventions
- Round-trip tests: Parse(ToString(x)) == x
- Coverage target: 98%+
- Follow AAA pattern (Arrange, Act, Assert)
- One assertion focus per test method, although multiple assertions are allowed

### Documentation

- Keep usage examples up to date in project README.md
- XML docs on all public APIs with `<summary>`, `<param>`, `<returns>`, `<exception>` being required.
- Include `<example>` tags showing valid/invalid inputs where appropriate.
- Reference OCI spec sections where applicable
- Document format specifications clearly
- Keep documentation up to date with code changes
- Use Markdown for README and other documentation files