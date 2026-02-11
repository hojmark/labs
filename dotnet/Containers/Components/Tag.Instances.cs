namespace HLabs.Containers.Components;

public sealed partial record Tag {
  /// <summary>
  /// Represents the "latest" tag, commonly used as the default tag for container images.
  /// </summary>
  public static readonly Tag Latest = new("latest");
}