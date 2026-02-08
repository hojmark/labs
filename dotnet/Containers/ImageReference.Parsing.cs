using System.Text.RegularExpressions;

namespace HLabs.Containers;

public sealed partial record ImageReference {
  [GeneratedRegex(
    @"^(?:(?<registry>[^/]+?)/)?(?:(?<namespace>[^/]+?)/)?(?<repository>[^:@]+)(?::(?<tag>[^@]+))?(?:@(?<digest>.+))?$",
    RegexOptions.CultureInvariant
  )]
  private static partial Regex ImageRefRegex();

  public static ImageReference Parse( string imageReference ) {
    ArgumentNullException.ThrowIfNull( imageReference );

    var match = ImageRefRegex().Match( imageReference.Trim() );
    if ( !match.Success ) {
      throw new FormatException( $"Invalid image reference: '{imageReference}'" );
    }

    var registryStr = match.Groups["registry"].Value;
    var namespaceStr = match.Groups["namespace"].Value;
    var repositoryStr = match.Groups["repository"].Value;
    var tagStr = match.Groups["tag"].Value;
    var digestStr = match.Groups["digest"].Value;

    var registry = string.IsNullOrEmpty( registryStr )
      ? Registry.DockerHub
      : new Registry( registryStr );

    var @namespace = string.IsNullOrEmpty( namespaceStr )
      ? ( registry == Registry.DockerHub ? Namespace.Library : null )
      : new Namespace( namespaceStr );

    var repository = new Repository( repositoryStr );

    var tag = string.IsNullOrEmpty( tagStr ) ? Tag.Latest : new Tag( tagStr );

    var digest = string.IsNullOrEmpty( digestStr )
      ? null
      : new Digest( digestStr );

    return new ImageReference( repository, tag, registry, @namespace, digest );
  }

  public static bool TryParse( string? input, out ImageReference? reference ) {
    try {
      if ( input is null ) {
        reference = null;
        return false;
      }

      reference = Parse( input );
      return true;
    }
    // Justification: ok for TryParse pattern
#pragma warning disable CA1031
    catch {
#pragma warning restore CA1031
      reference = null;
      return false;
    }
  }
}