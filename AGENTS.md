# AGENTS.md

## Project structure

- `dotnet/Containers.ImageReferences/` - Build, parse, and manipulate container image references
- `dotnet/Containers.ImageReferences.Extensions.Nuke/` - NUKE build extensions

Each project may have:

- its own AGENTS.md file with more detailed guidance
- related test projects, whose name will follow the pattern `<ProjectName>.Tests`
- its own README.md file used for the NuGet package
