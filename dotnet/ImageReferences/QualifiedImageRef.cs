namespace HLabs.ImageReferences;

// -----------------------------
// Qualified image reference: fully qualified (registry, namespace, repository, tag or digest)
// -----------------------------
/// <summary>
/// Qualified image reference that can address a specific image.
/// The underlying image may change over time (e.g., tag can be moved).
/// Requires registry and repository, with namespace depending on registry requirements.
/// Must have either a tag or digest.
/// </summary>
public sealed record QualifiedImageRef : ImageRef {
  /// <summary>
  /// Gets the registry.
  /// </summary>
  public new Registry Registry {
    get;
  }

  /// <summary>
  /// Gets the repository.
  /// </summary>
  public new Repository Repository {
    get;
  }

  internal QualifiedImageRef( Registry registry, Namespace? ns, Repository repository, Tag? tag, Digest? digest ) {
    Registry = registry ?? throw new ArgumentNullException( nameof(registry) );
    Namespace = ns ?? ( Registry.NamespaceRequired ? throw new ArgumentNullException( nameof(ns) ) : null );
    Repository = repository ?? throw new ArgumentNullException( nameof(repository) );

    if ( tag is null && digest is null ) {
      throw new InvalidOperationException( "Canonical image reference must have either a tag or a digest." );
    }

    Tag = tag;
    Digest = digest;
  }

  /// <summary>
  /// Returns a new instance with the specified tag.
  /// </summary>
  /// <param name="tag">The tag, or null to remove it.</param>
  /// <returns>A new <see cref="QualifiedImageRef"/> with the specified tag.</returns>
  public QualifiedImageRef With( Tag? tag ) =>
    new(Registry, Namespace, Repository, tag, Digest);

  /// <summary>
  /// Returns a new instance with the specified registry.
  /// </summary>
  /// <param name="registry">The registry.</param>
  /// <returns>A new <see cref="QualifiedImageRef"/> with the specified registry.</returns>
  public QualifiedImageRef With( Registry registry ) =>
    new(registry, Namespace, Repository, Tag, Digest);

  /// <summary>
  /// Returns a new instance with the specified registry and namespace.
  /// </summary>
  /// <param name="registry">The registry.</param>
  /// <param name="ns">The namespace.</param>
  /// <returns>A new <see cref="QualifiedImageRef"/> with the specified registry and namespace.</returns>
  public QualifiedImageRef With( Registry registry, Namespace ns ) =>
    new(registry, ns, Repository, Tag, Digest);

  /// <summary>
  /// Returns a new instance with the specified namespace.
  /// </summary>
  /// <param name="ns">The namespace, or null to remove it.</param>
  /// <returns>A new <see cref="QualifiedImageRef"/> with the specified namespace.</returns>
  public QualifiedImageRef With( Namespace? ns ) =>
    new(Registry, ns, Repository, Tag, Digest);

  /// <summary>
  /// Returns a new instance with the specified digest.
  /// </summary>
  /// <param name="digest">The digest, or null to remove it.</param>
  /// <returns>A new <see cref="QualifiedImageRef"/> with the specified digest.</returns>
  public QualifiedImageRef With( Digest? digest ) =>
    new(Registry, Namespace, Repository, Tag, digest);

  /// <summary>
  /// Creates a digest-pinned image reference.
  /// </summary>
  /// <param name="digest">The digest to pin the reference with.</param>
  /// <param name="mode">Specifies whether to maintain or exclude the tag in the canonical reference.</param>
  /// <returns>A canonical (immutable) image reference.</returns>
  /// <exception cref="ArgumentNullException">Thrown when digest is null.</exception>
  public CanonicalImageRef Canonicalize( Digest digest, CanonicalizationMode mode = CanonicalizationMode.ExcludeTag ) {
    ArgumentNullException.ThrowIfNull( digest );

    var tag = mode == CanonicalizationMode.MaintainTag ? Tag : null;
    return new CanonicalImageRef( Registry, Namespace, Repository, digest, tag );
  }

  /// <summary>
  /// Creates a digest-pinned image reference using the current digest.
  /// </summary>
  /// <param name="mode">Specifies whether to maintain or exclude the tag in the canonical reference.</param>
  /// <returns>A canonical (immutable) image reference.</returns>
  /// <exception cref="InvalidOperationException">Thrown when digest is not present.</exception>
  public CanonicalImageRef Canonicalize( CanonicalizationMode mode = CanonicalizationMode.ExcludeTag ) {
    if ( Digest is null ) {
      throw new InvalidOperationException(
        "Cannot canonicalize without a digest. Use Canonicalize(digest) to provide one." );
    }

    var tag = mode == CanonicalizationMode.MaintainTag ? Tag : null;
    return new CanonicalImageRef( Registry, Namespace, Repository, Digest, tag );
  }

  /// <summary>
  /// Returns a string representation of this qualified image reference.
  /// </summary>
  /// <returns>A string representation of this qualified image reference.</returns>
  public override string ToString() {
    var nsPart = Namespace is not null ? $"{Namespace}/" : string.Empty;
    var tagPart = Tag is not null ? $":{Tag}" : string.Empty;
    var digestPart = Digest is not null ? $"@{Digest}" : string.Empty;
    return $"{Registry}/{nsPart}{Repository}{tagPart}{digestPart}";
  }
}