using System.Diagnostics.CodeAnalysis;

namespace HLabs.Containers;

// TODO support platform
// TODO docs
/// <summary>
/// ~~A fully-qualified container image reference.~~
/// <example>
/// example.com:5000/team/my-app:2.0
///
/// Host: example.com:5000
/// Namespace: team
/// Repository: my-app
/// Tag: 2.0
/// </example>
/// </summary>
public sealed partial record PartialImageRef : ImageRef {
  private PartialImageRef(
    Repository repository,
    Tag? tag = null,
    Registry? registry = null,
    Namespace? @namespace = null,
    Digest? digest = null
  ) : base( repository ) {
    Tag = tag;
    Registry = registry;
    Namespace = @namespace;
    Digest = digest;
  }

#pragma warning disable SA1124

  #region DockerHub

#pragma warning restore SA1124

  public PartialImageRef( Repository repository )
    : this( repository, null, null, null, null ) {
  }

  public PartialImageRef( Repository repository, Tag tag )
    : this( repository, tag, null, null, null ) {
  }

  public PartialImageRef( Namespace ns, Repository repository )
    : this( repository, null, null, ns, null ) {
  }

  public PartialImageRef( Namespace ns, Repository repository, Tag tag )
    : this( repository, tag, null, ns, null ) {
  }

  public PartialImageRef( Repository repository, Digest digest )
    : this( repository, null, null, null, digest ) {
  }

  public PartialImageRef( Repository repository, Tag tag, Digest digest )
    : this( repository, tag, null, null, digest ) {
  }

  #endregion

#pragma warning disable SA1124

  #region Any registry

#pragma warning restore SA1124
  public PartialImageRef( Registry registry, Repository repository )
    : this( repository, null, registry, null, null ) {
  }

  // Overload clashes with Namespace + Repository + Tag when using string literals (implicit conversions), so not included for now.
  // The more common use case is probably the other one.
  // TODO decide
  public PartialImageRef( Registry registry, Repository repository, Tag tag )
    : this( repository, tag, registry, null, null ) {
  }

  public PartialImageRef( Registry registry, Repository repository, Digest digest )
    : this( repository, null, registry, null, digest ) {
  }

  public PartialImageRef( Registry registry, Namespace ns, Repository repository )
    : this( repository, null, registry, ns, null ) {
  }

  public PartialImageRef( Registry registry, Namespace ns, Repository repository, Tag tag )
    : this( repository, tag, registry, ns, null ) {
  }

  public PartialImageRef( Registry registry, Namespace ns, Repository repository, Digest digest )
    : this( repository, null, registry, ns, digest ) {
  }

  public PartialImageRef( Registry registry, Repository repository, Tag tag, Digest digest )
    : this( repository, tag, registry, null, digest ) {
  }

  public PartialImageRef( Registry registry, Namespace ns, Repository repository, Tag tag, Digest digest )
    : this( repository, tag, registry, ns, digest ) {
  }

  #endregion

  public override string ToString() {
    var reg = Registry is null ? string.Empty : $"{Registry}/";
    var ns = Namespace is null ? string.Empty : $"{Namespace}/";
    var tag = Tag is null ? string.Empty : $":{Tag}";
    var digest = Digest is null ? string.Empty : $"@{Digest}";
    return $"{reg}{ns}{Repository}{tag}{digest}";
  }

  public bool TryQualify( out QualifiedImageRef? canonical, out string? reason ) {
    try {
      canonical = Qualify();
      reason = null;
      return true;
    }
    catch ( Exception ex ) {
      canonical = null;
      reason = ex.Message;
      return false;
    }
  }

  /// <summary>
  /// Returns a new instance with a different cosmetic tag.
  /// </summary>
  public PartialImageRef With( Tag? tag ) =>
    new(Repository, tag, Registry, Namespace, Digest);

  /// <summary>
  /// Returns a new instance with a different registry.
  /// </summary>
  public PartialImageRef With( Registry? registry ) =>
    new(Repository, Tag, registry, Namespace, Digest);

  /// <summary>
  /// Returns a new instance with a different registry.
  /// </summary>
  public PartialImageRef With( Registry registry, Namespace ns ) =>
    new(Repository, Tag, registry, ns, Digest);

  /// <summary>
  /// Returns a new instance with a different namespace.
  /// </summary>
  public PartialImageRef With( Namespace? ns ) =>
    // TODO remember registry/namespace requirement
    new(Repository, Tag, Registry, ns, Digest);

  /// <summary>
  /// Returns a new instance with a different digest.
  /// </summary>
  public PartialImageRef With( Digest? digest ) =>
    new(Repository, Tag, Registry, Namespace, digest);

  public override bool IsQualified => Registry != null && ( !Registry.NamespaceRequired || Namespace != null ) &&
                                      ( Tag != null || Digest != null );

  public bool CanQualify => TryQualify( out _, out _ );

  /// <summary>
  /// Converts this reference to a canonical form.
  /// Attempts to fill in defaults for missing registry/namespace based on common conventions (e.g. Docker Hub defaults).
  /// Either a tag or digest must be present.
  /// </summary>
  /// <param name="mode">The behavior when the reference is not in a canonical form.</param>
  /// <exception cref="InvalidOperationException">Throws InvalidOperationException if the reference is not in a qualified form.</exception>
  /// <returns>A canonical image reference.</returns>
  public QualifiedImageRef Qualify( QualificationMode mode = QualificationMode.DefaultFilling ) {
    // Defaults
    var registry = Registry ?? Registry.DockerHub;
    var ns = Namespace ?? ( registry == Registry.DockerHub ? Namespace.Library : null );
    var repo = Repository ?? throw new InvalidOperationException( "Repository is required" );
    var tag = Tag ?? ( Digest is null ? Tag.Latest : null );

    if ( tag is null && Digest is null ) {
      throw new InvalidOperationException( "Canonical image reference must have either a tag or a digest." );
    }

    return new QualifiedImageRef( registry, ns, repo, tag, Digest );
  }


  /// <summary>
  /// Canonicalizes the image reference using the current digest.
  /// </summary>
  /// <param name="canonicalizationMode">Whether to maintain or exclude the tag.</param>
  /// <param name="qualificationMode">How to handle missing components when qualifying.</param>
  /// <returns>A canonical image reference.</returns>
  /// <exception cref="InvalidOperationException">Thrown when digest is not present.</exception>
  public CanonicalImageRef Canonicalize(
    CanonicalizationMode canonicalizationMode = CanonicalizationMode.ExcludeTag,
    QualificationMode qualificationMode = QualificationMode.DefaultFilling
  ) {
    return Qualify( qualificationMode ).Canonicalize( canonicalizationMode );
  }


  /// <summary>
  /// Canonicalizes the image reference with a specific digest.
  /// </summary>
  /// <param name="digest">The digest to use.</param>
  /// <param name="canonicalizationMode">Whether to maintain or exclude the tag.</param>
  /// <param name="qualificationMode">How to handle missing components when qualifying.</param>
  /// <returns>A canonical image reference.</returns>
  public CanonicalImageRef Canonicalize(
    Digest digest,
    CanonicalizationMode canonicalizationMode = CanonicalizationMode.ExcludeTag,
    QualificationMode qualificationMode = QualificationMode.DefaultFilling
  ) {
    return With( digest ).Canonicalize( canonicalizationMode, qualificationMode );
  }
}