namespace HLabs.ImageReferences;

// TODO support platform
// TODO add no-arg Canonicalize?
/// <summary>
/// Partial image reference that may have some components unspecified.
/// <list type="bullet">
/// <item>
/// <description>Use <see cref="Qualify"/> to convert to a fully qualified reference.</description>
/// </item>
/// <item>
/// <description>Use <see cref="Canonicalize(CanonicalizationMode,QualificationMode)"/> to convert to a canonical reference.</description>
/// </item>
/// </list>
/// </summary>
public sealed partial record PartialImageRef : ImageRef {
  private PartialImageRef(
    Repository? repository,
#pragma warning disable S3427
    Tag? tag = null,
#pragma warning restore S3427
    Registry? registry = null,
    Namespace? @namespace = null,
    Digest? digest = null
  ) {
    Repository = repository;
    Tag = tag;
    Registry = registry;
    Namespace = @namespace;
    Digest = digest;
  }

#pragma warning disable SA1124

  #region DockerHub

#pragma warning restore SA1124

  /// <summary>
  /// Initializes a new instance of the <see cref="PartialImageRef"/> class.
  /// </summary>
  /// <param name="repository">The repository name.</param>
  public PartialImageRef( Repository repository )
    : this( repository, null, null, null, null ) {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="PartialImageRef"/> class.
  /// </summary>
  /// <param name="repository">The repository name.</param>
  /// <param name="tag">The tag.</param>
  public PartialImageRef( Repository repository, Tag tag )
    : this( repository, tag, null, null, null ) {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="PartialImageRef"/> class.
  /// </summary>
  /// <param name="ns">The namespace.</param>
  /// <param name="repository">The repository name.</param>
  public PartialImageRef( Namespace ns, Repository repository )
    : this( repository, null, null, ns, null ) {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="PartialImageRef"/> class.
  /// </summary>
  /// <param name="ns">The namespace.</param>
  /// <param name="repository">The repository name.</param>
  /// <param name="tag">The tag.</param>
  public PartialImageRef( Namespace ns, Repository repository, Tag tag )
    : this( repository, tag, null, ns, null ) {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="PartialImageRef"/> class.
  /// </summary>
  /// <param name="repository">The repository name.</param>
  /// <param name="digest">The digest.</param>
  public PartialImageRef( Repository repository, Digest digest )
    : this( repository, null, null, null, digest ) {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="PartialImageRef"/> class.
  /// </summary>
  /// <param name="repository">The repository name.</param>
  /// <param name="tag">The tag.</param>
  /// <param name="digest">The digest.</param>
  public PartialImageRef( Repository repository, Tag tag, Digest digest )
    : this( repository, tag, null, null, digest ) {
  }

  #endregion

#pragma warning disable SA1124

  #region Any registry

#pragma warning restore SA1124
  /// <summary>
  /// Initializes a new instance of the <see cref="PartialImageRef"/> class.
  /// </summary>
  /// <param name="registry">The registry.</param>
  /// <param name="repository">The repository name.</param>
  public PartialImageRef( Registry registry, Repository repository )
    : this( repository, null, registry, null, null ) {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="PartialImageRef"/> class.
  /// </summary>
  /// <param name="registry">The registry.</param>
  /// <param name="repository">The repository name.</param>
  /// <param name="tag">The tag.</param>
  public PartialImageRef( Registry registry, Repository repository, Tag tag )
    : this( repository, tag, registry, null, null ) {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="PartialImageRef"/> class.
  /// </summary>
  /// <param name="registry">The registry.</param>
  /// <param name="repository">The repository name.</param>
  /// <param name="digest">The digest.</param>
  public PartialImageRef( Registry registry, Repository? repository, Digest digest )
    : this( repository, null, registry, null, digest ) {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="PartialImageRef"/> class.
  /// </summary>
  /// <param name="registry">The registry.</param>
  /// <param name="ns">The namespace.</param>
  /// <param name="repository">The repository name.</param>
  public PartialImageRef( Registry registry, Namespace ns, Repository repository )
    : this( repository, null, registry, ns, null ) {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="PartialImageRef"/> class.
  /// </summary>
  /// <param name="registry">The registry.</param>
  /// <param name="ns">The namespace.</param>
  /// <param name="repository">The repository name.</param>
  /// <param name="tag">The tag.</param>
  public PartialImageRef( Registry registry, Namespace ns, Repository repository, Tag tag )
    : this( repository, tag, registry, ns, null ) {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="PartialImageRef"/> class.
  /// </summary>
  /// <param name="registry">The registry.</param>
  /// <param name="ns">The namespace.</param>
  /// <param name="repository">The repository name.</param>
  /// <param name="digest">The digest.</param>
  public PartialImageRef( Registry registry, Namespace ns, Repository repository, Digest digest )
    : this( repository, null, registry, ns, digest ) {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="PartialImageRef"/> class.
  /// </summary>
  /// <param name="registry">The registry.</param>
  /// <param name="repository">The repository name.</param>
  /// <param name="tag">The tag.</param>
  /// <param name="digest">The digest.</param>
  public PartialImageRef( Registry registry, Repository repository, Tag tag, Digest digest )
    : this( repository, tag, registry, null, digest ) {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="PartialImageRef"/> class.
  /// </summary>
  /// <param name="registry">The registry.</param>
  /// <param name="ns">The namespace.</param>
  /// <param name="repository">The repository name.</param>
  /// <param name="tag">The tag.</param>
  /// <param name="digest">The digest.</param>
  public PartialImageRef( Registry registry, Namespace ns, Repository repository, Tag tag, Digest digest )
    : this( repository, tag, registry, ns, digest ) {
  }

  #endregion

  /// <summary>
  /// Returns a new instance with the specified tag.
  /// </summary>
  /// <param name="tag">The tag.</param>
  /// <returns>A new <see cref="PartialImageRef"/> with the specified tag.</returns>
  public PartialImageRef With( Tag? tag ) =>
    new(Repository, tag, Registry, Namespace, Digest);

  /// <summary>
  /// Returns a new instance with the specified registry.
  /// </summary>
  /// <param name="registry">The registry.</param>
  /// <returns>A new <see cref="PartialImageRef"/> with the specified registry.</returns>
  public PartialImageRef With( Registry? registry ) =>
    new(Repository, Tag, registry, Namespace, Digest);

  /// <summary>
  /// Returns a new instance with the specified registry and namespace.
  /// </summary>
  /// <param name="registry">The registry.</param>
  /// <param name="ns">The namespace.</param>
  /// <returns>A new <see cref="PartialImageRef"/> with the specified registry and namespace.</returns>
  public PartialImageRef With( Registry registry, Namespace ns ) =>
    new(Repository, Tag, registry, ns, Digest);

  /// <summary>
  /// Returns a new instance with the specified namespace.
  /// </summary>
  /// <param name="ns">The namespace.</param>
  /// <returns>A new <see cref="PartialImageRef"/> with the specified namespace.</returns>
  public PartialImageRef With( Namespace? ns ) =>
    new(Repository, Tag, Registry, ns, Digest);

  /// <summary>
  /// Returns a new instance with the specified digest.
  /// </summary>
  /// <param name="digest">The digest.</param>
  /// <returns>A new <see cref="PartialImageRef"/> with the specified digest.</returns>
  public PartialImageRef With( Digest? digest ) =>
    new(Repository, Tag, Registry, Namespace, digest);

  /// <summary>
  /// Tries to convert this partial reference to a qualified reference by applying default conventions.
  /// </summary>
  /// <param name="qualified">When this method returns, contains the qualified reference if qualification succeeded, or null if it failed.</param>
  /// <param name="reason">When this method returns, contains an error message if qualification failed, or null if it succeeded.</param>
  /// <returns>true if qualification succeeded; otherwise, false.</returns>
  public bool TryQualify( out QualifiedImageRef? qualified, out string? reason ) {
    try {
      qualified = Qualify();
      reason = null;
      return true;
    }
#pragma warning disable CA1031
    catch ( Exception ex ) {
#pragma warning restore CA1031
      qualified = null;
      reason = ex.Message;
      return false;
    }
  }

  /// <summary>
  /// Converts this reference to a qualified form by applying default conventions.
  /// Fills in defaults for missing registry/namespace based on common conventions (e.g., Docker Hub defaults).
  /// Either a tag or digest must be present.
  /// </summary>
  /// <param name="mode">The qualification mode to use.</param>
  /// <returns>A qualified image reference.</returns>
  /// <exception cref="InvalidOperationException">Thrown if repository is missing or if neither tag nor digest is present.</exception>
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
  /// Converts this reference to a canonical (digest-pinned) form using the current digest.
  /// </summary>
  /// <param name="canonicalizationMode">Specifies whether to maintain or exclude the tag in the canonical reference.</param>
  /// <param name="qualificationMode">Specifies how to handle missing components when qualifying.</param>
  /// <returns>A canonical image reference.</returns>
  /// <exception cref="InvalidOperationException">Thrown when digest is not present.</exception>
  public CanonicalImageRef Canonicalize(
    CanonicalizationMode canonicalizationMode = CanonicalizationMode.ExcludeTag,
    QualificationMode qualificationMode = QualificationMode.DefaultFilling
  ) {
    return Qualify( qualificationMode ).Canonicalize( canonicalizationMode );
  }

  /// <summary>
  /// Converts this reference to a canonical (digest-pinned) form with a specific digest.
  /// </summary>
  /// <param name="digest">The digest to use.</param>
  /// <param name="canonicalizationMode">Specifies whether to maintain or exclude the tag in the canonical reference.</param>
  /// <param name="qualificationMode">Specifies how to handle missing components when qualifying.</param>
  /// <returns>A canonical image reference.</returns>
  public CanonicalImageRef Canonicalize(
    Digest digest,
    CanonicalizationMode canonicalizationMode = CanonicalizationMode.ExcludeTag,
    QualificationMode qualificationMode = QualificationMode.DefaultFilling
  ) {
    return With( digest ).Canonicalize( canonicalizationMode, qualificationMode );
  }

  /// <summary>
  /// Returns the string representation of this partial image reference.
  /// </summary>
  /// <returns>A string representation of this partial image reference.</returns>
  public override string ToString() {
    var reg = Registry is null ? string.Empty : $"{Registry}/";
    var ns = Namespace is null ? string.Empty : $"{Namespace}/";
    var repo = Repository is null ? string.Empty : $"{Repository}";
    var tag = Tag is null ? string.Empty : $":{Tag}";
    var digest = Digest is null ? string.Empty : $"@{Digest}";
    return $"{reg}{ns}{repo}{tag}{digest}";
  }
}