namespace HLabs.ImageReferences;

/// <summary>
/// Specifies how to handle missing components when qualifying a partial image reference.
/// </summary>
public enum QualificationMode {
  /// <summary>
  /// Requires all components to be explicitly provided.
  /// </summary>
  RequireAll,

  /// <summary>
  /// Fills in missing components with defaults (e.g., docker.io for registry, library for namespace on DockerHub).
  /// </summary>
  DefaultFilling
}