namespace HLabs.ImageReferences;

// TODO support platform
// TODO add no-arg Canonicalize?
/// <summary>
/// Partial image reference.
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
    Repository repository,
#pragma warning disable S3427
    Tag? tag = null,
#pragma warning restore S3427
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

  // Overload clashes with Namespace + Repository + Tag when using string literals (implicit conversions), so not included for now.
  // The more common use case is probably the other one.
  // TODO decide

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
  public PartialImageRef( Registry registry, Repository repository, Digest digest )
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
  /// Returns a string representation of this image reference.
  /// </summary>
  /// <returns>A string representation of this image reference.</returns>
  public override string ToString() {
    var reg = Registry is null ? string.Empty : $"{Registry}/";
    var ns = Namespace is null ? string.Empty : $"{Namespace}/";
    var tag = Tag is null ? string.Empty : $":{Tag}";
    var digest = Digest is null ? string.Empty : $"@{Digest}";
    return $"{reg}{ns}{Repository}{tag}{digest}";
  }

  /// <summary>
  /// Tries to convert this partial reference to a qualified reference by applying default conventions.
  /// </summary>
  /// <param name="canonical">When this method returns, contains the qualified reference if qualification succeeded, or null if it failed.</param>
  /// <param name="reason">When this method returns, contains an error message if qualification failed, or null if it succeeded.</param>
  /// <returns>true if qualification succeeded; otherwise, false.</returns>
  public bool TryQualify( out QualifiedImageRef? canonical, out string? reason ) {
    try {
      canonical = Qualify();
      reason = null;
      return true;
    }
#pragma warning disable CA1031
    catch ( Exception ex ) {
#pragma warning restore CA1031
      canonical = null;
      reason = ex.Message;
      return false;
    }
  }

  /// <summary>
  /// Returns a new instance with a different cosmetic tag.
  /// </summary>
  /// <param name="tag">The tag to use, or null to remove the tag.</param>
  /// <returns>A new <see cref="PartialImageRef"/> with the specified tag.</returns>
  public PartialImageRef With( Tag? tag ) =>
    new(Repository, tag, Registry, Namespace, Digest);

  /// <summary>
  /// Returns a new instance with a different registry.
  /// </summary>
  /// <param name="registry">The registry to use, or null to remove the registry.</param>
  /// <returns>A new <see cref="PartialImageRef"/> with the specified registry.</returns>
  public PartialImageRef With( Registry? registry ) =>
    new(Repository, Tag, registry, Namespace, Digest);

  /// <summary>
  /// Returns a new instance with a different registry.
  /// </summary>
  /// <param name="registry">The registry to use.</param>
  /// <param name="ns">The namespace to use.</param>
  /// <returns>A new <see cref="PartialImageRef"/> with the specified registry and namespace.</returns>
  public PartialImageRef With( Registry registry, Namespace ns ) =>
    new(Repository, Tag, registry, ns, Digest);

  /// <summary>
  /// Returns a new instance with a different namespace.
  /// </summary>
  /// <param name="ns">The namespace to use, or null to remove the namespace.</param>
  /// <returns>A new <see cref="PartialImageRef"/> with the specified namespace.</returns>
  public PartialImageRef With( Namespace? ns ) =>
    // TODO remember registry/namespace requirement
    new(Repository, Tag, Registry, ns, Digest);

  /// <summary>
  /// Returns a new instance with a different digest.
  /// </summary>
  /// <param name="digest">The digest to use, or null to remove the digest.</param>
  /// <returns>A new <see cref="PartialImageRef"/> with the specified digest.</returns>
  public PartialImageRef With( Digest? digest ) =>
    new(Repository, Tag, Registry, Namespace, digest);

  // Additional fluent builder methods for better discoverability
  
  /// <summary>
  /// Returns a new instance with a different tag.
  /// Alias for <see cref="With(Tag?)"/> for better fluent API discoverability.
  /// </summary>
  /// <param name="tag">The tag to use, or null to remove the tag.</param>
  /// <returns>A new <see cref="PartialImageRef"/> with the specified tag.</returns>
  public PartialImageRef WithTag( Tag? tag ) => With( tag );

  /// <summary>
  /// Returns a new instance with a different tag.
  /// </summary>
  /// <param name="tag">The tag to use as a string.</param>
  /// <returns>A new <see cref="PartialImageRef"/> with the specified tag.</returns>
  public PartialImageRef WithTag( string tag ) => With( new Tag( tag ) );

  /// <summary>
  /// Returns a new instance with a different registry.
  /// Alias for <see cref="With(Registry?)"/> for better fluent API discoverability.
  /// </summary>
  /// <param name="registry">The registry to use, or null to remove the registry.</param>
  /// <returns>A new <see cref="PartialImageRef"/> with the specified registry.</returns>
  public PartialImageRef WithRegistry( Registry? registry ) => With( registry );

  /// <summary>
  /// Returns a new instance with a different registry.
  /// </summary>
  /// <param name="registryHostname">The registry hostname to use as a string.</param>
  /// <returns>A new <see cref="PartialImageRef"/> with the specified registry.</returns>
  public PartialImageRef WithRegistry( string registryHostname ) => With( new Registry( registryHostname ) );

  /// <summary>
  /// Returns a new instance with a different namespace.
  /// Alias for <see cref="With(Namespace?)"/> for better fluent API discoverability.
  /// </summary>
  /// <param name="ns">The namespace to use, or null to remove the namespace.</param>
  /// <returns>A new <see cref="PartialImageRef"/> with the specified namespace.</returns>
  public PartialImageRef WithNamespace( Namespace? ns ) => With( ns );

  /// <summary>
  /// Returns a new instance with a different namespace.
  /// </summary>
  /// <param name="ns">The namespace to use as a string.</param>
  /// <returns>A new <see cref="PartialImageRef"/> with the specified namespace.</returns>
  public PartialImageRef WithNamespace( string ns ) => With( new Namespace( ns ) );

  /// <summary>
  /// Returns a new instance with a different digest.
  /// Alias for <see cref="With(Digest?)"/> for better fluent API discoverability.
  /// </summary>
  /// <param name="digest">The digest to use, or null to remove the digest.</param>
  /// <returns>A new <see cref="PartialImageRef"/> with the specified digest.</returns>
  public PartialImageRef WithDigest( Digest? digest ) => With( digest );

  /// <summary>
  /// Returns a new instance with a different digest.
  /// </summary>
  /// <param name="digest">The digest to use as a string.</param>
  /// <returns>A new <see cref="PartialImageRef"/> with the specified digest.</returns>
  public PartialImageRef WithDigest( string digest ) => With( new Digest( digest ) );

  /// <summary>
  /// Configures this image for a specific registry and namespace.
  /// </summary>
  /// <param name="registry">The registry to use.</param>
  /// <param name="ns">The namespace to use.</param>
  /// <returns>A new <see cref="PartialImageRef"/> with the specified registry and namespace.</returns>
  public PartialImageRef On( Registry registry, Namespace ns ) => With( registry, ns );

  /// <summary>
  /// Configures this image for a specific registry and namespace.
  /// </summary>
  /// <param name="registryHostname">The registry hostname to use as a string.</param>
  /// <param name="ns">The namespace to use as a string.</param>
  /// <returns>A new <see cref="PartialImageRef"/> with the specified registry and namespace.</returns>
  public PartialImageRef On( string registryHostname, string ns ) => 
    With( new Registry( registryHostname ), new Namespace( ns ) );

  /// <summary>
  /// Configures this image for a specific registry.
  /// </summary>
  /// <param name="registry">The registry to use.</param>
  /// <returns>A new <see cref="PartialImageRef"/> with the specified registry.</returns>
  public PartialImageRef On( Registry registry ) => With( registry );

  /// <summary>
  /// Configures this image for a specific registry.
  /// </summary>
  /// <param name="registryHostname">The registry hostname to use as a string.</param>
  /// <returns>A new <see cref="PartialImageRef"/> with the specified registry.</returns>
  public PartialImageRef On( string registryHostname ) => With( new Registry( registryHostname ) );

  /// <summary>
  /// Configures this image for a specific namespace.
  /// </summary>
  /// <param name="ns">The namespace to use.</param>
  /// <returns>A new <see cref="PartialImageRef"/> with the specified namespace.</returns>
  public PartialImageRef In( Namespace ns ) => With( ns );

  /// <summary>
  /// Configures this image for a specific namespace.
  /// </summary>
  /// <param name="ns">The namespace to use as a string.</param>
  /// <returns>A new <see cref="PartialImageRef"/> with the specified namespace.</returns>
  public PartialImageRef In( string ns ) => With( new Namespace( ns ) );

  // Static factory methods as alternatives to constructors

  /// <summary>
  /// Creates a partial image reference from a repository name.
  /// Alternative to using constructors.
  /// </summary>
  /// <param name="repository">The repository name.</param>
  /// <returns>A new <see cref="PartialImageRef"/>.</returns>
  public static PartialImageRef From( Repository repository ) => new(repository);

  /// <summary>
  /// Creates a partial image reference from a repository name.
  /// Alternative to using constructors.
  /// </summary>
  /// <param name="repository">The repository name as a string.</param>
  /// <returns>A new <see cref="PartialImageRef"/>.</returns>
  public static PartialImageRef From( string repository ) => new(new Repository( repository ));

  /// <summary>
  /// Creates a partial image reference from a repository and tag.
  /// Alternative to using constructors.
  /// </summary>
  /// <param name="repository">The repository name.</param>
  /// <param name="tag">The tag.</param>
  /// <returns>A new <see cref="PartialImageRef"/>.</returns>
  public static PartialImageRef From( string repository, string tag ) => 
    new(new Repository( repository ), new Tag( tag ));

  /// <summary>
  /// Creates a partial image reference for the localhost registry.
  /// Convenient factory method for development scenarios.
  /// </summary>
  /// <param name="repository">The repository name.</param>
  /// <param name="tag">The tag (optional, defaults to "latest").</param>
  /// <returns>A new <see cref="PartialImageRef"/> configured for localhost.</returns>
  public static PartialImageRef Localhost( string repository, string? tag = null ) =>
    From( repository )
      .WithRegistry( Registry.Localhost )
      .WithTag( tag ?? "latest" );

  /// <summary>
  /// Creates a partial image reference for GitHub Container Registry.
  /// Convenient factory method for GitHub-hosted images.
  /// </summary>
  /// <param name="namespace">The GitHub organization or username.</param>
  /// <param name="repository">The repository name.</param>
  /// <param name="tag">The tag (optional, defaults to "latest").</param>
  /// <returns>A new <see cref="PartialImageRef"/> configured for GitHub Container Registry.</returns>
  public static PartialImageRef GitHub( string @namespace, string repository, string? tag = null ) =>
    From( repository )
      .WithRegistry( Registry.GitHub )
      .WithNamespace( @namespace )
      .WithTag( tag ?? "latest" );

  /// <summary>
  /// Creates a partial image reference for Docker Hub.
  /// Convenient factory method for Docker Hub-hosted images.
  /// </summary>
  /// <param name="namespace">The Docker Hub organization or username.</param>
  /// <param name="repository">The repository name.</param>
  /// <param name="tag">The tag (optional, defaults to "latest").</param>
  /// <returns>A new <see cref="PartialImageRef"/> configured for Docker Hub.</returns>
  public static PartialImageRef DockerHub( string @namespace, string repository, string? tag = null ) =>
    From( repository )
      .WithRegistry( Registry.DockerHub )
      .WithNamespace( @namespace )
      .WithTag( tag ?? "latest" );

  /// <summary>
  /// Gets a value indicating whether this reference has all required components for qualification.
  /// </summary>
  public override bool IsQualified => Registry != null && ( !Registry.NamespaceRequired || Namespace != null ) &&
                                      ( Tag != null || Digest != null );

  /// <summary>
  /// Gets a value indicating whether this reference can be qualified using default conventions.
  /// </summary>
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