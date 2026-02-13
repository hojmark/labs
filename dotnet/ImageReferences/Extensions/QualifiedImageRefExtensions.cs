namespace HLabs.ImageReferences;

/// <summary>
/// Extension methods for <see cref="QualifiedImageRef"/> that provide convenient canonicalization with different components.
/// </summary>
public static class QualifiedImageRefExtensions {
#pragma warning disable CA1034
  extension( QualifiedImageRef imageRef ) {
#pragma warning restore CA1034
    /// <summary>
    /// Canonicalizes the image reference with a digest, creating an immutable reference.
    /// More terse than .With(digest).Canonicalize().
    /// </summary>
    /// <param name="digest">The digest to pin the reference with.</param>
    /// <returns>A canonical (immutable) image reference.</returns>
    public CanonicalImageRef Canonicalize( Digest digest ) =>
      imageRef.With( digest ).Canonicalize();

    /// <summary>
    /// Canonicalizes the image reference with registry and digest, creating an immutable reference.
    /// Useful for pinning images from a specific registry.
    /// </summary>
    /// <param name="registry">The registry to use.</param>
    /// <param name="digest">The digest to pin the reference with.</param>
    /// <returns>A canonical (immutable) image reference.</returns>
    public CanonicalImageRef Canonicalize( Registry registry, Digest digest ) =>
      imageRef.With( registry ).With( digest ).Canonicalize();

    /// <summary>
    /// Canonicalizes the image reference with registry, namespace, and digest, creating an immutable reference.
    /// Useful for pinning images from organization registries.
    /// </summary>
    /// <param name="registry">The registry to use.</param>
    /// <param name="ns">The namespace to use.</param>
    /// <param name="digest">The digest to pin the reference with.</param>
    /// <returns>A canonical (immutable) image reference.</returns>
    public CanonicalImageRef Canonicalize( Registry registry, Namespace ns, Digest digest ) =>
      imageRef.With( registry, ns ).With( digest ).Canonicalize();
  }
}