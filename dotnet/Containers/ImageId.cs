using System.Text.RegularExpressions;

namespace HLabs.Containers;

/// <summary>
/// Represents a local container image ID (e.g., "sha256:a3ed95caeb02...").
/// Image IDs are used to uniquely identify images in the local Docker daemon.
/// </summary>
/// <example>
/// <code>
/// var imageId = new ImageId("sha256:a3ed95caeb02ffe68cdd9fd84406680ae93d633cb16422d00e8a7c22955b46d4");
/// var imageId = new ImageId("a3ed95caeb02ffe68cdd9fd84406680ae93d633cb16422d00e8a7c22955b46d4");  // Algorithm prefix optional
/// ImageId imageId = "sha256:abc...";  // Implicit conversion from string
/// </code>
/// </example>
public sealed record ImageId {
  private const string DefaultAlgorithm = "sha256";

  private static readonly Regex Sha256Regex =
    new(@"^[0-9a-fA-F]{64}$", RegexOptions.Compiled);

  private string Value {
    get;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ImageId"/> class.
  /// </summary>
  /// <param name="value">The image ID value. Can be in format "sha256:hash" or just "hash". Hash must be a valid 64-character hexadecimal SHA-256.</param>
  /// <exception cref="ArgumentException">Thrown when value is null, empty, contains whitespace, uses an unsupported algorithm, or has an invalid hash format.</exception>
  public ImageId( string value ) {
    if ( string.IsNullOrWhiteSpace( value ) ) {
      throw new ArgumentException( "Image ID cannot be null or empty", nameof(value) );
    }

    if ( value.Trim().Length != value.Length ) {
      throw new ArgumentException( "Image ID contains leading/trailing whitespace", nameof(value) );
    }

    string digest;

    if ( value.Contains( ':', StringComparison.Ordinal ) ) {
      var parts = value.Split( ':', 2 );

      var algorithm = parts[0];
      digest = parts[1];

      if ( !algorithm.Equals( DefaultAlgorithm, StringComparison.OrdinalIgnoreCase ) ) {
        throw new ArgumentException(
          $"Unsupported image ID algorithm '{algorithm}'. Only sha256 is supported.",
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
  /// Implicitly converts a string to an <see cref="ImageId"/>.
  /// </summary>
  /// <param name="value">The image ID value.</param>
  public static implicit operator ImageId( string value ) => FromString( value );

  /// <summary>
  /// Creates an <see cref="ImageId"/> from a string value.
  /// </summary>
  /// <param name="value">The image ID value.</param>
  /// <returns>A new <see cref="ImageId"/> instance.</returns>
  /// <exception cref="ArgumentException">Thrown when value is invalid.</exception>
  public static ImageId FromString( string value ) {
    return new(value);
  }

  /// <summary>
  /// Returns the string representation of this image ID.
  /// </summary>
  /// <returns>The image ID in format "sha256:hash" with lowercase hash.</returns>
  public override string ToString() => Value;
}