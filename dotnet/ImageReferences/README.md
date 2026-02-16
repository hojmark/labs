# `HLabs.ImageReferences`

Strongly-typed container image references for .NET.

```bash
dotnet add package HLabs.ImageReferences
```

## Getting Started

```csharp
var partial = "nginx".Image(); // nginx

var qualified = partial.Qualified(); // docker.io/library/nginx:latest

var canonical = partial.Canonical("57e903..."); // docker.io/library/nginx@sha256:57e903...
```

### Parsing references

```csharp
var image = "ghcr.io/myorg/myapp:3.1.0".Image();

Registry   reg  = image.Registry; // ghcr.io
Namespace  ns   = image.Namespace; // myorg
Repository repo = image.Repository; // myapp
Tag        tag  = image.Tag; // 3.1.0
```

### Modifying references

```csharp
var dev = new ImageReference( "myapp", Tag.Latest, Registry.Localhost ); // localhost:5000/myapp:latest
var prod = dev.With( Registry.DockerHub, "myorg" ); // docker.io/myorg/myapp:latest
var pinned = prod.With( new SemVersion(2, 1, 0)); // docker.io/myorg/myapp:2.1.0
var withDigest = pinned.With( "sha256:a3ed95caeb02..." ); // docker.io/myorg/myapp@sha256:a3ed95caeb02...
```

### Built-in registries and tags

```csharp
Registry.DockerHub // docker.io
Registry.GitHub // ghcr.io
Registry.Quay // quay.io
Registry.Localhost // localhost:5000
Registry.Acr("mycompany") // mycompany.azurecr.io
Registry.Ecr("1234", "eu-west-1") // 1234.dkr.ecr.eu-west-1.amazonaws.com

Tag.Latest // latest
```

### Extending with custom tags

Define your own well-known tags (or registries, repositories, etc.) using C# 14 extensions:

```csharp
static class MyExtensions 
{
    extension(Tag) 
    {
        static Tag Dev => new("dev");
        static Tag Alpha(uint n) => new($"alpha-{n}"); 
    }
    
    extension(Registry) 
    {
        static Registry Internal => Registry.Ecr("1234", "eu-west-1")
    }
}
```

Then use them naturally:

```csharp
var image = "myapp".Image( Tag.Dev, Registry.Localhost); // localhost:5000/myapp:dev
var alpha = image.With( Registry.Internal, Tag.Alpha(3) ); // 1234.dkr.ecr.eu-west-1.amazonaws.com/myapp:alpha-3
```
