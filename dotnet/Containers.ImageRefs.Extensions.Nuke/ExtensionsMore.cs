using HLabs.Containers.ImageRefs.Components;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;

namespace HLabs.Containers.ImageRefs.Extensions.Nuke;

public static class LocalDockerRepositoryExtensions {
  public static Digest GetDigest( this ImageId imageId ) {
    var output = DockerTasks.DockerImageLs( s => s
      .SetNoTrunc( true )
      .SetDigests( true )
      .SetProcessOutputLogging( false ) // Set to true to debug
      .SetFormat( "{{.ID}} {{.Digest}}" )
    );

    return GetDigestForImageId( output, imageId );
  }

  private static Digest GetDigestForImageId( IEnumerable<Output> lines, ImageId imageId ) {
    return GetDigestForImageId( lines.Select( l => l.Text ).ToArray(), imageId );
  }

  private static Digest GetDigestForImageId( string[] lines, ImageId targetImageId ) {
    foreach ( var line in lines ) {
      var parts = line.Split( ' ', 2 );
      if ( parts.Length != 2 ) {
        throw new FormatException( $"Unexpected line from docker image ls: '{line}'" );
      }

      var imageId = parts[0];
      var digest = parts[1];

      if ( imageId == targetImageId.ToString() ) {
        return new Digest( digest );
      }
    }

    throw new InvalidOperationException( $"No digest found for image ID {targetImageId}" );
  }
}