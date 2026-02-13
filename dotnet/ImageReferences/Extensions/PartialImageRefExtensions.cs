namespace HLabs.ImageReferences;

/// <summary>
/// Extension methods for <see cref="PartialImageRef"/> that provide convenient qualification and canonicalization.
/// </summary>
public static class PartialImageRefExtensions {
#pragma warning disable CA1034
  extension( PartialImageRef imageRef ) {
#pragma warning restore CA1034
    // Single component overloads

    /// <summary>
    /// Qualifies the image reference with the specified registry.
    /// </summary>
    /// <param name="registry">The registry.</param>
    /// <returns>A qualified image reference.</returns>
    public QualifiedImageRef Qualify( Registry? registry ) =>
      imageRef.With( registry ).Qualify();

    /// <summary>
    /// Qualifies the image reference with the specified tag.
    /// </summary>
    /// <param name="tag">The tag.</param>
    /// <returns>A qualified image reference.</returns>
    public QualifiedImageRef Qualify( Tag? tag ) =>
      imageRef.With( tag ).Qualify();

    // Multi-component overloads

    /// <summary>
    /// Qualifies the image reference with the specified registry and namespace.
    /// </summary>
    /// <param name="registry">The registry.</param>
    /// <param name="ns">The namespace.</param>
    /// <returns>A qualified image reference.</returns>
    public QualifiedImageRef Qualify( Registry registry, Namespace ns ) =>
      imageRef.With( registry, ns ).Qualify();

    /// <summary>
    /// Qualifies the image reference with the specified registry and tag.
    /// </summary>
    /// <param name="registry">The registry.</param>
    /// <param name="tag">The tag.</param>
    /// <returns>A qualified image reference.</returns>
    public QualifiedImageRef Qualify( Registry registry, Tag tag ) =>
      imageRef.With( registry ).With( tag ).Qualify();

    /// <summary>
    /// Qualifies the image reference with the specified registry, namespace, and tag.
    /// </summary>
    /// <param name="registry">The registry.</param>
    /// <param name="ns">The namespace.</param>
    /// <param name="tag">The tag.</param>
    /// <returns>A qualified image reference.</returns>
    public QualifiedImageRef Qualify( Registry registry, Namespace ns, Tag tag ) =>
      imageRef.With( registry, ns ).With( tag ).Qualify();

    // Canonicalization overloads

    /// <summary>
    /// Canonicalizes the image reference with the specified digest.
    /// </summary>
    /// <param name="digest">The digest.</param>
    /// <returns>A canonical image reference.</returns>
    public CanonicalImageRef Canonicalize( Digest digest ) =>
      imageRef.With( digest ).Qualify().Canonicalize();

    /// <summary>
    /// Canonicalizes the image reference with the specified registry and digest.
    /// </summary>
    /// <param name="registry">The registry.</param>
    /// <param name="digest">The digest.</param>
    /// <returns>A canonical image reference.</returns>
    public CanonicalImageRef Canonicalize( Registry registry, Digest digest ) =>
      imageRef.With( registry ).With( digest ).Qualify().Canonicalize();

    /// <summary>
    /// Canonicalizes the image reference with the specified registry, namespace, and digest.
    /// </summary>
    /// <param name="registry">The registry.</param>
    /// <param name="ns">The namespace.</param>
    /// <param name="digest">The digest.</param>
    /// <returns>A canonical image reference.</returns>
    public CanonicalImageRef Canonicalize( Registry registry, Namespace ns, Digest digest ) =>
      imageRef.With( registry, ns ).With( digest ).Qualify().Canonicalize();
  }
}