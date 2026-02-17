using Nuke.Common.Tools.Docker;

namespace HLabs.ImageReferences.Extensions.Nuke;

/// <summary>
/// Extension methods for <see cref="DockerBuildSettings"/> to work with strongly-typed image references.
/// </summary>
public static class DockerBuildSettingsExtensions {
  /// <summary>
  /// Sets the image tag using a <see cref="QualifiedImageRef"/>.
  /// </summary>
  /// <param name="settings">The Docker build settings.</param>
  /// <param name="imageReference">The image reference to tag.</param>
  /// <returns>The updated settings.</returns>
  public static DockerBuildSettings SetTag( this DockerBuildSettings settings, QualifiedImageRef imageReference ) {
    ArgumentNullException.ThrowIfNull( imageReference );
    return global::Nuke.Common.Tools.Docker.DockerBuildSettingsExtensions.SetTag( settings, imageReference.ToString() );
  }
}