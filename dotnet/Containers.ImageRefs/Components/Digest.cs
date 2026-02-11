using System.Text.RegularExpressions;

namespace HLabs.Containers.Components;

/// <summary>
/// Represents a digest for a container image e.g., <c>sha256:a3ed95caeb02ffe68cdd9fd84406680ae93d633cb16422d00e8a7c22955b46d4</c>.
/// <p>
/// Digests are immutable identifiers that uniquely identify image content i.e., they are content-addressable.
/// </p>
/// </summary>
/// <example>
/// Using implicit string conversion:
/// <code>
/// Digest digest = "sha256:abc123";
/// </code>
/// Using constructor
/// <code>
/// var digest = new Digest("sha256:abc123");
/// </code>
/// Algorithm prefix is optional:
/// <code>
/// Digest digest1 = "abc123";
/// </code>
/// </example>
public sealed record Digest {
  private const string DefaultAlgorithm = "sha256";

  private static readonly Regex Sha256Regex =
    new(@"^[0-9a-fA-F]{64}$", RegexOptions.Compiled);

  private string Value {
    get;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="Digest"/> class.
  /// </summary>
  /// <param name="value">The digest value. Can be in format <c>sha256:abc123</c> or just <c>abc123</c>.</param>
  /// <exception cref="ArgumentException">Thrown when <see cref="value"/> is not a valid digest.</exception>
  public Digest( string value ) {
    if ( string.IsNullOrWhiteSpace( value ) ) {
      throw new ArgumentException( "Digest cannot be null or empty", nameof(value) );
    }

    if ( value.Trim().Length != value.Length ) {
      throw new ArgumentException( "Digest contains leading/trailing whitespace", nameof(value) );
    }

    string digest;

    if ( value.Contains( ':', StringComparison.Ordinal ) ) {
      var parts = value.Split( ':', 2 );

      var algorithm = parts[0];
      digest = parts[1];

      if ( !algorithm.Equals( DefaultAlgorithm, StringComparison.OrdinalIgnoreCase ) ) {
        throw new ArgumentException(
          $"Unsupported digest algorithm '{algorithm}'. Only sha256 is supported.",
          nameof(value)
        );
      }
    }
    else {
      digest = value;
    }

    if ( !Sha256Regex.IsMatch( digest ) ) {
      throw new ArgumentException(
        "Image ID digest must be a valid 64-character hexadecimal SHA-256 hash.",
        nameof(value)
      );
    }

    // Conventionally lowercase
#pragma warning disable CA1308
    Value = $"{DefaultAlgorithm}:{digest.ToLowerInvariant()}";
#pragma warning restore CA1308
  }

  /// <summary>
  /// Implicitly converts a string to a <see cref="Digest"/>.
  /// </summary>
  /// <param name="value">The digest value.</param>
  public static implicit operator Digest( string value ) => FromString( value );

  /// <summary>
  /// Creates a <see cref="Digest"/> from a string value.
  /// </summary>
  /// <param name="value">The digest value.</param>
  /// <returns>A new <see cref="Digest"/> instance.</returns>
  /// <exception cref="ArgumentException">Thrown when <see cref="value"/> is not a valid digest.</exception>
  public static Digest FromString( string value ) {
    return new(value);
  }

  /// <summary>
  /// Returns the string representation of this digest.
  /// </summary>
  /// <returns>The digest in format <c>sha256:abc123</c>.</returns>
  public override string ToString() => Value;
}