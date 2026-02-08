namespace HLabs.Containers;

// TODO support platform
// TODO docs
/// <summary>
/// A fully-qualified container image reference.
/// <example>
/// example.com:5000/team/my-app:2.0
///
/// Host: example.com:5000
/// Namespace: team
/// Repository: my-app
/// Tag: 2.0
/// </example>
/// </summary>
public sealed partial record ImageReference {
  public Registry Registry {
    get;
    init;
  }

  public Namespace? Namespace {
    get;
    init;
  }

  public Repository Repository {
    get;
  }

  public Tag Tag {
    get;
    init;
  }

  public Digest? Digest {
    get;
    init;
  }

  public ImageReference(
    Repository repository,
    Tag? tag = null,
    Registry? host = null,
    Namespace? @namespace = null,
    Digest? digest = null
  ) {
    Repository = repository;
    Tag = tag ?? Tag.Latest;
    Registry = host ?? Registry.DockerHub;
    Namespace = @namespace ?? ( Registry == Registry.DockerHub ? Namespace.Library : null );
    Digest = digest;
  }

  public override string ToString() {
    var ns = Namespace is null ? string.Empty : $"{Namespace}/";
    var digest = Digest is null ? string.Empty : $"@{Digest}";
    return $"{Registry}/{ns}{Repository}:{Tag}{digest}";
  }
}