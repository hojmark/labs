namespace HLabs.Containers;

public abstract record ImageRef {
  protected ImageRef( Repository repository ) {
    Repository = repository ?? throw new ArgumentNullException( nameof(repository) );
  }

  public Registry? Registry {
    get;
    protected init;
  }

#pragma warning disable CA1716
  public Namespace? Namespace {
#pragma warning restore CA1716
    get;
    protected init;
  }

  public Repository Repository {
    get;
  }

  public Tag? Tag {
    get;
    protected init;
  }

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