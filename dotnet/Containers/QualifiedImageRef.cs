using System.Diagnostics.CodeAnalysis;

namespace HLabs.Containers;

// -----------------------------
// Canonical image reference: fully qualified (registry, namespace, repository, tag or digest)
// -----------------------------
[SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1615:Element return value should be documented" )]
[SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1611:Element parameters should be documented" )]
[SuppressMessage(
  "StyleCop.CSharp.MaintainabilityRules",
  "SA1404:Code analysis suppression should have justification" )]
/// <summary>
/// Mutable image reference.
/// Can adress an image, but the underlying image may change.
/// (registry, optional- depends on registy namespace, repository, tag or digest)
/// </summary>
public sealed record QualifiedImageRef : ImageRef {
  public new Registry Registry {
    get;
  }

  /*
  public Namespace? Namespace {
    get;
  } // only required if the registry requires it
  */

/*
public Tag? Tag {
  get;
} // nullable if digest-only

public Digest? Digest {
  get;
} // nullable if tag-only
*/
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
  /// Creates a pinned image reference.
  /// </summary>
  public CanonicalImageRef Canonicalize( Digest digest ) {
    ArgumentNullException.ThrowIfNull( digest );

    return new CanonicalImageRef( Registry, Namespace, Repository, digest );
  }

  public CanonicalImageRef Canonicalize() {
    // ! TODO:
    return Canonicalize( Digest! );
  }

  public override bool IsQualified => true;

  public override string ToString() {
    var nsPart = Namespace is not null ? $"{Namespace}/" : string.Empty;
    var tagPart = Tag is not null ? $":{Tag}" : string.Empty;
    var digestPart = Digest is not null ? $"@{Digest}" : string.Empty;
    return $"{Registry}/{nsPart}{Repository}{tagPart}{digestPart}";
  }
}