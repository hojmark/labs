namespace HLabs.Containers;

/// <summary>
/// Content-addressable (immutable) image reference.
/// Guaranteed to resolve to the same image content due to digest pinning.
/// </summary>
public sealed record CanonicalImageRef : ImageRef {
  public new Registry Registry {
    get;
  }

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
  public CanonicalImageRef WithTag( Tag tag ) =>
    new(Registry, Namespace, Repository, Digest, tag);

  /// <summary>
  /// Returns a new instance with a different registry.
  /// </summary>
  public CanonicalImageRef On( Registry registry ) =>
    new(registry, Namespace, Repository, Digest, Tag);

  /// <summary>
  /// Returns a new instance with a different namespace.
  /// </summary>
  public CanonicalImageRef WithNamespace( Namespace ns ) =>
    new(Registry, ns, Repository, Digest, Tag);

  public override bool IsQualified => true;

  public override string ToString() {
    var nsPart = Namespace is not null ? $"{Namespace}/" : string.Empty;
    return Tag is not null
      ? $"{Registry}/{nsPart}{Repository}:{Tag}@{Digest}" // shows tag + digest
      : $"{Registry}/{nsPart}{Repository}@{Digest}";
  }
}