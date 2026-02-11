using System.Diagnostics.CodeAnalysis;

namespace HLabs.Containers;

[SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1615:Element return value should be documented" )]
[SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1611:Element parameters should be documented" )]
[SuppressMessage(
  "StyleCop.CSharp.MaintainabilityRules",
  "SA1404:Code analysis suppression should have justification" )]
/// <summary>
/// Content-addressable (immutable) image reference.
/// </summary>
public sealed record CanonicalImageRef : ImageRef {
  public new Registry Registry {
    get;
  }

  /* public Namespace? Namespace {
     get;
   }*/

  /*public Repository Repository {
    get;
  }*/

  public new Digest Digest {
    get;
  }

  /*public Tag? Tag {
    get;
  } // optional cosmetic / informational tag
  */

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
    // TODO remember registry/namespace requirement
    new(Registry, ns, Repository, Digest, Tag);

  public override bool IsQualified => true;

  public override string ToString() {
    var nsPart = Namespace is not null ? $"{Namespace}/" : string.Empty;
    return Tag is not null
      ? $"{Registry}/{nsPart}{Repository}:{Tag}@{Digest}" // shows tag + digest
      : $"{Registry}/{nsPart}{Repository}@{Digest}";
  }
}