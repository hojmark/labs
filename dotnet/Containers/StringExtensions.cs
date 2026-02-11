namespace HLabs.Containers;

public static class StringExtensions {
#pragma warning disable CA1034
  extension( string imageReference ) {
#pragma warning restore CA1034
    /// <summary>
    /// Creates an unqualified container image reference.
    /// </summary>
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