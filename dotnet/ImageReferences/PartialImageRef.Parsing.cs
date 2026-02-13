using System.Text.RegularExpressions;

namespace HLabs.ImageReferences;

public sealed partial record PartialImageRef {
  [GeneratedRegex(
    @"^(?:(?<registry>[^/]+?)/)?(?:(?<namespace>[^/]+?)/)?(?<repository>[^:@]+)(?::(?<tag>[^@]+))?(?:@(?<digest>.+))?$",
    RegexOptions.CultureInvariant
  )]
  private static partial Regex ImageRefRegex();

  /// <summary>
  /// Parses a string representation of an image reference.
  /// </summary>
  /// <param name="imageReference">The string to parse.</param>
  /// <returns>A parsed <see cref="PartialImageRef"/>.</returns>
  /// <exception cref="ArgumentNullException">Thrown when imageReference is null.</exception>
  /// <exception cref="FormatException">Thrown when imageReference is not in a valid format.</exception>
  public static PartialImageRef Parse( string imageReference ) {
    ArgumentNullException.ThrowIfNull( imageReference );

    var match = ImageRefRegex().Match( imageReference.Trim() );
    if ( !match.Success ) {
      throw new FormatException( $"Invalid image reference: '{imageReference}'" );
    }

    var registry = string.IsNullOrEmpty( match.Groups["registry"].Value )
      ? null
      : new Registry( match.Groups["registry"].Value );
    var @namespace = string.IsNullOrEmpty( match.Groups["namespace"].Value )
      ? null
      : new Namespace( match.Groups["namespace"].Value );
    var repository = new Repository( match.Groups["repository"].Value );
    var tag = string.IsNullOrEmpty( match.Groups["tag"].Value ) ? null : new Tag( match.Groups["tag"].Value );
    var digest = string.IsNullOrEmpty( match.Groups["digest"].Value )
      ? null
      : new Digest( match.Groups["digest"].Value );

    return new PartialImageRef( repository, tag, registry, @namespace, digest );
  }

  /// <summary>
  /// Tries to parse a string representation of an image reference.
  /// </summary>
  /// <param name="input">The string to parse.</param>
  /// <param name="reference">When this method returns, contains the parsed reference if parsing succeeded, or null if it failed.</param>
  /// <returns>true if parsing succeeded; otherwise, false.</returns>
  public static bool TryParse( string? input, out PartialImageRef? reference ) {
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