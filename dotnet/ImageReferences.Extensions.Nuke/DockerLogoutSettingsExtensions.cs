using HLabs.ImageReferences.Components;
using Nuke.Common.Tools.Docker;

namespace HLabs.ImageReferences.Extensions.Nuke;

/// <summary>
/// Extension methods for <see cref="DockerLogoutSettings"/> to work with type-safe <see cref="Registry"/> objects.
/// </summary>
public static class DockerLogoutSettingsExtensions {
  /// <summary>
  /// Sets the server (registry) for the docker logout command.
  /// </summary>
  /// <param name="settings">The docker logout settings.</param>
  /// <param name="registry">The registry to logout from.</param>
  /// <returns>The modified settings.</returns>
  public static DockerLogoutSettings SetServer( this DockerLogoutSettings settings, Registry registry ) {
    ArgumentNullException.ThrowIfNull( registry );
    return global::Nuke.Common.Tools.Docker.DockerLogoutSettingsExtensions.SetServer( settings, registry.ToString() );
  }
}