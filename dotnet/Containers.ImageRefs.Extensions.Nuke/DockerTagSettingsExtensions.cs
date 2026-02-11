using Nuke.Common.Tools.Docker;

namespace HLabs.Containers.Extensions.Nuke;

/// <summary>
/// Extension methods for <see cref="DockerTagSettings"/> to work with strongly-typed image references.
/// </summary>
public static class DockerTagSettingsExtensions {
  /// <summary>
  /// Sets the source image using an <see cref="ImageId"/>.
  /// </summary>
  /// <param name="settings">The Docker tag settings.</param>
  /// <param name="imageId">The source image ID.</param>
  /// <returns>The updated settings.</returns>
  public static DockerTagSettings SetSourceImage( this DockerTagSettings settings, ImageId imageId ) {
    ArgumentNullException.ThrowIfNull( imageId );
    return global::Nuke.Common.Tools.Docker.DockerTagSettingsExtensions.SetSourceImage( settings, imageId.ToString() );
  }

  /// <summary>
  /// Sets the source image using a <see cref="QualifiedImageRef"/>.
  /// </summary>
  /// <param name="settings">The Docker tag settings.</param>
  /// <param name="imageReference">The source image reference.</param>
  /// <returns>The updated settings.</returns>
  public static DockerTagSettings SetSourceImage( this DockerTagSettings settings, QualifiedImageRef imageReference ) {
    ArgumentNullException.ThrowIfNull( imageReference );
    return global::Nuke.Common.Tools.Docker.DockerTagSettingsExtensions.SetSourceImage(
      settings,
      imageReference.ToString()
    );
  }

  /// <summary>
  /// Sets the source image using a <see cref="CanonicalImageRef"/>.
  /// </summary>
  /// <param name="settings">The Docker tag settings.</param>
  /// <param name="imageReference">The source image reference.</param>
  /// <returns>The updated settings.</returns>
  public static DockerTagSettings SetSourceImage( this DockerTagSettings settings, CanonicalImageRef imageReference ) {
    ArgumentNullException.ThrowIfNull( imageReference );
    return global::Nuke.Common.Tools.Docker.DockerTagSettingsExtensions.SetSourceImage(
      settings,
      imageReference.ToString()
    );
  }

  /// <summary>
  /// Sets the target image using a <see cref="QualifiedImageRef"/>.
  /// </summary>
  /// <param name="settings">The Docker tag settings.</param>
  /// <param name="imageReference">The target image reference.</param>
  /// <returns>The updated settings.</returns>
  public static DockerTagSettings SetTargetImage( this DockerTagSettings settings, QualifiedImageRef imageReference ) {
    ArgumentNullException.ThrowIfNull( imageReference );
    return global::Nuke.Common.Tools.Docker.DockerTagSettingsExtensions.SetTargetImage(
      settings,
      imageReference.ToString()
    );
  }
}