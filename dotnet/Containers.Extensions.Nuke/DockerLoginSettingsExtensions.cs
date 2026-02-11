using Nuke.Common.Tools.Docker;

namespace HLabs.Containers.Extensions.Nuke;

/// <summary>
/// Extension methods for <see cref="DockerLoginSettings"/> to work with strongly-typed registries.
/// </summary>
public static class DockerLoginSettingsExtensions {
  /// <summary>
  /// Sets the registry server to log in to using a <see cref="Registry"/>.
  /// </summary>
  /// <param name="settings">The Docker login settings.</param>
  /// <param name="registry">The registry to log in to.</param>
  /// <returns>The updated settings.</returns>
  public static DockerLoginSettings SetServer( this DockerLoginSettings settings, Registry registry ) {
    ArgumentNullException.ThrowIfNull( registry );
    return global::Nuke.Common.Tools.Docker.DockerLoginSettingsExtensions.SetServer( settings, registry.ToString() );
  }
}