namespace HLabs.Containers;

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
  public new Registry Registry {
    get;
  }

  internal QualifiedImageRef( Registry registry, Namespace? ns, Repository repository, Tag? tag, Digest? digest ) :
    base( repository ) {
    Registry = registry ?? throw new ArgumentNullException( nameof(registry) );
    Namespace = ns ?? ( Registry.NamespaceRequired ? throw new ArgumentNullException( nameof(ns) ) : null );

    if ( tag is null && digest is null ) {
      throw new InvalidOperationException( "Canonical image reference must have either a tag or a digest." );
    }

    Tag = tag;
    Digest = digest;
  }

  /// <summary>
  /// Returns a new instance with a different tag.
  /// </summary>
  public QualifiedImageRef WithTag( Tag tag ) =>
    new(Registry, Namespace, Repository, tag, Digest);

  /// <summary>
  /// Returns a new instance with a different registry.
  /// </summary>
  public QualifiedImageRef On( Registry registry, Namespace? ns = null ) =>
    new(registry, ns ?? Namespace, Repository, Tag, Digest);

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
  /// <exception cref="InvalidOperationException">Thrown when digest is not present.</exception>
  public CanonicalImageRef Canonicalize( CanonicalizationMode mode = CanonicalizationMode.ExcludeTag ) {
    if ( Digest is null ) {
      throw new InvalidOperationException(
        "Cannot canonicalize without a digest. Use Canonicalize(digest) to provide one." );
    }

    var tag = mode == CanonicalizationMode.MaintainTag ? Tag : null;
    return new CanonicalImageRef( Registry, Namespace, Repository, Digest, tag );
  }

  public override bool IsQualified => true;

  public override string ToString() {
    var nsPart = Namespace is not null ? $"{Namespace}/" : string.Empty;
    var tagPart = Tag is not null ? $":{Tag}" : string.Empty;
    var digestPart = Digest is not null ? $"@{Digest}" : string.Empty;
    return $"{Registry}/{nsPart}{Repository}{tagPart}{digestPart}";
  }
}