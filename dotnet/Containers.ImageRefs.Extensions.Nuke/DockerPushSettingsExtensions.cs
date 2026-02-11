using Nuke.Common.Tools.Docker;

namespace HLabs.Containers.ImageRefs.Extensions.Nuke;

/// <summary>
/// Extension methods for <see cref="DockerPushSettings"/> to work with strongly-typed image references.
/// </summary>
public static class DockerPushSettingsExtensions {
  /// <summary>
  /// Sets the image name to push using a <see cref="QualifiedImageRef"/>.
  /// </summary>
  /// <param name="settings">The Docker push settings.</param>
  /// <param name="imageReference">The image reference to push.</param>
  /// <returns>The updated settings.</returns>
  public static DockerPushSettings SetName( this DockerPushSettings settings, QualifiedImageRef imageReference ) {
    ArgumentNullException.ThrowIfNull( imageReference );
    return global::Nuke.Common.Tools.Docker.DockerPushSettingsExtensions.SetName( settings, imageReference.ToString() );
  }
}