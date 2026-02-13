namespace HLabs.ImageReferences;

/// <summary>
/// Content-addressable (immutable) image reference.
/// Guaranteed to resolve to the same image content due to digest pinning.
/// </summary>
public sealed record CanonicalImageRef : ImageRef {
  /// <summary>
  /// Gets the registry.
  /// </summary>
  public new Registry Registry {
    get;
  }

  /// <summary>
  /// Gets the digest.
  /// </summary>
  public new Digest Digest {
    get;
  }

  internal CanonicalImageRef(
    Registry registry,
    Namespace? ns,
    Repository repository,
    Digest digest,
    Tag? tag = null
  ) : base( repository ) {
    Registry = registry ?? throw new ArgumentNullException( nameof(registry) );
    Namespace = ns ?? ( Registry.NamespaceRequired ? throw new ArgumentNullException( nameof(ns) ) : null );
    Digest = digest ?? throw new ArgumentNullException( nameof(digest) );
    Tag = tag; // optional, does not affect immutability
  }

  /// <summary>
  /// Returns a new instance with a different cosmetic tag.
  /// </summary>
  /// <param name="tag">The tag to apply to the new reference.</param>
  /// <returns>A new <see cref="CanonicalImageRef"/> with the specified tag.</returns>
  public CanonicalImageRef WithTag( Tag tag ) =>
    new(Registry, Namespace, Repository, Digest, tag);

  /// <summary>
  /// Returns a new instance with a different registry.
  /// </summary>
  /// <param name="registry">The registry to use for the new reference.</param>
  /// <returns>A new <see cref="CanonicalImageRef"/> with the specified registry.</returns>
  public CanonicalImageRef On( Registry registry ) =>
    new(registry, Namespace, Repository, Digest, Tag);

  /// <summary>
  /// Returns a new instance with a different namespace.
  /// </summary>
  /// <param name="ns">The namespace to use for the new reference.</param>
  /// <returns>A new <see cref="CanonicalImageRef"/> with the specified namespace.</returns>
  public CanonicalImageRef WithNamespace( Namespace ns ) =>
    new(Registry, ns, Repository, Digest, Tag);

  /// <summary>
  /// Gets a value indicating whether this reference has a fully qualified registry.
  /// Always true for canonical references as they always have a registry.
  /// </summary>
  public override bool IsQualified => true;

  /// <summary>
  /// Gets a value indicating whether this reference is pinned by a digest.
  /// Always true for canonical references as they always have a digest.
  /// </summary>
  public override bool IsPinned => true;

  /// <summary>
  /// Returns a string representation of this canonical image reference,
  /// including the registry, namespace (if present), repository, digest, and tag (if present).
  /// </summary>
  /// <returns>A string representation of this canonical image reference.</returns>
  public override string ToString() {
    var nsPart = Namespace is not null ? $"{Namespace}/" : string.Empty;
    return Tag is not null
      ? $"{Registry}/{nsPart}{Repository}:{Tag}@{Digest}" // shows tag + digest
      : $"{Registry}/{nsPart}{Repository}@{Digest}";
  }
}