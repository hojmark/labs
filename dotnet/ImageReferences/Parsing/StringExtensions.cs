namespace HLabs.ImageReferences;

/// <summary>
/// Extension methods for working with container image references from strings.
/// </summary>
public static class StringExtensions {
#pragma warning disable CA1034
  extension( string imageReference ) {
#pragma warning restore CA1034
    /// <summary>
    /// Creates a partial container image reference from a string.
    /// The string is parsed to extract registry, namespace, repository, tag, and digest components.
    /// </summary>
    /// <returns>A <see cref="PartialImageRef"/> parsed from the string.</returns>
    /// <exception cref="FormatException">Thrown when the image reference string is invalid.</exception>
    /// <example>
    /// <code>
    /// var image = "nginx:latest".Image();
    /// var image = "docker.io/library/nginx:1.25".Image();
    /// </code>
    /// </example>
    public PartialImageRef Image() => PartialImageRef.Parse( imageReference );
/*
    /// <summary>
    /// Creates a qualified container image reference.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the image reference is not in a canonical form.</exception>
    public QualifiedImageRef QualifiedImage() => PartialImageRef.Parse( imageReference ).Qualify();

    /// <summary>
    /// Creates a pinned container image reference. That is, a reference that is guaranteed to resolve to the same image (content-addressable).
    /// This is only possible if the image reference has a digest.
    /// </summary>
    public CanonicalImageRef CanonicalImage() => PartialImageRef.Parse( imageReference ).Canonicalize();*/
  }
}