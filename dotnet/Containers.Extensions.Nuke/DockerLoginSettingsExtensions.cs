using Nuke.Common.Tools.Docker;

namespace HLabs.Containers.Extensions.Nuke;

public static class DockerLoginSettingsExtensions {
  public static DockerLoginSettings SetServer( this DockerLoginSettings settings, Registry registry ) {
    ArgumentNullException.ThrowIfNull( registry );
    return global::Nuke.Common.Tools.Docker.DockerLoginSettingsExtensions.SetServer( settings, registry.ToString() );
  }
}