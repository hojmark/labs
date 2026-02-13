namespace HLabs.ImageReferences;

/// <summary>
/// Extension methods for <see cref="PartialImageRef"/> that provide convenient qualification with different components.
/// </summary>
public static class PartialImageRefExtensions {
#pragma warning disable CA1034
  extension( PartialImageRef imageRef ) {
#pragma warning restore CA1034
    // Single component overloads - most common use cases

    /// <summary>
    /// Qualifies the image reference with a different tag, filling in defaults for missing components.
    /// </summary>
    /// <param name="tag">The tag to use.</param>
    /// <returns>A qualified image reference.</returns>
    public QualifiedImageRef Qualify( Tag? tag ) =>
      imageRef.With( tag ).Qualify();

    /// <summary>
    /// Qualifies the image reference with a different registry, filling in defaults for missing components.
    /// </summary>
    /// <param name="registry">The registry to use.</param>
    /// <returns>A qualified image reference.</returns>
    public QualifiedImageRef Qualify( Registry? registry ) =>
      imageRef.With( registry ).Qualify();

    // Multi-component overloads - practical combinations

    /// <summary>
    /// Qualifies the image reference with a different registry and namespace, filling in defaults for missing components.
    /// Common pattern for switching to organization registries like ghcr.io/myorg or registry.io/myorg.
    /// </summary>
    /// <param name="registry">The registry to use.</param>
    /// <param name="ns">The namespace to use.</param>
    /// <returns>A qualified image reference.</returns>
    public QualifiedImageRef Qualify( Registry registry, Namespace ns ) =>
      imageRef.With( registry, ns ).Qualify();

    /// <summary>
    /// Qualifies the image reference with tag and registry, filling in defaults for missing components.
    /// Common pattern for versioning images across different registries.
    /// </summary>
    /// <param name="tag">The tag to use.</param>
    /// <param name="registry">The registry to use.</param>
    /// <returns>A qualified image reference.</returns>
    public QualifiedImageRef Qualify( Tag tag, Registry registry ) =>
      imageRef.With( tag ).With( registry ).Qualify();

    /// <summary>
    /// Qualifies the image reference with tag, registry, and namespace, filling in defaults for missing components.
    /// Complete specification for publishing versioned images to organization registries.
    /// </summary>
    /// <param name="tag">The tag to use.</param>
    /// <param name="registry">The registry to use.</param>
    /// <param name="ns">The namespace to use.</param>
    /// <returns>A qualified image reference.</returns>
    public QualifiedImageRef Qualify( Tag tag, Registry registry, Namespace ns ) =>
      imageRef.With( tag ).With( registry ).With( ns ).Qualify();

    // Canonicalization overloads - digest is required

    /// <summary>
    /// Qualifies and canonicalizes the image reference with a digest, creating an immutable reference.
    /// More terse than .With(digest).Qualify().Canonicalize().
    /// </summary>
    /// <param name="digest">The digest to pin the reference with.</param>
    /// <returns>A canonical (immutable) image reference.</returns>
    public CanonicalImageRef Canonicalize( Digest digest ) =>
      imageRef.With( digest ).Qualify().Canonicalize();

    /// <summary>
    /// Qualifies and canonicalizes the image reference with registry and digest, creating an immutable reference.
    /// Useful for pinning images from a specific registry.
    /// </summary>
    /// <param name="registry">The registry to use.</param>
    /// <param name="digest">The digest to pin the reference with.</param>
    /// <returns>A canonical (immutable) image reference.</returns>
    public CanonicalImageRef Canonicalize( Registry registry, Digest digest ) =>
      imageRef.With( registry ).With( digest ).Qualify().Canonicalize();

    /// <summary>
    /// Qualifies and canonicalizes the image reference with registry, namespace, and digest, creating an immutable reference.
    /// Useful for pinning images from organization registries.
    /// </summary>
    /// <param name="registry">The registry to use.</param>
    /// <param name="ns">The namespace to use.</param>
    /// <param name="digest">The digest to pin the reference with.</param>
    /// <returns>A canonical (immutable) image reference.</returns>
    public CanonicalImageRef Canonicalize( Registry registry, Namespace ns, Digest digest ) =>
      imageRef.With( registry ).With( ns ).With( digest ).Qualify().Canonicalize();
  }
}