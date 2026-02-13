namespace HLabs.ImageReferences;

/// <summary>
/// Base class for container image references, providing common properties.
/// </summary>
public abstract record ImageRef {
  /// <summary>
  /// Gets the registry where the image is hosted (e.g., docker.io, ghcr.io).
  /// May be null for partial references.
  /// </summary>
  public Registry? Registry {
    get;
    protected init;
  }

#pragma warning disable CA1716
  /// <summary>
  /// Gets the namespace within the registry (e.g., organization or username).
  /// May be null for partial references or registries that don't require namespaces.
  /// </summary>
  public Namespace? Namespace {
#pragma warning restore CA1716
    get;
    protected init;
  }

  /// <summary>
  /// Gets the repository name (e.g., nginx, ubuntu, myapp).
  /// May be null for partial references.
  /// </summary>
  public Repository? Repository {
    get;
    protected init;
  }

  /// <summary>
  /// Gets the tag that identifies a version or variant (e.g., latest, v1.0, alpine).
  /// May be null for partial references or qualified references with a digest.
  /// </summary>
  public Tag? Tag {
    get;
    protected init;
  }

  /// <summary>
  /// Gets the content-addressable digest that uniquely identifies image content.
  /// May be null for partial references or qualified references with a tag.
  /// </summary>
  public Digest? Digest {
    get;
    protected init;
  }
}