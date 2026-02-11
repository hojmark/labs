using Nuke.Common.Tools.Docker;

namespace HLabs.Containers.Extensions.Nuke;

public static class DockerPushSettingsExtensions {
  public static DockerPushSettings SetName( this DockerPushSettings settings, QualifiedImageRef imageReference ) {
    ArgumentNullException.ThrowIfNull( imageReference );
    return global::Nuke.Common.Tools.Docker.DockerPushSettingsExtensions.SetName( settings, imageReference.ToString() );
  }
}