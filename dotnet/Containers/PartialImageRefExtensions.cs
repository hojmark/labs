namespace HLabs.Containers;

/// <summary>
/// Extension methods for <see cref="PartialImageRef"/> that provide convenient qualification with different components.
/// </summary>
public static class PartialImageRefExtensions {
#pragma warning disable CA1034
  extension( PartialImageRef imageRef ) {
#pragma warning restore CA1034
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

    /// <summary>
    /// Qualifies the image reference with a different registry and namespace, filling in defaults for missing components.
    /// </summary>
    /// <param name="registry">The registry to use.</param>
    /// <param name="ns">The namespace to use.</param>
    /// <returns>A qualified image reference.</returns>
    public QualifiedImageRef Qualify( Registry registry, Namespace ns ) =>
      imageRef.With( registry, ns ).Qualify();

    /// <summary>
    /// Qualifies the image reference with a different namespace, filling in defaults for missing components.
    /// </summary>
    /// <param name="ns">The namespace to use.</param>
    /// <returns>A qualified image reference.</returns>
    public QualifiedImageRef Qualify( Namespace? ns ) =>
      imageRef.With( ns ).Qualify();

    /// <summary>
    /// Qualifies the image reference with a different digest, filling in defaults for missing components.
    /// </summary>
    /// <param name="digest">The digest to use.</param>
    /// <returns>A qualified image reference.</returns>
    public QualifiedImageRef Qualify( Digest? digest ) =>
      imageRef.With( digest ).Qualify();
  }
}