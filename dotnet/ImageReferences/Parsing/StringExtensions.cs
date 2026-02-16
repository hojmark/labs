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

    /// <summary>
    /// Creates a qualified container image reference from a string.
    /// </summary>
    /// <returns>A <see cref="QualifiedImageRef"/>.</returns>
    /// <exception cref="FormatException">Thrown when the image reference string is invalid.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the image reference cannot be qualified.</exception>
    public QualifiedImageRef QualifiedImage() => PartialImageRef.Parse( imageReference ).Qualify();

    /// <summary>
    /// Creates a canonical container image reference from a string.
    /// This is only possible if the image reference has a digest.
    /// </summary>
    /// <returns>A <see cref="CanonicalImageRef"/>.</returns>
    /// <exception cref="FormatException">Thrown when the image reference string is invalid.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the image reference cannot be canonicalized.</exception>
    public CanonicalImageRef CanonicalImage() => PartialImageRef.Parse( imageReference ).Canonicalize();
  }
}