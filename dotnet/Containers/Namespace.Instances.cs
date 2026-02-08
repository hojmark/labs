namespace HLabs.Containers;

public sealed partial record Namespace {
  /// <summary>
  /// Default DockerHub namespace.
  /// </summary>
  internal static readonly Namespace Library = new("library");
}