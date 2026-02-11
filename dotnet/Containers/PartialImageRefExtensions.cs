namespace HLabs.Containers;

public static class PartialImageRefExtensions {
#pragma warning disable CA1034
  extension( PartialImageRef imageRef ) {
#pragma warning restore CA1034
    /// <summary>
    /// Returns a new instance with a different cosmetic tag.
    /// </summary>
    public QualifiedImageRef Qualify( Tag? tag ) =>
      imageRef.With( tag ).Qualify();

    /// <summary>
    /// Returns a new instance with a different registry.
    /// </summary>
    public QualifiedImageRef Qualify( Registry? registry ) =>
      imageRef.With( registry ).Qualify();

    /// <summary>
    /// Returns a new instance with a different registry.
    /// </summary>
    public QualifiedImageRef Qualify( Registry registry, Namespace ns ) =>
      imageRef.With( registry, ns ).Qualify();

    /// <summary>
    /// Returns a new instance with a different namespace.
    /// </summary>
    public QualifiedImageRef Qualify( Namespace? ns ) =>
      // TODO remember registry/namespace requirement
      imageRef.With( ns ).Qualify();

    /// <summary>
    /// Returns a new instance with a different digest.
    /// </summary>
    public QualifiedImageRef Qualify( Digest? digest ) =>
      imageRef.With( digest ).Qualify();
  }
}