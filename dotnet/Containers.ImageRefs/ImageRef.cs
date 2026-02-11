using HLabs.Containers.Components;

namespace HLabs.Containers;

/// <summary>
/// Base class for container image references, providing common properties.
/// </summary>
public abstract record ImageRef {
  /// <summary>
  /// Initializes a new instance of the <see cref="ImageRef"/> class.
  /// </summary>
  /// <param name="repository">The repository name. Cannot be null.</param>
  /// <exception cref="ArgumentNullException">Thrown when repository is null.</exception>
  private protected ImageRef( Repository repository ) {
    Repository = repository ?? throw new ArgumentNullException( nameof(repository) );
  }

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
  /// Gets the namespace within the registry (e.g., organization or user name).
  /// May be null for partial references or registries that don't require namespaces.
  /// </summary>
  public Namespace? Namespace {
#pragma warning restore CA1716
    get;
    protected init;
  }

  /// <summary>
  /// Gets the repository name (e.g., nginx, ubuntu, myapp).
  /// </summary>
  public Repository Repository {
    get;
  }

  /// <summary>
  /// Gets the tag that identifies a version or variant (e.g., latest, v1.0, alpine).
  /// May be null for digest-only references.
  /// </summary>
  public Tag? Tag {
    get;
    protected init;
  }

  /// <summary>
  /// Gets the content-addressable digest that uniquely identifies image content.
  /// May be null for tag-only references.
  /// </summary>
  public Digest? Digest {
    get;
    protected init;
  }

  /// <summary>
  /// Gets a value indicating whether this reference is pinned by a <see cref="Digest"/>.
  /// </summary>
  public virtual bool IsPinned => Digest is not null;

  /// <summary>
  /// Gets a value indicating whether this reference can identify a specific image (has at least a tag or digest).
  /// Note: a tag-based canonical reference is not pinned — the tag can be moved to a different image.
  /// </summary>
  public abstract bool IsQualified { get; }
}