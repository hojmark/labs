using Nuke.Common.Tools.Docker;

namespace HLabs.Containers.Extensions.Nuke;

public static class DockerTagSettingsExtensions {
  /*public static DockerBuildSettings SetTag( this DockerBuildSettings settings, QualifiedImageRef imageReference ) {
    ArgumentNullException.ThrowIfNull( imageReference );
    return DockerBuildSettingsExtensions.SetTag( settings, imageReference.ToString() );
  }*/

  public static DockerTagSettings SetSourceImage( this DockerTagSettings settings, ImageId imageId ) {
    ArgumentNullException.ThrowIfNull( imageId );
    return global::Nuke.Common.Tools.Docker.DockerTagSettingsExtensions.SetSourceImage( settings, imageId.ToString() );
  }

  public static DockerTagSettings SetSourceImage( this DockerTagSettings settings, QualifiedImageRef imageReference ) {
    ArgumentNullException.ThrowIfNull( imageReference );
    return global::Nuke.Common.Tools.Docker.DockerTagSettingsExtensions.SetSourceImage( settings, imageReference.ToString() );
  }

  public static DockerTagSettings SetSourceImage( this DockerTagSettings settings, CanonicalImageRef imageReference ) {
    ArgumentNullException.ThrowIfNull( imageReference );
    return global::Nuke.Common.Tools.Docker.DockerTagSettingsExtensions.SetSourceImage( settings, imageReference.ToString() );
  }

  public static DockerTagSettings SetTargetImage( this DockerTagSettings settings, QualifiedImageRef imageReference ) {
    ArgumentNullException.ThrowIfNull( imageReference );
    return global::Nuke.Common.Tools.Docker.DockerTagSettingsExtensions.SetTargetImage( settings, imageReference.ToString() );
  }
}