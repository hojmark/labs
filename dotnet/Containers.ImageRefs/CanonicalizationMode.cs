namespace HLabs.Containers.ImageRefs;

/// <summary>
/// Specifies how to handle the tag when canonicalizing an image reference.
/// </summary>
public enum CanonicalizationMode {
  /// <summary>
  /// Excludes the tag from the canonical reference.
  /// The canonical reference will only include the digest.
  /// </summary>
  ExcludeTag,

  /// <summary>
  /// Maintains the tag in the canonical reference as a cosmetic label.
  /// The canonical reference will include both tag and digest.
  /// </summary>
  MaintainTag
}