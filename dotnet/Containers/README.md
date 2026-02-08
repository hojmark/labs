# HLabs.Containers

Strongly-typed, validated container image references for .NET.

Build, parse, and manipulate Docker/OCI image references (like `docker.io/library/nginx:1.25`).

---

## Installation

Install via NuGet:

```bash
dotnet add package HLabs.Containers
```

## Getting Started

```csharp
var image = new ImageReference("nginx");
Console.WriteLine(image); // docker.io/library/nginx:latest
```

### Building references

```csharp
// Implicit construction
var image = new ImageReference("nginx", "trixie"); // → docker.io/library/nginx:trixie

// Explicit construction
var image = new ImageReference(new Repository("nginx"), new Tag("trixie")); // → docker.io/library/nginx:trixie

// Semantic Versioning support (via the Semver package)
var image = new ImageReference("myapp", new SemVersion(3, 1, 0), Registry.GitHub, "myorg"); // → ghcr.io/myorg/myapp:3.1.0
```

### Parsing references

```csharp
var image = ImageReference.Parse("ghcr.io/myorg/myapp:3.1.0");
Console.WriteLine(image.Registry); // ghcr.io
Console.WriteLine(image.Namespace); // myorg
Console.WriteLine(image.Repository); // myapp
Console.WriteLine(image.Tag); // 3.1.0
```

### Modifying using `with` expressions

`ImageReference` is a C# `record`, so you can create modified copies using `with`:

```csharp
var dev = new ImageReference("myapp", Tag.Latest, Registry.Localhost); // → localhost:5000/myapp:latest
var prod = dev with { Registry = Registry.DockerHub, Namespace = "myorg" }; // → docker.io/myorg/myapp:latest
var pinned = prod with { Tag = new SemVersion(2, 1, 0) }; // → docker.io/myorg/myapp:2.1.0
var withDigest = pinned with { Digest = "sha256:a3ed95caeb02..." }; // → docker.io/myorg/myapp:2.1.0@sha256:a3ed95caeb02...
```

### Built-in registries and tags

Common registries and tags are provided as static members:

```csharp
Registry.DockerHub // docker.io
Registry.GitHub // ghcr.io
Registry.Quay // quay.io
Registry.Localhost // localhost:5000
Registry.Acr("mycompany") // mycompany.azurecr.io
Registry.Ecr("123456789", "eu-west-1") // 123456789.dkr.ecr.eu-west-1.amazonaws.com

Tag.Latest // latest
```

### Extending with custom tags

Define your own well-known tags (or registries, repositories, etc.) using C# 14 extensions:

```csharp
internal static class MyTagExtensions 
{
    extension(Tag) 
    {
        public static Tag Dev => new("dev");
        public static Tag Alpha(uint n) => new($"alpha-{n}"); 
    }
}
```

Then use them naturally:

```csharp
var image = new ImageReference("myapp", Tag.Dev, Registry.Localhost); // → localhost:5000/myapp:dev
var alpha = image with { Tag = Tag.Alpha(3) }; // → localhost:5000/myapp:alpha-3
```