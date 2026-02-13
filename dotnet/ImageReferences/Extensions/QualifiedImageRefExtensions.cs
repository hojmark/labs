namespace HLabs.ImageReferences;

/// <summary>
/// Extension methods for <see cref="QualifiedImageRef"/> that provide convenient canonicalization.
/// </summary>
public static class QualifiedImageRefExtensions {
#pragma warning disable CA1034
  extension( QualifiedImageRef imageRef ) {
#pragma warning restore CA1034
    /// <summary>
    /// Canonicalizes the image reference with the specified digest.
    /// </summary>
    /// <param name="digest">The digest.</param>
    /// <returns>A canonical image reference.</returns>
    public CanonicalImageRef Canonicalize( Digest digest ) =>
      imageRef.With( digest ).Canonicalize();

    /// <summary>
    /// Canonicalizes the image reference with the specified registry and digest.
    /// </summary>
    /// <param name="registry">The registry.</param>
    /// <param name="digest">The digest.</param>
    /// <returns>A canonical image reference.</returns>
    public CanonicalImageRef Canonicalize( Registry registry, Digest digest ) =>
      imageRef.With( registry ).With( digest ).Canonicalize();

    /// <summary>
    /// Canonicalizes the image reference with the specified registry, namespace, and digest.
    /// </summary>
    /// <param name="registry">The registry.</param>
    /// <param name="ns">The namespace.</param>
    /// <param name="digest">The digest.</param>
    /// <returns>A canonical image reference.</returns>
    public CanonicalImageRef Canonicalize( Registry registry, Namespace ns, Digest digest ) =>
      imageRef.With( registry, ns ).With( digest ).Canonicalize();
  }
}