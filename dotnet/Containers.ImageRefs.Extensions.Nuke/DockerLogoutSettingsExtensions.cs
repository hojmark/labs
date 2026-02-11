using HLabs.Containers.Components;
using Nuke.Common.Tools.Docker;

namespace HLabs.Containers.Extensions.Nuke;

public static class DockerLogoutSettingsExtensions {
  public static DockerLogoutSettings SetServer( this DockerLogoutSettings settings, Registry registry ) {
    ArgumentNullException.ThrowIfNull( registry );
    return global::Nuke.Common.Tools.Docker.DockerLogoutSettingsExtensions.SetServer( settings, registry.ToString() );
  }
}