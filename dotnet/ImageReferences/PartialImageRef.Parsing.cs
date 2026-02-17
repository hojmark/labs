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

    var registryCapture = match.Groups["registry"].Value;
    var namespaceCapture = match.Groups["namespace"].Value;

    Registry? registry;
    Namespace? @namespace;

    // Apply Docker naming convention: if the first component (registry capture) doesn't look like a registry
    // (no dots, no colons, not "localhost"), treat it as a namespace instead.
#pragma warning disable CA1307
    if ( !string.IsNullOrEmpty( registryCapture ) &&
         !registryCapture.Contains( '.' ) &&
         !registryCapture.Contains( ':' ) &&
         !registryCapture.Equals( "localhost", StringComparison.OrdinalIgnoreCase ) ) {
#pragma warning restore CA1307
      // First component is actually a namespace, not a registry
      registry = null;
      @namespace = new Namespace( registryCapture );
    }
    else {
      registry = string.IsNullOrEmpty( registryCapture ) ? null : new Registry( registryCapture );
      @namespace = string.IsNullOrEmpty( namespaceCapture ) ? null : new Namespace( namespaceCapture );
    }

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